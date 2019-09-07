using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SinglePlayerManager : MonoBehaviour
{
    public MusicManager MusicManager;
    public ViewBoard ViewBoard;
	public SpriteSelector Mascot;
	public Mascot Mascots;
	
    private bool intense = false;
    private bool isGameOver = false;
	private uint lastScore = 0;


	// Start is called before the first frame update
	void Start()
    {
        MusicManager = GameObject.Instantiate(MusicManager);
        MusicManager.PlayTrack(MusicManager.MainThemeIntro, MusicManager.MainThemeLoop);

        ViewBoard = GameObject.Instantiate(ViewBoard, new Vector3(-6, -8, 0), this.transform.rotation);

		Mascot.SetSprite(Mascots.Normal);
		
    }
	// Update is called once per frame
	void Update()
	{
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
		if (ViewBoard.IsGameOver && !isGameOver)
		{

			MusicManager.PlayTrack(MusicManager.Loss, MusicManager.PostGameLoop);
			isGameOver = true;
		}
	}
 }
