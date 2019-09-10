using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
	private Queue<AudioSource> sources;
	private float volume;


	// Start is called before the first frame update
	void Awake
		()
	{
		volume = PlayerPrefs.GetFloat("SFXVolume");
		var audioSources = GetComponents<AudioSource>();

		sources = new Queue<AudioSource>();

		sources.Enqueue(audioSources[0]);
		sources.Enqueue(audioSources[1]);
	}

	public void SetVolume(float v)
	{
		volume = v;
		var sources = GetComponents<AudioSource>();
		foreach (AudioSource s in sources)
		{
			s.volume = volume;
		}
	}

	// Update is called once per frame
	void Update()
	{
	}

	public void PlayTrack(AudioClip track)
	{
		var activeSource = sources.Dequeue();
		sources.Enqueue(activeSource);

		activeSource.clip = track;
		activeSource.loop = false;
		activeSource.volume = volume;
		activeSource.Play();
	}
}
