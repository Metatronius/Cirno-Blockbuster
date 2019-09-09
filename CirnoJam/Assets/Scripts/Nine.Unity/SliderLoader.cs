using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEngine.UI
{

	public class SliderLoader : MonoBehaviour
	{
		private Slider slider;
		public string Key;
		// Start is called before the first frame update
		void Start()
		{
			slider = this.GetComponent<Slider>();
			if (PlayerPrefs.HasKey(Key))
			{
				slider.value = PlayerPrefs.GetFloat(Key);
			}
			else
			{
				slider.value = 0;
			}
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}