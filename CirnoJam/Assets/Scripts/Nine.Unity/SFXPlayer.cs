﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
	private Queue<AudioSource> sources;

	// Start is called before the first frame update
	void Start()
	{
		var audioSources = GetComponents<AudioSource>();

		sources = new Queue<AudioSource>();
		sources.Enqueue(audioSources[0]);
		sources.Enqueue(audioSources[1]);
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
		activeSource.Play();
	}
}
