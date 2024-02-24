using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformMovingDirection
{
	Horizontally,
	Vertically,
}

public class PlatformMovable : MonoBehaviour
{
	[SerializeField]
	private float _leftDownBoundary = 3.0f;
	
	[SerializeField]
	private float _rightUpBoundary = 3.0f;

	[SerializeField]
	private float _speed = 2.0f;

	[SerializeField]
	private PlatformMovingDirection _direction = PlatformMovingDirection.Horizontally;

	private Rigidbody2D _rigidbody;

	private Vector3 _initialPosition;
	
	// Start is called before the first frame update
	void Start()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_initialPosition = transform.position;

		int randomInitialDirection = Random.Range(0, 50) < 25 ? -1 : 1;
		float velocityAxis = _speed * randomInitialDirection;

		if (_direction == PlatformMovingDirection.Horizontally)
		{
			_rigidbody.velocity = new Vector2(velocityAxis, _rigidbody.velocity.y);
		}
		else
		{
			_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, velocityAxis);
		}
		
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (_direction == PlatformMovingDirection.Horizontally)
		{
			if (transform.position.x <= _initialPosition.x - _leftDownBoundary)
			{
				// Go Right
				_rigidbody.velocity = new Vector2(_speed, _rigidbody.velocity.y);
			}
			else if (transform.position.x >= _initialPosition.x + _rightUpBoundary)
			{
				// Go Left
				_rigidbody.velocity = new Vector2(-_speed, _rigidbody.velocity.y);
			}
		}
		else
		{
			if (transform.position.y <= _initialPosition.y - _leftDownBoundary)
			{
				// Go Up
				_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _speed);
			}
			else if (transform.position.y >= _initialPosition.y + _rightUpBoundary)
			{
				// Go Down
				_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -_speed);
			}
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (!collision.gameObject.CompareTag("Player"))
		{
			return;
		}

		// TODO: Move the player with the platform
	}
}
