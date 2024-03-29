﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class ViewBoard : MonoBehaviour
{
	private Nine.Core.Board gameBoard;
	private Sprite sprite;
	private Transform position;
	private List<ViewBlock> blocks;
	private static float Y_OFFSET = 0.93f;
	private static float X_OFFSET = 3f;
	//private int seed;


	public bool IsGameOver = false;
	public bool IsPaused = false;
	public ViewBlockFactory ViewBlockFactory;
	public float ScrollSpeed;
	public Cursor Cursor;

	public SFXPlayer SoundPlayer;
	public AudioClip SwapSound;
	public AudioClip SwapErrorSound;
	public AudioClip MatchMadeSound;
	public AudioClip MatchClearSound;
	public AudioClip PauseSound;
	public List<Sprite> Backgrounds;
	public List<Sprite> Frames;
	public SpriteSelector Frame;

	public int StackHeight => gameBoard == null ? 0 : gameBoard.StackHeight;
	public uint Score => gameBoard == null ? 0: gameBoard.Score;
	public int BlocksCleared => gameBoard == null ? 0 : gameBoard.BlocksCleared;
	public float ComboTimeMax => gameBoard == null ? 0 : gameBoard.ComboMeter.MeterTimeMax;
	public float ComboTimeRemaining => gameBoard == null ? 0 : gameBoard.ComboMeter.MeterTimeRemaining;
	public int CurrentCombo => gameBoard == null ? 0 : gameBoard.ComboMeter.Combo;

	//public void Seed(int s)
	//{
	//	seed = s;
	//}
	//public void Seed()
	//{
	//	seed = new System.Random().Next();
	//}

	public void Initialize(int seed)
	{
		var rand = new System.Random(seed);
		
		gameBoard = new Nine.Core.Board(ScrollSpeed, seed);
		Frame.transform.position = this.transform.position - new Vector3(0, 0, 4);
		position = this.GetComponent<Transform>();
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
		SoundPlayer = GameObject.Instantiate(SoundPlayer);

	}
	// Start is called before the first frame update
	public void Start()
	{
		int boardStyle = new System.Random().Next(Backgrounds.Count);
		this.gameObject.GetComponent<SpriteRenderer>().sprite = Backgrounds[boardStyle];
		Frame = GameObject.Instantiate(Frame, new Vector3 (this.transform.position.x, this.transform.position.y, -4), this.transform.rotation);
		Frame.SetSprite(Frames[boardStyle]);
	}
	public void ToggleBackground()
	{
		if(IsPaused)
		{
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -4);
			SoundPlayer.PlayTrack(PauseSound);
		}
		else
		{
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
		}
	}

	// Update is called once per frame
	public void Update()
	{
		if(gameBoard == null)
		{
			return;
		}
		if(IsPaused)
		{
			return;
		}
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
		if(gameBoard.PlaySound())
		{
			SoundPlayer.PlayTrack(MatchMadeSound);
		}
		IsGameOver = gameBoard.GameOver;
	}

	private void Swap((int X, int Y) pointA, (int X, int Y) pointB)
	{
		if((gameBoard.GetBlock(pointA) == null || gameBoard.GetBlock(pointA).CanSwap) && (gameBoard.GetBlock(pointB) == null || gameBoard.GetBlock(pointB).CanSwap))
		{
			gameBoard.Swap(pointA, pointB);
			SoundPlayer.PlayTrack(SwapSound);
		}
		else
		{
			SoundPlayer.PlayTrack(SwapErrorSound);
		}
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
			this.Swap(Cursor.GridPosition, (Cursor.GridPosition.X+1, Cursor.GridPosition.Y) );
		}

		Cursor.transform.position = this.transform.position + new Vector3(Cursor.GridPosition.X + X_OFFSET, Cursor.GridPosition.Y + Y_OFFSET + (gameBoard.ScrollProgress % 1), -2);
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
			block.transform.position = this.transform.position + new Vector3(block.Block.Position.X + X_OFFSET, block.Block.Position.Y + Y_OFFSET + (gameBoard.ScrollProgress % 1), -1);
		}
	}
}