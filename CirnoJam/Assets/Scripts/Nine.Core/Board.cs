using System;
using System.Linq;
using System.Collections.Generic;

namespace Nine.Core
{
	public class Board : IUpdatable
	{
		public const int ROW_WIDTH = 6;
		public const int COLUMN_HEIGHT = 14;

		private readonly Random rand = new Random();
		private bool playMatchSound = false;

		public float ScrollProgress { get; set; }
		public float ScrollSpeed { get; private set; }
		public uint Score { get; private set; } = 0;
		public int BlocksCleared { get; private set; } = 0;
		public Block[][] Blocks { get; set; } // Blocks are accessed by Blocks[y][x]
		public List<Nine.Core.MatchedSet> MatchedSets { get; set; }

		public bool GameOver
		{
			get
			{
				return Blocks[COLUMN_HEIGHT - 1].Any(block => block != null);
			}
		}

		public int StackHeight
		{
			get
			{
				for(int y = COLUMN_HEIGHT - 1; y < COLUMN_HEIGHT; y--)
				{
					if (Blocks[y].Any((block) => block != null))
					{
						return y;
					}
				}

				return 0;
			}
		}

		public IEnumerable<Block> AllBlocks
		{
			get
			{
				List<Block> allBlocks = new List<Block>();

				for (int y = 0; y < COLUMN_HEIGHT; y++)
				{
					for (int x = 0; x < ROW_WIDTH; x++)
					{
						if(Blocks[y][x] != null)
						{
							allBlocks.Add(Blocks[y][x]);
						}
					}
				}

				return allBlocks;
			}
		}

		public bool IsFrozen
		{
			get
			{
				return  AllBlocks.Any
				(
					(block) => 
					{
						return new Nine.Core.BlockStatus[] { Nine.Core.BlockStatus.Clearing }
							.Contains(block.Status);
					}
				);
			}
		}

		public Board(float scrollSpeed) //Constructor
		{
			this.MatchedSets = new List<MatchedSet>();
			this.ScrollSpeed = scrollSpeed;
			this.Initialize();
		}

		public void Initialize()
		{
			Blocks = new Block[COLUMN_HEIGHT][];

			for(int row = 0; row < Blocks.Length; row++)
			{
				if (row < Blocks.Length / 2)
				{
					Blocks[row] = GetNewRow(row);
				}
				else
				{
					Blocks[row] = new Block[ROW_WIDTH];
				}

				foreach(var block in Blocks[row])
				{
					if (block != null)
					{
						if(block.Position.Y > 0)
						{
							block.Status = Nine.Core.BlockStatus.Active;
						}
						else
						{
							block.Status = Nine.Core.BlockStatus.Inactive;
						}
					}
				}
			}

			var matchBlocks = GetMatchBlocks();

			foreach (var block in matchBlocks)
			{
				ClearBlock(block);
			}

			Score = 0;
			BlocksCleared = 0;

			AdjustScrollSpeed();
		}

		public void ShiftBlocksUp()
		{
			ScrollProgress--;

			// shift UP
			for (int row = Blocks.Length - 1; row >= 0; row--)
			{
				// replace bottom row with some fresh blocks
				if(row == 0)
				{
					Blocks[row] = GetNewRow(0);
				}
				else
				{
					Blocks[row] = Blocks[row - 1]; // TODO: ShiftRowUp(row) ?
					for (int x = 0; x < ROW_WIDTH; x++)
					{
						if (Blocks[row][x] != null)
						{
							Blocks[row][x].Position = (Blocks[row][x].Position.X, row);

							if(row == 1)
							{
								Blocks[row][x].Status = Nine.Core.BlockStatus.Active;
							}
						}
					}
				}
			}

			ProcessMatches();
		}

		public Block[] GetNewRow(int y)
		{
			var row = new Block[ROW_WIDTH];
			BlockType? lastLastType = null;
			BlockType? lastType = null;

			for (int i = 0; i < row.Length; i++)
			{
				BlockType? thisType = null;

				while (thisType == null || (lastType != null && lastLastType != null && thisType == lastType && thisType == lastLastType))
				{
					thisType = (BlockType)rand.Next(Enum.GetNames(typeof(BlockType)).Length - 1); //remember to add colors before garbage in the enum
				}

				lastLastType = lastType;
				lastType = thisType;

				row[i] = new Block(i, y, thisType.Value, this);
			}

			return row;
		}

		private void SwapBlocks((int X, int Y) pointA, (int X, int Y) pointB)
		{
			var blockA = Blocks[pointA.Y][pointA.X];
			var blockB = Blocks[pointB.Y][pointB.X];

			// TODO: account for animation/state transition time
			// maybe tell the blocks that they are in the swapping state, and where to?
			if (blockA != null)
			{
				blockA.Position = pointB;
			}
			if (blockB != null)
			{
				blockB.Position = pointA;
			}

			Blocks[pointA.Y][pointA.X] = blockB;
			Blocks[pointB.Y][pointB.X] = blockA;
		}

