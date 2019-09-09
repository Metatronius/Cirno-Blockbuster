using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
	public float MusicVolume;
	public float SFXVolume;
    // Start is called before the first frame update
    void Awake()
    {
		if(PlayerPrefs.HasKey("MusicVolume"))
		{
			MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
		}
		else
		{

			MusicVolume = .5f;
		}
		if(PlayerPrefs.HasKey("SFXVolume"))
		{
			SFXVolume = PlayerPrefs.GetFloat("SFXVolume");
		}
		else
		{
			SFXVolume = 1f;
		}
		SaveSettings();
	}
	public void SetMusicVolume(float volume)
	{
		MusicVolume = volume;
		SaveSettings();
	}
	public void SetSFXVolume(float volume)
	{
		SFXVolume = volume;
		SaveSettings();
	}
	

	public void SaveSettings()
	{
		PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
		PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
		PlayerPrefs.Save();
	}

}
