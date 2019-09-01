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

	private Sprite GetBlockSprite(Nine.Core.BlockType blockType)
	{
		switch(blockType)
		{
			case Nine.Core.BlockType.Pink:
				return PinkBlockSprite;
			case Nine.Core.BlockType.Green:
				return GreenBlockSprite;
			case Nine.Core.BlockType.Blue:
				return BlueBlockSprite;
			case Nine.Core.BlockType.Black:
				return BlackBlockSprite;
			case Nine.Core.BlockType.White:
				return WhiteBlockSprite;
			case Nine.Core.BlockType.Garbage:
				return GarbageBlockSprite;
			default:
				throw new Exception($"no sprite hooked up for specified block type: {blockType.ToString()}");
		}
	}

	public ViewBlock CreateBlock(Nine.Core.Block block, Vector3 boardPosition, Quaternion rotation)
	{
		var blockSprite = GetBlockSprite(block.Type);
		ViewBlock viewBlock = MonoBehaviour.Instantiate<ViewBlock>(BaseBlock, boardPosition, rotation);

		viewBlock.Block = block;

		viewBlock.gameObject.GetComponent<SpriteRenderer>().sprite = blockSprite;

		return viewBlock;
	}
}