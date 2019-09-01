using System;
using System.Linq;

namespace Nine.Core
{
    public class Board : IUpdatable
    {
        public const int ROW_WIDTH = 6;
        public const int COLUMN_HEIGHT = 12;
        public readonly float ScrollTime;
        public float ScrollProgress;


        // Blocks are accessed by Blocks[y][x]
		public Block[][] Blocks { get; set; }
		public bool GameOver
		{
			get
			{
				return Blocks[COLUMN_HEIGHT - 1].Any(block => block != null);
			}
		}

        public Board(float scrollTime)
        {
            this.ScrollTime = scrollTime;
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
            }
        }

		public void ShiftBlocksUp()
		{
            ScrollProgress -= ScrollTime;

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
                        }
                    }
				}
			}
		}

		public Block[] GetNewRow(int y)
		{
			var row = new Block[ROW_WIDTH];

			for (int i = 0; i < row.Length; i++)
			{
				row[i] = new Block(i, y);
			}

			return row;
		}

		public void Swap((int X, int Y) pointA, (int X, int Y) pointB)
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

		public void Update(float deltaTime)
		{
            ScrollProgress += deltaTime;
		}
	}
}