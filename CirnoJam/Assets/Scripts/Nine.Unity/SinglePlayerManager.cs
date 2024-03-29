﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SinglePlayerManager : MonoBehaviour
{
    public MusicManager MusicManager;
    public ViewBoard ViewBoard;
	public SpriteSelector Mascot;
	public Mascot Mascots;
	public Button RestartButton;
	public Button MainMenuButton;
	public Button QuitButton;
	public RawImage GameoverBackground;
	public RawImage PersonalBestBackground;
	public Text PersonalBest;

	private bool gameover = false;
	private bool active = false;
	private bool intense = false;
	private uint lastScore = 0;


	// Start is called before the first frame update
	void Start()
    {
        MusicManager = GameObject.Instantiate(MusicManager);
		MusicManager.PlayTrack(MusicManager.MainThemeIntro, MusicManager.MainThemeLoop);

        ViewBoard = GameObject.Instantiate(ViewBoard, new Vector3(-6, -7, 0), this.transform.rotation);

		ViewBoard.Initialize(new System.Random().Next());
		Mascot.SetSprite(Mascots.Normal);
		
    }
	// Update is called once per frame
	void Update()
	{
		if (ViewBoard.IsGameOver && !gameover)
		{
			ToggleButtons();
			GameoverBackground.gameObject.SetActive(true);
			gameover = true;
			if(PlayerPrefs.HasKey("PersonalBest"))
			{
				if ((int)(ViewBoard.Score/100) > PlayerPrefs.GetInt("PersonalBest"))
				{
					PlayerPrefs.SetInt("PersonalBest", (int)(ViewBoard.Score / 100));
					Mascot.SetSprite(Mascots.Cheer);
					MusicManager.PlayTrack(MusicManager.Victory, MusicManager.PostGameLoop);

				}
				else
				{
					MusicManager.PlayTrack(MusicManager.Loss, MusicManager.PostGameLoop);
				}
			}
			else
			{
				PlayerPrefs.SetInt("PersonalBest", (int)(ViewBoard.Score / 100));
				MusicManager.PlayTrack(MusicManager.Victory, MusicManager.PostGameLoop);
			}
			PersonalBest.text = ("Personal Best: " + (uint)PlayerPrefs.GetInt("PersonalBest") * 100);
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ViewBoard.IsPaused = !ViewBoard.IsPaused;
			ViewBoard.ToggleBackground();
			ToggleButtons();
		}
		if(ViewBoard.Score != lastScore)
		{
			lastScore = ViewBoard.Score;
			Mascot.SetSprite(Mascots.Cheer, 1f);
		}
		if (ViewBoard.StackHeight >= Nine.Core.Board.COLUMN_HEIGHT - 4 && !intense)
		{
			MusicManager.PlayTrack(MusicManager.IntenseThemeIntro, MusicManager.IntenseThemeLoop);
			Mascot.TransitionSprites(Mascots.Surprised, 2, Mascots.Angry);
			intense = true;
		}
		else if (ViewBoard.StackHeight < Nine.Core.Board.COLUMN_HEIGHT - 6 && intense)
		{
			MusicManager.PlayTrack(MusicManager.MainThemeIntro, MusicManager.MainThemeLoop);
			Mascot.TransitionSprites(Mascots.Determined, 2, Mascots.Normal);
			intense = false;
		}
		
	}
	public void ToggleButtons()
	{
		active = !active;
		RestartButton.gameObject.SetActive(active);
		MainMenuButton.gameObject.SetActive(active);
		QuitButton.gameObject.SetActive(active);
		PersonalBest.gameObject.SetActive(active);
		PersonalBestBackground.gameObject.SetActive(active);
	}
}
