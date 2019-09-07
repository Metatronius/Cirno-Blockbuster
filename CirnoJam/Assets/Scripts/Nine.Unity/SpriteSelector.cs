using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSelector : MonoBehaviour
{
    private float spriteTimer = 0;
	private Sprite lastSprite;
	private Sprite defaultSprite;
	private bool timing = false;
	public void SetSprite(Sprite s)
	{
		defaultSprite = s;
		lastSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
		this.gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite;
	}
	public void SetSprite(Sprite s, float Time)
	{
		this.gameObject.GetComponent<SpriteRenderer>().sprite = s;
		spriteTimer = Time;
		timing = true;
	}
	public void TransitionSprites(Sprite a, float Time, Sprite b)
	{
		lastSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
		this.gameObject.GetComponent<SpriteRenderer>().sprite = a;
		defaultSprite = b;
		spriteTimer = Time;
		timing = true;
	}
	private void Update()
	{
		if (timing)
		{
			if (spriteTimer >= 0)
			{
				spriteTimer -= Time.deltaTime;
			}
			else
			{
				this.gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite;
				timing = false;
			}
		}
	}

}
