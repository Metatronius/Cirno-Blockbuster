using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBoard : MonoBehaviour
{
    private Nine.Core.Board gameBoard;
    private Sprite sprite;
    private Transform position;
    private List<ViewBlock> blocks;

    public ViewBlockFactory ViewBlockFactory;

    // Start is called before the first frame update
    void Start()
    {
        sprite = this.GetComponent<Sprite>();
        position = this.GetComponent<Transform>();
        gameBoard = new Nine.Core.Board();

        for(int x = 0; x < Nine.Core.Board.ROW_WIDTH; x++)
        {
            for(int y = 0; y < Nine.Core.Board.COLUMN_HEIGHT; y++)
            {
                if (gameBoard.Blocks[y][x] != null)
                {
                    ViewBlockFactory.CreateBlock(this.transform.position + new Vector3(x + 1, y + 1, -1), this.transform.rotation);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameBoard.Update(Time.deltaTime);
    }
}
