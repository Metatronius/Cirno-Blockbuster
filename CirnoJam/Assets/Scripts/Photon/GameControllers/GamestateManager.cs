using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamestateManager : MonoBehaviourPunCallbacks, IPunObservable
{
	public bool IsGameOver = false;
	public bool HasGameStarted = false;
	public MusicManager MusicManager;
	public bool intense = false;
	public int Seed;

	private PhotonView PV;
	private int playerCount = 0;
	// Start is called before the first frame update

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{

		if (stream.IsWriting)
		{
			stream.SendNext(HasGameStarted);
		}
		else
		{
			HasGameStarted = (bool)stream.ReceiveNext();
		}
	}

	void Start()
    {
		PV = GetComponent<PhotonView>();
		MusicManager = GameObject.Instantiate(MusicManager);
		MusicManager.PlayTrack(MusicManager.MainThemeIntro, MusicManager.MainThemeLoop);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
	public void StartGame()
	{
		HasGameStarted = true;
		var players = FindObjectsOfType<MultiplayerViewboard>();
		Debug.Log("Game has started letsgogogogogogo");

	}
	[PunRPC]
	public void Register()
	{
		playerCount++;
		if(playerCount >= 2)
		{
			StartGame();
		}
	}
}