		public void Swap((int X, int Y) pointA, (int X, int Y) pointB)
		{
			SwapBlocks(pointA, pointB);
			FillGaps();
		}

		public void Update(float deltaTime)
		{
			foreach (var set in MatchedSets)
			{
				set.Update(deltaTime);
			}

			FillGaps();

			MatchedSets.RemoveAll(set => set.IsCleared);

			if (!IsFrozen)
			{
				ScrollProgress += deltaTime * ScrollSpeed;
			}
		}

		public Block GetBlock((int X, int Y) position)
		{
			return Blocks[position.Y][position.X];
		}

		Block GetBlock(int x, int y)
		{
			if(IsInPlayableBoard(x, y))
			{
				return Blocks[y][x];
			}

			return null;
		}

		private bool IsInPlayableBoard(int x, int y)
		{
			return (x >= 0 && y >= 1 && x < ROW_WIDTH && y < COLUMN_HEIGHT);
		}

		private void FillGap(int x, int y)
		{
			SwapBlocks((x, y), (x, y + 1));
		}

		public void FillGaps()
		{
			bool isBlockThatCanFall = true;

			while (isBlockThatCanFall)
			{
				isBlockThatCanFall = false;

				for (int y = 0; y < COLUMN_HEIGHT - 1; y++)
				{
					for (int x = 0; x < ROW_WIDTH; x++)
					{
						if (Blocks[y][x] == null)
						{
							FillGap(x, y);
						}

						if (Blocks[y][x] != null && IsInPlayableBoard(x, y - 1) && Blocks[y - 1][x] == null)
						{
							isBlockThatCanFall = true;
						}
					}
				}
			}

			ProcessMatches();
		}

		public void ClearBlock(Block block)
		{
			Blocks[block.Position.Y][block.Position.X] = null;
			BlocksCleared++;

			AdjustScrollSpeed();
		}
		public bool PlaySound()
		{
			if(playMatchSound)
			{
				playMatchSound = false;
				return true;
			}
			return false;
		}
		private void AdjustScrollSpeed()
		{
			ScrollSpeed = 1/5f;

			if (BlocksCleared > 10)
			{
				ScrollSpeed = 2/9f;
			}
			if (BlocksCleared > 30)
			{
				ScrollSpeed = 1/4f;
			}
			if (BlocksCleared > 60)
			{
				ScrollSpeed = 2/7f;
			}
			if (BlocksCleared > 120)
			{
				ScrollSpeed = 1/3f;
			}
			if (BlocksCleared > 240)
			{
				ScrollSpeed = 2/5f;
			}
		}

		private void ProcessMatches()
		{
			// do whatever we want to do with matches -- delete blocks for now
			var matchBlocks = GetMatchBlocks();
			var blocksToClear = matchBlocks.Count();

			if (blocksToClear > 0)
			{
				uint blockScore = (uint)(blocksToClear - 2);

				Score += (blockScore * blockScore) * 100U;

				MatchedSets.Add(new MatchedSet(matchBlocks, 1, this));
				playMatchSound = true;
			}
		}

		IEnumerable<Block> GetMatchBlocks()
		{
			var matchBlocks = new List<Block>();

			for (int y = 1; y < COLUMN_HEIGHT; y++)
			{
				for (int x = 0; x < ROW_WIDTH; x++)
				{
					var thisBlock = GetBlock(x, y);

					if(thisBlock == null)
					{
						continue;
					}

					var leftBlock = GetBlock(x - 1, y);
					var rightBlock = GetBlock(x + 1, y);
					var aboveBlock = GetBlock(x, y + 1);
					var belowBlock = GetBlock(x, y - 1);

					if
					(
						leftBlock != null 
						&& rightBlock != null 
						&& thisBlock.Type == leftBlock.Type 
						&& thisBlock.Type == rightBlock.Type
						&& thisBlock.Status == Nine.Core.BlockStatus.Active
						&& rightBlock.Status == Nine.Core.BlockStatus.Active
						&& leftBlock.Status == Nine.Core.BlockStatus.Active
					)
					{
						// horizontal match
						// TODO: flag blocks as inactive/able to be deleted by typing

						// for now: flag them for deletion
						matchBlocks.Add(leftBlock);
						matchBlocks.Add(thisBlock);
						matchBlocks.Add(rightBlock);
					}

					if
					(
						aboveBlock != null
						&& belowBlock != null
						&& thisBlock.Type == aboveBlock.Type
						&& thisBlock.Type == belowBlock.Type
						&& thisBlock.Status == Nine.Core.BlockStatus.Active
						&& aboveBlock.Status == Nine.Core.BlockStatus.Active
						&& belowBlock.Status == Nine.Core.BlockStatus.Active
					)
					{
						// vertical match
						// TODO: flag blocks as inactive/able to be deleted by typing

						// for now: flag them for deletion
						matchBlocks.Add(aboveBlock);
						matchBlocks.Add(thisBlock);
						matchBlocks.Add(belowBlock);
					}
				}
			}

			return matchBlocks.Distinct();
		}
	}
}