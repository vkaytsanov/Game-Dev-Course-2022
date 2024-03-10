using System;
using UnityEngine;

public enum PlayerState
{
	Ground,
	Jumping,
	Dead,
}

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private float _speedModifier = 1.0f;

	[SerializeField]
	private float _jumpForce = 10.0f;

	private SpriteRenderer _spriteRenderer;

	private Rigidbody2D _rigidbody;

	private Animator _animator;

	private PlayerState _state;
	private float _desiredSpeed = 0.0f;
	private bool _desiresToJump = false;

	public bool IsInState(PlayerState state)
	{
		return _state == state;
	}

	public void ChangePlayerState(PlayerState newState)
	{
		if (_state == newState)
		{
			return;
		}

		Debug.LogFormat("Change player state to {0}", newState);

		switch (newState)
		{
		case PlayerState.Ground:
		{
			_desiresToJump = false;
			_animator.SetBool("IsJumping", false);
			break;
		}
		case PlayerState.Jumping:
		{
			_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
			_rigidbody.AddForce(new Vector2(0.0f, _jumpForce), ForceMode2D.Impulse);

			_animator.SetBool("IsJumping", true);
			break;
		}
		case PlayerState.Dead:
		{
			_animator.SetBool("IsAlive", false);
			break;
		}
		}

		_state = newState;
	}

	// Start is called before the first frame update
	void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_rigidbody = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();

		if (Math.Abs(_rigidbody.velocity.y) > 0.0f)
		{
			_state = PlayerState.Jumping;
		}
		else
		{
			_state = PlayerState.Ground;
		}
	}

	// Update is called once per frame
	void Update()
	{
		_desiredSpeed = Input.GetAxis("Horizontal") * _speedModifier;

		if (!_desiresToJump)
		{
			_desiresToJump = Input.GetButtonDown("Jump");
		}
	}

	private void FixedUpdate()
	{
		UpdateCurrentState();
	}

	private void UpdateCurrentState()
	{
		UpdateMovement();

		switch (_state)
		{
		case PlayerState.Ground:
		{
			UpdateOnGround();
			break;
		}
		case PlayerState.Jumping:
		{
			UpdateJumping();
			break;
		}
		}
	}

	private void UpdateMovement()
	{
		_rigidbody.velocity = new Vector2(_desiredSpeed, _rigidbody.velocity.y);

		if (_rigidbody.velocity.x != 0.0f)
		{
			_spriteRenderer.flipX = Mathf.Sign(_rigidbody.velocity.x) < 0.0f;
		}
	}

	private void UpdateOnGround()
	{
		_animator.SetBool("IsWalking", _rigidbody.velocity.x != 0.0f);

		if (_desiresToJump)
		{
			_desiresToJump = false;

			ChangePlayerState(PlayerState.Jumping);
		}
	}

	private void UpdateJumping()
	{
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (_state == PlayerState.Jumping)
		{
			if (collision.contactCount > 0)
			{
				ContactPoint2D contact = collision.GetContact(0);
				if (Vector3.Dot(contact.normal, Vector3.up) > 0.5)
				{
					ChangePlayerState(PlayerState.Ground);
				}
			}
		}
	}
}
