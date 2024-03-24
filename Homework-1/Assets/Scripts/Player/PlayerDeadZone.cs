using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadZone : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (GameTags.IsPlayer(collision))
		{
			OnTriggerWithPlayer(collision);
		}
		else if (GameTags.IsPlatform(collision))
		{
			OnTriggerWithPlatform(collision);
		}
	}

	private void OnTriggerWithPlayer(Collider2D collision)
	{
		PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
		int currentHealth = pc.TakeDamage(1);

		if (currentHealth > 0)
		{
			Game.Instance.RespawnPlayer(collision.gameObject);
		}
	}

	private void OnTriggerWithPlatform(Collider2D collision)
	{
		//PlatformDroppable droppable = collision.gameObject.GetComponent<PlatformDroppable>();
		//if (droppable)
		//{
		//	droppable.Respawn();
		//}
	}
}
