using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PowerUpPickUp : MonoBehaviour
{
	[SerializeField]
	private PowerUp _powerUp;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!GameTags.IsPlayer(other))
		{
			return;
		}

		UsePowerUp(other.gameObject);
	}

	private void UsePowerUp(GameObject obj)
	{
		Debug.LogFormat("PowerUp {0} used", name);

		PlayerPowerUps powerUps = obj.GetComponent<PlayerPowerUps>();
		powerUps.ActivatePowerUp(_powerUp);

		Destroy(gameObject);
	}
}
