using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public enum PlayerState
{
	Ground,
	Jumping,
}

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private float _speedModifier = 1.0f;

	[SerializeField]
	private float _jumpForce = 10.0f;

	private Rigidbody2D _rigidbody;

	private PlayerState _state;
	private float _desiredSpeed = 0.0f;
	private bool _desiresToJump = false;

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
			break;
		}
		case PlayerState.Jumping:
		{
			_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
			_rigidbody.AddForce(new Vector2(0.0f, _jumpForce), ForceMode2D.Impulse);
			break;
		}
		}

		_state = newState;
	}

	// Start is called before the first frame update
	void Start()
	{
		_rigidbody = GetComponent<Rigidbody2D>();

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
		_desiresToJump = Input.GetButtonDown("Jump");
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
	}

	private void UpdateOnGround()
	{
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
