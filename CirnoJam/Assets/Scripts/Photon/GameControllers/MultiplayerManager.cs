using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
	public MultiplayerViewboard MultiplayerViewboard;
	public SpriteSelector Mascot;
	public Mascot Mascots;
	public Button RestartButton;
	public Button MainMenuButton;
	public Button QuitButton;
	public RawImage GameoverBackground;
	public GameObject myBoard;

	private GamestateManager Manager;
	private int connectedClients = 0;
	private PhotonView PV;
	private bool gameover = false;
	private bool active = false;
	private bool intense = false;
	private int playerNumber;
	private uint lastScore = 0;



	// Start is called before the first frame update
	void Start()
	{
		PV = GetComponent<PhotonView>();
		

		if (PhotonNetwork.NickName.Equals("1"))
		{
			playerNumber = 1;
		}
		else if (PhotonNetwork.NickName.Equals("2"))
		{
			playerNumber = 2;
		}
		if (PV.IsMine)
		{
			myBoard = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MultiplayerBoard"), GameSetup.GS.spawnPoints[playerNumber - 1].position, GameSetup.GS.spawnPoints[playerNumber - 1].rotation, 0);
			MultiplayerViewboard = myBoard.GetComponent<MultiplayerViewboard>();

			myBoard.GetComponent<PhotonView>().RPC("Initialize", RpcTarget.All, (PhotonNetwork.CurrentRoom.GetHashCode()));
			Mascot.SetSprite(Mascots.Normal);
			Manager = FindObjectOfType<GamestateManager>();
			Manager.GetComponent<PhotonView>().RPC("Register", RpcTarget.All);
		}
		

		//MultiplayerViewboard = GameObject.Instantiate(MultiplayerViewboard, this.transform.position, this.transform.rotation);
		
	}

	//[PunRPC]
	//public void Connect()
	//{
	//	connectedClients++;
	//	if(connectedClients >= 2)
	//	{
	//		myBoard.GetComponent<PhotonView>().RPC("Unpause", RpcTarget.All);
	//	}
	//}

	// Update is called once per frame
	void Update()
	{
		if(MultiplayerViewboard == null)
		{
			return;
		}
		if (MultiplayerViewboard.IsGameOver && !Manager.IsGameOver)
		{
			ToggleButtons();
			GameoverBackground.gameObject.SetActive(true);
			gameover = true;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ToggleButtons();
		}
		if (MultiplayerViewboard.Score != lastScore)
		{
			lastScore = MultiplayerViewboard.Score;
			Mascot.SetSprite(Mascots.Cheer, 1f);
		}
		if (MultiplayerViewboard.StackHeight >= Nine.Core.Board.COLUMN_HEIGHT - 4 && !Manager.intense)
		{
			Manager.MusicManager.PlayTrack(Manager.MusicManager.IntenseThemeIntro, Manager.MusicManager.IntenseThemeLoop);
			Mascot.TransitionSprites(Mascots.Surprised, 2, Mascots.Angry);
			Manager.intense = true;
		}
		else if (MultiplayerViewboard.StackHeight < Nine.Core.Board.COLUMN_HEIGHT - 6 && Manager.intense)
		{
			Manager.MusicManager.PlayTrack(Manager.MusicManager.MainThemeIntro, Manager.MusicManager.MainThemeLoop);
			Mascot.TransitionSprites(Mascots.Determined, 2, Mascots.Normal);
			Manager.intense = false;
		}
		if (MultiplayerViewboard.IsGameOver && !Manager.IsGameOver)
		{

			Manager.MusicManager.PlayTrack(Manager.MusicManager.Loss, Manager.MusicManager.PostGameLoop);
			Manager.IsGameOver = true;
		}
	}
	public void ToggleButtons()
	{
		active = !active;
		MainMenuButton.gameObject.SetActive(active);
		QuitButton.gameObject.SetActive(active);

	}
	
}
