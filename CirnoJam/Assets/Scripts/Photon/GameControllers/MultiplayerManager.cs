using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;
using System.IO;

public class MultiplayerManager : MonoBehaviour
{
	public MusicManager MusicManager;
	public ViewBoard ViewBoard;
	public SpriteSelector Mascot;
	public Mascot Mascots;
	public Button RestartButton;
	public Button MainMenuButton;
	public Button QuitButton;
	public RawImage GameoverBackground;
	public GameObject myBoard;


	private PhotonView PV;
	private bool gameover = false;
	private bool active = false;
	private bool intense = false;
	private bool isGameOver = false;
	private int playerNumber;
	private uint lastScore = 0;


	// Start is called before the first frame update
	void Start()
	{
		PV = GetComponent<PhotonView>();
		MusicManager = GameObject.Instantiate(MusicManager);
		MusicManager.PlayTrack(MusicManager.MainThemeIntro, MusicManager.MainThemeLoop);

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
			myBoard = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MultiplayerBoard"), GameSetup.GS.spawnPoints[playerNumber-1].position, GameSetup.GS.spawnPoints[playerNumber-1].rotation, 0);
		}

		ViewBoard = GameObject.Instantiate(ViewBoard, this.transform.position, this.transform.rotation);

		Mascot.SetSprite(Mascots.Normal);

	}
	// Update is called once per frame
	void Update()
	{
		if (ViewBoard.IsGameOver && !gameover)
		{
			ToggleButtons();
			GameoverBackground.gameObject.SetActive(true);
			gameover = true;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ToggleButtons();
		}
		if (ViewBoard.Score != lastScore)
		{
			lastScore = ViewBoard.Score;
			Mascot.SetSprite(Mascots.Cheer, 1f);
		}
		if (ViewBoard.StackHeight >= Nine.Core.Board.COLUMN_HEIGHT - 4 && !intense)
		{
			MusicManager.PlayTrack(MusicManager.IntenseThemeIntro, MusicManager.IntenseThemeLoop);
			Mascot.TransitionSprites(Mascots.Surprised, 2, Mascots.Angry);
			intense = true;
		}
		else if (ViewBoard.StackHeight < Nine.Core.Board.COLUMN_HEIGHT - 6 && intense)
		{
			MusicManager.PlayTrack(MusicManager.MainThemeIntro, MusicManager.MainThemeLoop);
			Mascot.TransitionSprites(Mascots.Determined, 2, Mascots.Normal);
			intense = false;
		}
		if (ViewBoard.IsGameOver && !isGameOver)
		{

			MusicManager.PlayTrack(MusicManager.Loss, MusicManager.PostGameLoop);
			isGameOver = true;
		}
	}
	public void ToggleButtons()
	{
		active = !active;
		MainMenuButton.gameObject.SetActive(active);
		QuitButton.gameObject.SetActive(active);

	}
}
