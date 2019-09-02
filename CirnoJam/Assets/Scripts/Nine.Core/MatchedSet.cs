using System.Collections;
using System.Collections.Generic;

namespace Nine.Core
{
	public class MatchedSet : IUpdatable
	{
		private readonly Board board;
		private readonly IEnumerable<Block> blocks;
		private float timeToClear;

		public bool IsCleared { get; private set; }

		public MatchedSet(IEnumerable<Block> blocks, float timeToClear, Board board)
		{
			this.board = board;
			this.blocks = blocks;
			this.timeToClear = timeToClear;
			this.IsCleared = false;

			foreach (var block in blocks)
			{
				block.Status = Nine.Core.BlockStatus.Clearing;
			}
		}

		public void Update(float deltaTime)
		{
			this.timeToClear -= deltaTime;

			if (this.timeToClear <= 0)
			{
				foreach (var block in blocks)
				{
					board.ClearBlock(block);
				}

				this.IsCleared = true;
			}
		}
	}
}