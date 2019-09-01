using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Cursor : MonoBehaviour
{
    public (int X, int Y) GridPosition;

    // Start is called before the first frame update
    void Start()
    {
        GridPosition = (2, 3);
        var boards = FindObjectsOfType<ViewBoard>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void Move(int dx, int dy)
    {
        this.GridPosition = (this.GridPosition.X + dx, this.GridPosition.Y + dy);
    }
}
