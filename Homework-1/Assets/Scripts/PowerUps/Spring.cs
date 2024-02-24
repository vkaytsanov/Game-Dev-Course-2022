using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{

	[SerializeField]
	private float _jumpForce = 10.0f;


	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.gameObject.CompareTag("Player"))
		{
			return;
		}

		if (collision.contactCount > 0)
		{
			ContactPoint2D contact = collision.GetContact(0);
			if (Vector3.Dot(contact.normal, transform.up) < -0.5)
			{
				ShootPlayer(collision.gameObject);
			}
		}
	}

	private void ShootPlayer(GameObject gameObject)
	{
		Debug.Log("Shooting player");

		Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
		rb.velocity = Vector2.zero;
		rb.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
	}
}
