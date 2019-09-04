using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
	private Queue<AudioSource> sources;
	private bool sourceFlag = false;

	// Start is called before the first frame update
	void Start()
	{


	}

	// Update is called once per frame
	void Update()
	{
	}

	public void PlayTrack(AudioClip track)
	{
		var audioSources = GetComponents<AudioSource>();

		sources = new Queue<AudioSource>();

		if (sourceFlag)
		{
			sources.Enqueue(audioSources[0]);
			sources.Enqueue(audioSources[1]);
		}
		else
		{
			sources.Enqueue(audioSources[1]);
			sources.Enqueue(audioSources[0]);
		}
		sourceFlag = !sourceFlag;

		var activeSource = sources.Dequeue();
		sources.Enqueue(activeSource);

		activeSource.clip = track;
		activeSource.loop = false;
		activeSource.Play();
	}
}
