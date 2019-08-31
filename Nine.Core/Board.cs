using System;
using System.Linq;

namespace Nine.Core
{
	public class Board
	{
		public const int ROW_WIDTH = 6;
		public const int COLUMN_HEIGHT = 12;

		// Blocks are accessed by Blocks[y][x]
		public Block[][] Blocks { get; set; }
		public bool GameOver
		{
			get
			{
				return Blocks[COLUMN_HEIGHT - 1].Any(block => block != null);
			}
		}

		public void ShiftBlocksUp()
		{
			// shift UP
			for (int row = 0; row < Blocks.Length; row++)
			{
				// replace bottom row with some fresh blocks
				if(row == 0)
				{
					Blocks[row] = GetNewRow();
				}
				else
				{
					Blocks[row] = Blocks[row - 1]; // TODO: ShiftRowUp(row) ?
				}
			}
		}

		public Block[] GetNewRow()
		{
			var row = new Block[ROW_WIDTH];

			for (int i = 0; i < row.Length; i++)
			{
				// TODO: move random instance, no way this works well
				var rand = new Random();
				BlockType blockType = (BlockType)rand.Next(Enum.GetNames(typeof(BlockType)).Length);

				row[i] = new Block(blockType);
			}

			return row;
		}

		public void Swap((int X, int Y) pointA, (int X, int Y) pointB)
		{
			var blockA = Blocks[pointA.Y][pointA.X];
			var blockB = Blocks[pointB.Y][pointB.X];

			// TODO: account for animation/state transition time
			// maybe tell the blocks that they are in the swapping state, and where to?

			Blocks[pointA.Y][pointA.X] = blockB;
			Blocks[pointB.Y][pointB.X] = blockA;
		}
	}
}