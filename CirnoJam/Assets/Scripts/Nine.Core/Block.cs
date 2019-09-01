using System;

namespace Nine.Core
{
	public enum BlockType
	{
		Green,
		Blue,
		Pink,
		White,
        Black,
		Garbage,
	}


    //public struct Neighbors
    //{
    //    Block left;
    //    Block right;
    //    Block up;
    //    Block down;
    //}

    public class Block
    {
        private static readonly Random rand = new Random();

        public BlockType Type { get; }
        public (int X, int Y) Position { get; set; }
        //public Neighbors Neighbors { get; set; }
        
        public Block(int x, int y)
        {
            this.Type = (BlockType)rand.Next(Enum.GetNames(typeof(BlockType)).Length);
            this.Position = (x, y);
        }
        
        // public Block(BlockType blockType, Block left, Block right, Block up, Block down)
        // {
        //     this.Type = blockType;

        //     this.left = left;
        //     this.right = right;
        //     this.up = up;
        //     this.down = down;
        // }

        //public uint check(uint i, char dir)
        //{
        //    switch (dir)
        //    {
        //        case 'l':
        //            if (this.neighbours.left && this.neighbours.left.Type == this.Type)
        //            {
        //                return check(++i, 'l');
        //            }
        //            else
        //            {
        //                return i;
        //            }
        //        case 'r':
        //            if(this.neightbours.right && this.neighbours.right.Type == this.Type)
        //            {
        //                return check(++i, 'r');
        //            }
        //            else
        //            {
        //                return i;
        //            }
        //        case 'u':
        //            if(this.neighbours.up && this.neighbours.up.Type == this.neighbours.left.Type)
        //            {
        //                return check(++i, 'u');
        //            }
        //            else
        //            {
        //                return i;
        //            }
        //        case 'd':
        //            if(this.neighbours.down && this.neighbours.down.Type == this.neighbours.left.Type)
        //            {
        //                return check(++i, 'd');
        //            }
        //            else
        //            {
        //                return i;
        //            }
        //    }
        //}
        
        
	}
}