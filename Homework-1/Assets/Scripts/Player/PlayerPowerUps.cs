using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
	private List<PowerUp> _activePowerUps;

	public PlayerPowerUps()
	{
		_activePowerUps = new List<PowerUp>();
	}

	public void ActivatePowerUp(PowerUp powerUp)
	{
		_activePowerUps.Add(powerUp);

		powerUp.OnStart();
	}

	// Update is called once per frame
	void Update()
	{
		for (int i = 0; i < _activePowerUps.Count; i++)
		{
			if (_activePowerUps[i].HasFinished())
			{
				_activePowerUps[i].OnEnd();
				_activePowerUps.RemoveAt(i);
				--i;
			}
			else
			{
				_activePowerUps[i].Update();
			}
			
		}
	}
}
