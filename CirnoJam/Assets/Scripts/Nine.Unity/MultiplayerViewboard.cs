using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerViewboard : MonoBehaviourPunCallbacks
{
	private Nine.Core.Board gameBoard;
	private Sprite sprite;
	private Transform position;
	private List<ViewBlock> blocks;
	private PhotonView PV;
	private GamestateManager Manager;
	private System.Random rand;
	private static float Y_OFFSET = 0.93f;
	private static float X_OFFSET = 3f;
	//private int seed;


	public bool IsGameOver = false;
	public bool IsPaused = true;
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
	public uint Score => gameBoard == null ? 0 : gameBoard.Score;
	public int BlocksCleared => gameBoard == null ? 0 : gameBoard.BlocksCleared;
	public float ComboTimeMax => gameBoard == null ? 0 : gameBoard.ComboMeter.MeterTimeMax;
	public float ComboTimeRemaining => gameBoard == null ? 0 : gameBoard.ComboMeter.MeterTimeRemaining;
	public int CurrentCombo => gameBoard == null ? 0 : gameBoard.ComboMeter.Combo;


	[PunRPC]
	public void Unpause()
	{
		IsPaused = false;
	}

	[PunRPC]
	public void Initialize(int seed)
	{
		rand = new System.Random(seed);
		PV = this.GetComponent<PhotonView>();
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
		Frame = GameObject.Instantiate(Frame, new Vector3(this.transform.position.x, this.transform.position.y, -4), this.transform.rotation);
		Frame.SetSprite(Frames[boardStyle]);
		if (PV.IsMine)
		{
			Manager = FindObjectOfType<GamestateManager>();

		}
	}
	
	// Update is called once per frame
	public void Update()
	{
		if (gameBoard == null)
		{
			return;
		}
		if (IsPaused)
		{
			IsPaused = !Manager.HasGameStarted;
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
		if (gameBoard.PlaySound())
		{
			SoundPlayer.PlayTrack(MatchMadeSound);
		}
		IsGameOver = gameBoard.GameOver;
	}
	[PunRPC]
	private void Swap(int X1, int Y1 , int X2, int Y2)
	{
		(int, int) pointA = (X1, Y1);
		(int, int) pointB = (X2, Y2);
		if ((gameBoard.GetBlock(pointA) == null || gameBoard.GetBlock(pointA).CanSwap) && (gameBoard.GetBlock(pointB) == null || gameBoard.GetBlock(pointB).CanSwap))
		{
			gameBoard.Swap(pointA, pointB);
			SoundPlayer.PlayTrack(SwapSound);
		}
		else
		{
			SoundPlayer.PlayTrack(SwapErrorSound);
		}
	}

	[PunRPC]
	public void MoveCursor(int x, int y)
	{
		Cursor.SetPosition(x, y);
	}

	private void updateCursor()
	{
		if (PV.IsMine)
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow) && Cursor.GridPosition.X > 0)
			{
				//PV.RPC("MoveCursor", RpcTarget.All, -1, 0);
				Cursor.Move(-1, 0);
				PV.RPC("MoveCursor", RpcTarget.Others, Cursor.GridPosition.X, Cursor.GridPosition.Y);
			}
			if (Input.GetKeyDown(KeyCode.RightArrow) && Cursor.GridPosition.X < Nine.Core.Board.ROW_WIDTH - 2)
			{
				//PV.RPC("MoveCursor", RpcTarget.All, 1, 0);
				Cursor.Move(1, 0);
				PV.RPC("MoveCursor", RpcTarget.Others, Cursor.GridPosition.X, Cursor.GridPosition.Y);
			}
			if (Input.GetKeyDown(KeyCode.DownArrow) && Cursor.GridPosition.Y > 1)
			{
				//PV.RPC("MoveCursor", RpcTarget.All, 0, -1);
				Cursor.Move(0, -1);
				PV.RPC("MoveCursor", RpcTarget.Others, Cursor.GridPosition.X, Cursor.GridPosition.Y);
			}
			if (Input.GetKeyDown(KeyCode.UpArrow) && Cursor.GridPosition.Y < gameBoard.StackHeight)
			{
				Cursor.Move(0, 1);
				PV.RPC("MoveCursor", RpcTarget.Others, Cursor.GridPosition.X, Cursor.GridPosition.Y);
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				PV.RPC("MoveCursor", RpcTarget.Others, Cursor.GridPosition.X, Cursor.GridPosition.Y);
				PV.RPC("Swap", RpcTarget.All, Cursor.GridPosition.X, Cursor.GridPosition.Y, Cursor.GridPosition.X + 1, Cursor.GridPosition.Y);
				UpdateBlocks();
			}
		}

		Cursor.transform.position = this.transform.position + new Vector3(Cursor.GridPosition.X + X_OFFSET, Cursor.GridPosition.Y + Y_OFFSET + (gameBoard.ScrollProgress % 1), -2);
	}
	void UpdateBlocks()
	{
		int width = Nine.Core.Board.ROW_WIDTH;
		int height = Nine.Core.Board.COLUMN_HEIGHT;
		for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					if (gameBoard.Blocks[y][x] != null)
					{
						PV.RPC("UpdateBlock", RpcTarget.Others,y, x, gameBoard.Blocks[y][x].GetTypeAsInt());
					}
					else
					{
						PV.RPC("UpdateBlock", RpcTarget.Others, y, x, -1);
					}
				}
			}
	}
	[PunRPC]
	public void UpdateBlock(int y, int x, int type)
	{
		if(type == -1)
		{
			this.gameBoard.Blocks[y][x] = null;
		}
		else
		{
			this.gameBoard.Blocks[y][x].SetTypeAsInt(type);
		}
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