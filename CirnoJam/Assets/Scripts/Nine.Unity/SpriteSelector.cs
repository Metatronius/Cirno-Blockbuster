using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSelector : MonoBehaviour
{
    private float spriteTimer = 0;
	private Sprite lastSprite;
	private bool timing = false;
	public void SetSprite(Sprite s)
	{
		lastSprite = this.GetComponent<SpriteRenderer>().sprite;
		this.gameObject.GetComponent<SpriteRenderer>().sprite = s;
	}
	public void SetSprite(Sprite s, float Time)
	{
		SetSprite(s);
		spriteTimer = Time;
		timing = true;
	}
	public void TransitionSprites(Sprite a, float Time, Sprite b)
	{
		this.gameObject.GetComponent<SpriteRenderer>().sprite = a;
		lastSprite = b;
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
				SetSprite(lastSprite);
				timing = false;
			}
		}
	}

}
