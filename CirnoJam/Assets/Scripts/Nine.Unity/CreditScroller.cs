using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScroller : MonoBehaviour
{
	public float speed;
	private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
		startPosition = this.gameObject.transform.position;
    }
	public void Reset()
	{
		this.gameObject.transform.position = startPosition;
	}
	// Update is called once per frame
	void Update()
    {
		this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + (Time.deltaTime) * speed, 0f);
    }
}
