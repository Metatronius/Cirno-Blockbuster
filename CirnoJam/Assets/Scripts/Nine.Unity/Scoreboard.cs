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
	private ViewBoard ViewBoard;
	private bool gameover = false;

	// Start is called before the first frame update
	void Start()
    {
		ScoreboardText = Instantiate(ScoreboardText, new Vector3(1400, 900, -50), this.transform.rotation);
		ScoreboardText.transform.SetParent(this.transform, true);
	}

    // Update is called once per frame
    void Update()
    {
		ViewBoard = FindObjectOfType<ViewBoard>();
		ScoreboardText.text = $"Score: { ViewBoard.Score }\nBlocks Cleared: { ViewBoard.BlocksCleared }";
		if(ViewBoard.IsGameOver && !gameover) 
		{
			RestartButton = Instantiate(RestartButton, new Vector3(150, 900, -50), this.transform.rotation);
			RestartButton.transform.SetParent(this.transform, true);
			MainMenuButton = Instantiate(MainMenuButton, new Vector3(150, 850, -50), this.transform.rotation);
			MainMenuButton.transform.SetParent(this.transform, true);
			QuitButton = Instantiate(QuitButton, new Vector3(150, 800, -50), this.transform.rotation);
			QuitButton.transform.SetParent(this.transform, true);
			gameover = true;
		}
	}
}
