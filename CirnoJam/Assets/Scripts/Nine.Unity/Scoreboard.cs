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
		ScoreboardText = Instantiate(ScoreboardText, new Vector3(850, 500, -50), this.transform.rotation);
		ScoreboardText.transform.SetParent(this.transform, true);
	}

    // Update is called once per frame
    void Update()
    {
		ViewBoard = FindObjectOfType<ViewBoard>();
		ScoreboardText.text = $"Score: { ViewBoard.Score }\nBlocks Destroyed: { ViewBoard.BlocksCleared }";

	}
}
