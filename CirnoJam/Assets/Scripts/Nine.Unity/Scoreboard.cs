using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
	public Text ScoreboardText;
	public Button RestartButton;
	public Button MainMenuButton;
	public Button QuitButton;
	public RawImage GameoverBackground;
	private ViewBoard ViewBoard;
	private bool gameover = false;

	// Start is called before the first frame update
	void Start()
    {

	}

    // Update is called once per frame
    void Update()
    {
		ViewBoard = FindObjectOfType<ViewBoard>();
		ScoreboardText.text = $"Score: { ViewBoard.Score }\nBlocks Cleared: { ViewBoard.BlocksCleared }";
		if(ViewBoard.IsGameOver && !gameover)
		{
			RestartButton.gameObject.SetActive(true);
			MainMenuButton.gameObject.SetActive(true);
			QuitButton.gameObject.SetActive(true);
			GameoverBackground.gameObject.SetActive(true);
			gameover = true;
		}
	}
}
