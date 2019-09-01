using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBoard : MonoBehaviour
{
    private Nine.Core.Board gameBoard;
    private Sprite sprite;
    private Transform position;
    private List<ViewBlock> blocks;

    public bool IsGameOver = false;
    public ViewBlockFactory ViewBlockFactory;
    public float ScrollTime;
    public int PlayerIndex;
    public Cursor Cursor;

    // Start is called before the first frame update
    void Start()
    {
        sprite = this.GetComponent<Sprite>();
        position = this.GetComponent<Transform>();
        gameBoard = new Nine.Core.Board(ScrollTime);
        blocks = new List<ViewBlock>();
        for (int x = 0; x < Nine.Core.Board.ROW_WIDTH; x++)
        {
            for (int y = 0; y < Nine.Core.Board.COLUMN_HEIGHT; y++)
            {
                if (gameBoard.Blocks[y][x] != null)
                {
                    blocks.Add
                    (
                        ViewBlockFactory.CreateBlock(gameBoard.Blocks[y][x], this.transform.position + new Vector3(x + 1, y + 1, 1), this.transform.rotation)
                    );
                }
            }
        }
        Cursor = GameObject.Instantiate(Cursor);
    }

    private void updateCursor()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Cursor.Move(-1, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Cursor.Move(1, 0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Cursor.Move(0, -1);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Cursor.Move(0, 1);
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (IsGameOver)
        {
            return;
        }
        updateCursor();

        gameBoard.Update(Time.deltaTime);

        while (gameBoard.ScrollProgress >= gameBoard.ScrollTime)
        {
            gameBoard.ShiftBlocksUp();
            for (int x = 0; x < Nine.Core.Board.ROW_WIDTH; x++)
            {
                blocks.Add
                (
                    ViewBlockFactory.CreateBlock(gameBoard.Blocks[0][x], this.transform.position + new Vector3(x + 1, 1, 1), this.transform.rotation)
                );
            }
            Cursor.Move(0, 1);
        }

        foreach (var block in blocks)
        {
            block.transform.position = this.transform.position + new Vector3(block.Block.Position.X + 1, block.Block.Position.Y + 1 + (gameBoard.ScrollProgress % gameBoard.ScrollTime) / gameBoard.ScrollTime, 1);
        }
        Cursor.transform.position = this.transform.position + new Vector3(Cursor.GridPosition.X + 1, Cursor.GridPosition.Y + 1 + (gameBoard.ScrollProgress % gameBoard.ScrollTime) / gameBoard.ScrollTime, 1);

        IsGameOver = gameBoard.GameOver;
    }
}
