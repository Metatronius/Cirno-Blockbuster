using System;
using System.Collections;
using System.Collections.Generic;

namespace Nine.Core
{
	public class ComboMeter : IUpdatable
	{
		public float MeterTimeMax { get; private set; }
		public float MeterTimeRemaining { get; private set; }
		public int Combo { get; private set; }

		public event EventHandler<int> MeterDepleted;

		public ComboMeter(float meterTime)
		{
			MeterTimeMax = meterTime;
		}

		public void AddCombo()
		{
			if (MeterTimeRemaining <= 0)
			{
				MeterTimeRemaining = MeterTimeMax;
				Combo++;
			}

			MeterTimeRemaining += .5f;
			Combo++;
		}

		public void Update(float deltaTime)
		{
			if (MeterTimeRemaining <= 0)
			{
				return;
			}

			MeterTimeRemaining -= deltaTime;

			if (MeterTimeRemaining <= 0)
			{
				if (MeterDepleted != null)
				{
					MeterDepleted.Invoke(this, Combo);
				}
				
				Combo = 0;
				MeterTimeRemaining = 0;
			}
		}
	}
}