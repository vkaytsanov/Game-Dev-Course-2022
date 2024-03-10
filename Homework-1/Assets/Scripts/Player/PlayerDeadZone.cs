using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadZone : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.gameObject.CompareTag("Player"))
		{
			return;
		}

		PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
		int currentHealth = pc.TakeDamage(1);

		if (currentHealth > 0)
		{
			Game.Instance.RespawnPlayer(collision.gameObject);
		}
	}
}
