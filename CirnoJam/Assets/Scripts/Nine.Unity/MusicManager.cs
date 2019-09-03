using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource introSource;
    private AudioSource loopSource;
    public AudioClip MainThemeIntro;
    public AudioClip MainThemeLoop;
    public AudioClip IntenseThemeIntro;
    public AudioClip IntenseThemeLoop;
    public AudioClip Victory;
    public AudioClip Loss;
    public AudioClip PostGameIntro;
    public AudioClip PostGameLoop;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayTrack(AudioClip intro, AudioClip loop)
    {
        var sources = GetComponents<AudioSource>();
        introSource = sources[0];
        loopSource = sources[1];

        introSource.Stop();
        loopSource.Stop();

        introSource.loop = false;
        loopSource.loop = true;

        introSource.clip = intro;
        loopSource.clip = loop;

        var introDuration = (double)intro.samples / intro.frequency;
        introSource.volume = .5f;
        loopSource.volume = .5f;
        introSource.Play();
        loopSource.PlayScheduled(AudioSettings.dspTime + introDuration);
    }
}
