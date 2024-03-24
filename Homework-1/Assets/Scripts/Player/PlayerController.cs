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

	private event Action<GameObject> _onPlayerDeadAction;

	public void RegisterForPlayerDead(Action<GameObject> func)
	{
		_onPlayerDeadAction += func;
	}

	public void UnregisterForPlayerDead(Action<GameObject> func)
	{
		_onPlayerDeadAction -= func;
	}

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
			_animator.SetTrigger("Hurt");
			_onPlayerDeadAction?.Invoke(gameObject);
			break;
		}
		}

		_state = newState;
	}

	public int TakeDamage(int amount)
	{
		PlayerAttributes playerAttributes = GetComponent<PlayerAttributes>();

		int health = playerAttributes.GetHealth() - amount;
		playerAttributes.SetAttribute(AttributeType.Health, health);

		if (health <= 0)
		{
			ChangePlayerState(PlayerState.Dead);
		}
		else
		{
			_animator.SetTrigger("Hurt");
		}

		return health;
	}

	public int TakeDamage(int amount, Vector3 direction)
	{
		ApplyPushBackEffect(direction);
		return TakeDamage(amount);
	}

	private void ApplyPushBackEffect(Vector3 direction)
	{
		Vector3 force;
		if (Vector3.Dot(direction, Vector3.left) > 0)
		{
			force = Vector3.LerpUnclamped(Vector3.left, Vector3.up, 0.2f);
		}
		else
		{
			force = Vector3.LerpUnclamped(Vector3.right, Vector3.up, 0.2f);
		}

		_rigidbody.velocity = Vector2.zero;

		float forceMagnitute = _state == PlayerState.Jumping ? 3.0f : 15.0f;
		_rigidbody.AddForce(force * forceMagnitute, ForceMode2D.Impulse);
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
		if (_state != PlayerState.Dead)
		{
			_rigidbody.velocity = new Vector2(_desiredSpeed, _rigidbody.velocity.y);

			if (_rigidbody.velocity.x != 0.0f)
			{
				_spriteRenderer.flipX = Mathf.Sign(_rigidbody.velocity.x) < 0.0f;
			}
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
