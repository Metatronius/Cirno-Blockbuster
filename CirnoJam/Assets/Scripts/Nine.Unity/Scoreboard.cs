using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
	public Text ScoreboardText;
	
	private ViewBoard ViewBoard;

	// Start is called before the first frame update
	void Start()
    {

	}

    // Update is called once per frame
    void Update()
    {
		ViewBoard = FindObjectOfType<ViewBoard>();
		ScoreboardText.text = $"Score: { ViewBoard.Score }\nBlocks Cleared: { ViewBoard.BlocksCleared }\nCombo: { ViewBoard.CurrentCombo }\nCombo Timer: { ViewBoard.ComboTimeRemaining }/{ ViewBoard.ComboTimeMax }";
		

	}
	
}
