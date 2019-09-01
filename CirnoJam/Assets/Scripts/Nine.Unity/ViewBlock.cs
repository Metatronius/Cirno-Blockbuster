using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBlock : MonoBehaviour
{
    private Transform position;

    public Nine.Core.Block Block;
    public Sprite Sprite;
   
    // Start is called before the first frame update
    void Start()
    {
        Sprite = this.GetComponent<Sprite>();
        position = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
