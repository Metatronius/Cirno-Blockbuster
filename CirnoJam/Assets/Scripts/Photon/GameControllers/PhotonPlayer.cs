using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
	private PhotonView PV;

	public GameObject myAvatar;
    // Start is called before the first frame update
    void Start()
    {
		PV = GetComponent<PhotonView>();
		int spawnPicker = 0;
		//int spawnPicker = Random.Range(0, GameSetup.GS.spawnPoints.Length);
		if (PhotonNetwork.NickName.Equals("2"))
		{
			 spawnPicker = 1;
		}
		if(PV.IsMine)
		{
			myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation, 0);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
