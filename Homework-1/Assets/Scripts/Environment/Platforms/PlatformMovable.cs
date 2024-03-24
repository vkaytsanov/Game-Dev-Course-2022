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

	public float GetLeftWorldBoundary()
	{
		return _initialPosition.x - _leftDownBoundary;
	}

	public float GetRightWorldBoundary()
	{
		return _initialPosition.x + _rightUpBoundary;
	}

	public float GetDownWorldBoundary()
	{
		return _initialPosition.y - _leftDownBoundary;
	}

	public float GetUpWorldBoundary()
	{
		return _initialPosition.y + _rightUpBoundary;
	}

	public float GetDownBoundary()
	{
		return _leftDownBoundary;
	}

	public Vector2 GetInitialPosition()
	{
		return _initialPosition;
	}

	public bool IsMovingDown()
	{
		return _rigidbody.velocity.y < 0;
	}

	public void MoveRight()
	{
		_rigidbody.velocity = new Vector2(_speed, _rigidbody.velocity.y);
	}

	public void MoveUp()
	{
		_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _speed);
	}

	public void MoveLeft()
	{
		_rigidbody.velocity = new Vector2(-_speed, _rigidbody.velocity.y);
	}

	public void MoveDown()
	{
		_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -_speed);
	}
	
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
			if (transform.position.x <= GetLeftWorldBoundary())
			{
				MoveRight();
			}
			else if (transform.position.x >= GetRightWorldBoundary())
			{
				MoveLeft();
			}
		}
		else
		{
			if (transform.position.y <= GetDownWorldBoundary())
			{
				MoveUp();
			}
			else if (transform.position.y >= GetUpWorldBoundary())
			{
				MoveDown();
			}
		}
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		collision.gameObject.transform.parent = transform;
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		collision.gameObject.transform.parent = null;
	}
}
