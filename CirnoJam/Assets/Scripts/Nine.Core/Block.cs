using System;

namespace Nine.Core
{
	public enum BlockType //garbage must be last to continue working as intended
	{
		Green,
		Blue,
		Pink,
		White,
		Black,
		Garbage,
	}

	public class Block
	{
		public BlockType Type { get; }
		public (int X, int Y) Position { get; set; }
		public bool CanSwap { get; private set; }
		public bool IsClearing { get; private set; }

		public Block(int x, int y, BlockType type)
		{
			this.Type = type;
			this.Position = (x, y);
			this.CanSwap = false;
			this.IsClearing = false;
		}

		public void SetSwappable(bool canSwap)
		{
			this.CanSwap = canSwap;
		}
	}
}