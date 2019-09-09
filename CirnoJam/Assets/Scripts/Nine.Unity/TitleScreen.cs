using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
	private MusicManager MusicManager;
	public AudioClip TitleBGM;
    // Start is called before the first frame update
    void Start()
    {
		MusicManager = GetComponent<MusicManager>();
		MusicManager.PlayTrack(TitleBGM, TitleBGM);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
