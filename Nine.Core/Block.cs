using System;

namespace Nine.Core
{
	public enum BlockType
	{
		Red,
		Green,
		Blue,
		Purple,
		Yellow,
		Garbage,
	}

	public class Block
	{
		public BlockType Type { get; }

		public Block(BlockType blockType)
		{
			this.Type = blockType;
		}
	}
}