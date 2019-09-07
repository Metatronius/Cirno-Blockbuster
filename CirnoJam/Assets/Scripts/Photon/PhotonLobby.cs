using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
	public static PhotonLobby Lobby;

	public GameObject searchButton;

	public GameObject cancelButton;

	private void Awake()
	{
		Lobby = this;
	}

	// Start is called before the first frame update
	void Start()
    {
		PhotonNetwork.ConnectUsingSettings(); //connects to photon server
    }

	public override void OnConnectedToMaster()
	{
		Debug.Log("Player has connected to Photon server");
		searchButton.SetActive(true);
	}

	public void OnSearchButtonClick()
	{
		PhotonNetwork.JoinRandomRoom();
		searchButton.SetActive(false);
		cancelButton.SetActive(true);
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("Joining Room Failed. No Games Available");
		CreateRoom();
	}

	void CreateRoom()
	{
		int randomRoomName = Random.Range(0, 10000);
		Debug.Log("Trying to create a new room " + randomRoomName);
		RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
		PhotonNetwork.CreateRoom("Room " + randomRoomName, roomOps);
	}
	public void Disconnect()
	{
		PhotonNetwork.Disconnect();
	}
	public override void OnJoinedRoom()
	{
		base.OnJoinedRoom();
		Debug.Log("Joined Room");
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		Debug.Log("Tried to create a new room but failed, there must be a room with the same name");
		CreateRoom();
	}

	public void OnCancelButtonClicked()
	{
		cancelButton.SetActive(false);
		searchButton.SetActive(true);
		PhotonNetwork.LeaveRoom();
	}
	// Update is called once per frame
	void Update()
    {
        
    }
}
