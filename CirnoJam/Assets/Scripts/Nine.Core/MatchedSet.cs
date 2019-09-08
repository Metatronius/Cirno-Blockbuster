using System.Collections;
using System.Collections.Generic;

namespace Nine.Core
{
	public class MatchedSet : IUpdatable
	{
		private readonly Board board;
		private float timeToClear;

		public IEnumerable<Block> Blocks { get; private set; }
		public bool IsCleared { get; private set; }

		public MatchedSet(IEnumerable<Block> blocks, float timeToClear, Board board)
		{
			this.board = board;
			this.Blocks = blocks;
			this.timeToClear = timeToClear;
			this.IsCleared = false;

			foreach (var block in this.Blocks)
			{
				block.Status = Nine.Core.BlockStatus.Clearing;
			}
		}

		public void Update(float deltaTime)
		{
			this.timeToClear -= deltaTime;

			if (this.timeToClear <= 0)
			{
				board.ClearMatch(this);
				this.IsCleared = true;
			}
		}
	}
}