using System;
using UnityEngine;

public class ViewBlockFactory : MonoBehaviour
{
	public ViewBlock BaseBlock;
	public Sprite PinkBlockSprite;
	public Sprite GreenBlockSprite;
	public Sprite BlueBlockSprite;
	public Sprite BlackBlockSprite;
	public Sprite WhiteBlockSprite;
	public Sprite GarbageBlockSprite;
	public Sprite InactivePinkBlockSprite;
	public Sprite InactiveGreenBlockSprite;
	public Sprite InactiveBlueBlockSprite;
	public Sprite InactiveBlackBlockSprite;
	public Sprite InactiveWhiteBlockSprite;
	public Sprite InactiveGarbageBlockSprite;

	private Sprite GetBlockSprite(Nine.Core.Block block)
	{
		switch(block.Type)
		{
			case Nine.Core.BlockType.Pink:
				return block.CanSwap ? PinkBlockSprite : InactivePinkBlockSprite;
			case Nine.Core.BlockType.Green:
				return block.CanSwap ? GreenBlockSprite : InactiveGreenBlockSprite;
			case Nine.Core.BlockType.Blue:
				return block.CanSwap ? BlueBlockSprite : InactiveBlueBlockSprite;
			case Nine.Core.BlockType.Black:
				return block.CanSwap ? BlackBlockSprite : InactiveBlackBlockSprite;
			case Nine.Core.BlockType.White:
				return block.CanSwap ? WhiteBlockSprite : InactiveWhiteBlockSprite;
			case Nine.Core.BlockType.Garbage:
				return GarbageBlockSprite;
			default:
				throw new Exception($"no sprite hooked up for specified block type: {block.Type.ToString()}");
		}
	}

	public ViewBlock CreateBlock(Nine.Core.Block block)
	{
		var blockSprite = GetBlockSprite(block);

		ViewBlock viewBlock = MonoBehaviour.Instantiate<ViewBlock>(BaseBlock);

		viewBlock.Block = block;
		viewBlock.gameObject.GetComponent<SpriteRenderer>().sprite = blockSprite;

		return viewBlock;
	}
}