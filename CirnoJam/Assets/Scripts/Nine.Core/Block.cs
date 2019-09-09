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

	public enum BlockStatus
	{
		Active,
		Swapping,
		Inactive,
		Clearing,
	}

	public class Block
	{
		public BlockType Type { get; private set; }
		public (int X, int Y) Position { get; set; }
		public BlockStatus Status { get; set; }
		public bool CanSwap => this.Status == BlockStatus.Active;

		public Block(int x, int y, BlockType type, Board board)
		{
			this.Type = type;
			this.Position = (x, y);
			this.Status = BlockStatus.Inactive;
		}
		public int GetTypeAsInt()
		{
			return (int)Type;
		}
		public void SetTypeAsInt(int type)
		{
			this.Type = (BlockType)type;
		}
	}
}
