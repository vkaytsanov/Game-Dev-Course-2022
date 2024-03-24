using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDroppable : MonoBehaviour
{
	private Rigidbody2D _rigidbody;
	private Vector2 _spawnPosition;


	public void Respawn()
	{
		transform.position = _spawnPosition;
		_rigidbody.gravityScale = 0.0f;
		_rigidbody.velocity = Vector2.zero;
		_rigidbody.rotation = 0.0f;
		_rigidbody.angularVelocity = 0.0f;
	}

	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_rigidbody.gravityScale = 0.0f;
		_spawnPosition = transform.position;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (GameTags.IsPlayer(collision))
		{
			// Platform should be below the player?
			if (collision.transform.position.y > transform.position.y)
			{
				_rigidbody.gravityScale = 1.0f;

				Invoke("Respawn", 2.0f);
			}
		}
	}
}
