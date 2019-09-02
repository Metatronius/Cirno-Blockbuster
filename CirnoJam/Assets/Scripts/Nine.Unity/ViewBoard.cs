using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBoard : MonoBehaviour
{
	private Nine.Core.Board gameBoard;
	private Sprite sprite;
	private Transform position;
	private List<ViewBlock> blocks;

	public bool IsGameOver = false;
	public ViewBlockFactory ViewBlockFactory;
	public float ScrollSpeed;
	public int PlayerIndex;
	public Cursor Cursor;

	public int StackHeight => gameBoard.StackHeight;
	public uint Score => gameBoard.Score;
	public int BlocksCleared => gameBoard.BlocksCleared;

	// Start is called before the first frame update
	public void Start()
	{
		sprite = this.GetComponent<Sprite>();
		position = this.GetComponent<Transform>();
		gameBoard = new Nine.Core.Board(ScrollSpeed);
		blocks = new List<ViewBlock>();

		for (int x = 0; x < Nine.Core.Board.ROW_WIDTH; x++)
		{
			for (int y = 0; y < Nine.Core.Board.COLUMN_HEIGHT; y++)
			{
				if (gameBoard.Blocks[y][x] != null)
				{
					blocks.Add
					(
						ViewBlockFactory.CreateBlock(gameBoard.Blocks[y][x])
					);
				}
			}
		}

		Cursor = GameObject.Instantiate(Cursor);
	}

	// Update is called once per frame
	public void Update()
	{
		if (IsGameOver)
		{
			return;
		}

		updateCursor();
		gameBoard.Update(Time.deltaTime);

		while (gameBoard.ScrollProgress >= 1)
		{
			gameBoard.ShiftBlocksUp();
			for (int x = 0; x < Nine.Core.Board.ROW_WIDTH; x++)
			{
				blocks.Add
				(
					ViewBlockFactory.CreateBlock(gameBoard.Blocks[0][x]) // , this.transform.position + new Vector3(x + 1, 1, 1), this.transform.rotation
				);
			}

			Cursor.Move(0, 1);
		}

		sync();

		IsGameOver = gameBoard.GameOver;
	}

	private void updateCursor()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow) && Cursor.GridPosition.X > 0)
		{
			Cursor.Move(-1, 0);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow) && Cursor.GridPosition.X < Nine.Core.Board.ROW_WIDTH - 2)
		{
			Cursor.Move(1, 0);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow) && Cursor.GridPosition.Y > 1)
		{
			Cursor.Move(0, -1);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow) && Cursor.GridPosition.Y < gameBoard.StackHeight)
		{
			Cursor.Move(0, 1);
		}
		if(Input.GetKeyDown(KeyCode.Space))
		{
			gameBoard.Swap(Cursor.GridPosition, (Cursor.GridPosition.X+1, Cursor.GridPosition.Y) );
		}

		Cursor.transform.position = this.transform.position + new Vector3(Cursor.GridPosition.X + 1, Cursor.GridPosition.Y + 1 + (gameBoard.ScrollProgress % 1), 1);
	}

	void sync()
	{
		foreach (var block in blocks)
		{
			GameObject.Destroy(block.gameObject);
		}

		blocks = new List<ViewBlock>();

		for (int x = 0; x < Nine.Core.Board.ROW_WIDTH; x++)
		{
			for (int y = 0; y < Nine.Core.Board.COLUMN_HEIGHT; y++)
			{
				if (gameBoard.Blocks[y][x] != null)
				{
					blocks.Add
					(
						ViewBlockFactory.CreateBlock(gameBoard.Blocks[y][x])
					);
				}
			}
		}

		foreach (var block in blocks)
		{
			block.transform.position = this.transform.position + new Vector3(block.Block.Position.X + 1, block.Block.Position.Y + 1 + (gameBoard.ScrollProgress % 1), 1);
		}
	}
}