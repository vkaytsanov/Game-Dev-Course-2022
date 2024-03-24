using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField]
	private float _speed = 5.0f;

	[SerializeField]
	private float _lifeTimeSeconds = 3.0f;

	private float _lifeTimeEnd;

	private Vector3 _direction;

	public void SetDirection(Vector3 direction)
	{
		_direction = direction;
	}


	private void Start()
	{
		_lifeTimeEnd = Time.time + _lifeTimeSeconds;
	}

	void Update()
	{
		if (_lifeTimeEnd < Time.time)
		{
			Debug.Log("Projectile destroyed");
			Destroy(gameObject);
			return;
		}

		transform.position += _direction * _speed * Time.deltaTime;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (GameTags.IsEnemy(collision))
		{
			// TODO: Enemy attributes ?
			Destroy(collision.gameObject);
		}

		Destroy(gameObject);
	}
}
