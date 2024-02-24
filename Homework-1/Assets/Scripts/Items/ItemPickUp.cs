using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ItemPickUp : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.gameObject.CompareTag("Player"))
		{
			return;
		}

		UseItem();
	}

	private void UseItem()
	{
		Debug.LogFormat("Item {0} used", name);
		Destroy(gameObject);
	}
}
