using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ItemPickUp : MonoBehaviour
{
	[SerializeField]
	private AttributeType _type;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.gameObject.CompareTag("Player"))
		{
			return;
		}

		UseItem(other.gameObject);
	}

	private void UseItem(GameObject obj)
	{
		Debug.LogFormat("Item {0} used", name);

		PlayerAttributes attributes = obj.GetComponent<PlayerAttributes>();
		attributes.AddAttribute(_type);

		Destroy(gameObject);
	}
}
