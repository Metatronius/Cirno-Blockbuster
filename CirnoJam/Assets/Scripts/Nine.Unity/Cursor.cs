using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Cursor : MonoBehaviour
{
    public (int X, int Y) GridPosition;
    public int PlayerIndex;

    private ViewBoard board;
    // Start is called before the first frame update
    void Start()
    {
        GridPosition = (0, 0);
        var boards = FindObjectsOfType<ViewBoard>();
        board = boards.First((b) => b.PlayerIndex == this.PlayerIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(-1, 0);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(1, 0);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(0, -1);
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(0, 1);
        }

        this.transform.position = board.transform.position + new Vector3(this.GridPosition.X + 1, this.GridPosition.Y+1, -1);
    }
    void Move(int dx, int dy)
    {
        this.GridPosition = (this.GridPosition.X + dx, this.GridPosition.Y + dy);
    }
}
