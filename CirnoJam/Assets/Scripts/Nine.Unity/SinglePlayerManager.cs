using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerManager : MonoBehaviour
{
    public MusicManager MusicManager;
    public ViewBoard ViewBoard;
    private bool intense = false;
    private bool isGameOver = false;


	// Start is called before the first frame update
	void Start()
    {
        MusicManager = GameObject.Instantiate(MusicManager);
        MusicManager.PlayTrack(MusicManager.MainThemeIntro, MusicManager.MainThemeLoop);

        ViewBoard = GameObject.Instantiate(ViewBoard, new Vector2(-4, -7), this.transform.rotation);

		
    }

    // Update is called once per frame
    void Update()
    {

		if (ViewBoard.StackHeight >= Nine.Core.Board.COLUMN_HEIGHT - 4 && !intense)
        {
            MusicManager.PlayTrack(MusicManager.IntenseThemeIntro, MusicManager.IntenseThemeLoop);
            intense = true;
        }
        else if(ViewBoard.StackHeight < Nine.Core.Board.COLUMN_HEIGHT - 6 && intense)
        {
            MusicManager.PlayTrack(MusicManager.MainThemeIntro, MusicManager.MainThemeLoop);
            intense = false;
        }

        if(ViewBoard.IsGameOver && !isGameOver)
        {
            
            MusicManager.PlayTrack(MusicManager.Loss, MusicManager.PostGameLoop);
            isGameOver = true;

        }
    }
}
