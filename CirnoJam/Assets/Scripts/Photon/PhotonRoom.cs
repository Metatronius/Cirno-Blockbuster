using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
	public static PhotonRoom room;
	private PhotonView PV;

	public bool isGameLoaded;
	public int currentScene;
	public int MultiplayerScene;
	private Player[] photonPlayers;

	public int playersInRoom;
	public int myNumberInRoom;

	public int playerInGame;

	private bool readyToCount;
	private bool readyToStart;
	public float startingTime;
	private float lessThanMaxPlayers;
	private float atMaxPlayers;
	private float timeToStart;
	private void Awake()
	{
		if (PhotonRoom.room == null)
		{
			PhotonRoom.room = this;
		}
		else
		{
			if (PhotonRoom.room != this)
			{
				Destroy(PhotonRoom.room.gameObject);
				PhotonRoom.room = this;
			}
		}
		DontDestroyOnLoad(this.gameObject);
	}

	public override void OnEnable()
	{
		base.OnEnable();
		PhotonNetwork.AddCallbackTarget(this);
		SceneManager.sceneLoaded += OnSceneFinishedLoading;
	}

	public override void OnDisable()
	{
		base.OnDisable();
		PhotonNetwork.RemoveCallbackTarget(this);
		SceneManager.sceneLoaded -= OnSceneFinishedLoading;
	}
	void Start()
	{
		PV = GetComponent<PhotonView>();
		readyToCount = false;
		readyToStart = false;
		lessThanMaxPlayers = startingTime;
		atMaxPlayers = 6;
		timeToStart = startingTime;
	}
	// Update is called once per frame
	void Update()
	{
		if (playersInRoom == 1)
		{
			RestartTimer();
		}
		if (!isGameLoaded)
		{
			if (readyToStart)
			{
				atMaxPlayers -= Time.deltaTime;
				lessThanMaxPlayers = atMaxPlayers;
				timeToStart = atMaxPlayers;
			}
			else if (readyToCount)
			{
				lessThanMaxPlayers -= Time.deltaTime;
				timeToStart = lessThanMaxPlayers;
			}
			if (timeToStart <= 0)
			{
				StartGame();
			}
		}
	}

	public override void OnJoinedRoom()
	{
		base.OnJoinedRoom();
		Debug.Log("Joined Room");
		photonPlayers = PhotonNetwork.PlayerList;
		playersInRoom = photonPlayers.Length;
		myNumberInRoom = playersInRoom;
		PhotonNetwork.NickName = myNumberInRoom.ToString();
		Debug.Log("Players in room (" + playersInRoom + "/" + MultiplayerSettings.multiplayerSettings.maxPlayers + ")");
		if (playersInRoom > 1)
		{
			readyToCount = true;
		}
		if (playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers)
		{
			readyToStart = true;
			if (!PhotonNetwork.IsMasterClient)
			{
				return;
			}
			PhotonNetwork.CurrentRoom.IsOpen = false;
		}

	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		base.OnPlayerEnteredRoom(newPlayer);
		Debug.Log("A new player has joined the room");
		photonPlayers = PhotonNetwork.PlayerList;
		playersInRoom++;
		if (playersInRoom > 1)
		{
			readyToCount = true;
		}
		if (playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers)
		{
			readyToStart = true;
			if (PhotonNetwork.IsMasterClient)
			{
				return;
			}
			PhotonNetwork.CurrentRoom.IsOpen = false;
		}
	}
	void StartGame()
	{
		isGameLoaded = true;
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		PhotonNetwork.CurrentRoom.IsOpen = false;
		PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.multiplayerScene);
	}

	//private void CreatePlayer()
	//{
	//	PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
	//}
	void RestartTimer()
	{
		lessThanMaxPlayers = startingTime;
		timeToStart = startingTime;
		atMaxPlayers = 6;
		readyToCount = false;
		readyToStart = false;
	}
	void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		currentScene = scene.buildIndex;
		if (currentScene == MultiplayerSettings.multiplayerSettings.multiplayerScene)
		{
			isGameLoaded = true;

			PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
		}
	}
	[PunRPC]
	private void RPC_LoadedGameScene()
	{
		playerInGame++;
		if(playerInGame == PhotonNetwork.PlayerList.Length)
		{
			PV.RPC("RPC_CreatePlayer", RpcTarget.All);
		}
		Debug.Log("Loaded Scene");
	}
	[PunRPC]
	private void RPC_CreatePlayer()
	{
		PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
		Debug.Log("Created Player");
	}
}


