using System;
using UnityEngine;
using UnityEngine.Assertions;
using Utils.Attributes;

[Serializable]
public enum AIState
{
	Idle,
	Walking,
	Attack,
}

public class AIController : MonoBehaviour
{
	private AIState _state;

	private SpriteRenderer _spriteRenderer;

	private Rigidbody2D _rigidbody;

	private Animator _animator;

	private BoxCollider2D _boxCollider;

	private BoxCollider2D _currentPlatform;

	[SerializeField]
	[NamedArrayAttribute(typeof(AIState))]
	private int[] _decisionChances = new int[Enum.GetNames(typeof(AIState)).Length];

	[SerializeField]
	private float _walkingSpeed;

	[SerializeField]
	private float _attackSpeed;

	[SerializeField]
	private const float _nextDecisionCycleInSeconds = 2.0f;

	[SerializeField]
	private float _chasePlayerSeconds = 30.0f;

	private int _totalDecisionValue = 0;

	private float _nextDecisionTime = float.MinValue;

	private float _desiredVelocity = 0;

	private PlayerController _attackedPlayer;

	// Start is called before the first frame update
	void Start()
	{
		_state = AIState.Idle;

		_spriteRenderer = GetComponent<SpriteRenderer>();
		_rigidbody = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
		_boxCollider = GetComponent<BoxCollider2D>();

		foreach (var decision in _decisionChances)
		{
			_totalDecisionValue += decision;
		}

		AdvanceNextDecisionTime();
	}

	// Update is called once per frame
	void Update()
	{
		//if (_attackedTransform)
		//{
		//	// Multiplayer?
		//	Vector2 forwardVector = _spriteRenderer.flipX ? Vector2.left : Vector2.right;
		//	Vector2 toTarget = _attackedTransform.position - transform.position;
		//	toTarget.Normalize();

		//	float dotToTarget = Vector2.Dot(forwardVector, toTarget);
		//	Debug.LogFormat("{0}, {1}, {2}, {3}", forwardVector, toTarget, dotToTarget, Mathf.Acos(_sightAngle));
		//	if (dotToTarget > 0)
		//	{
		//		// TODO: Maybe cache the cosine sightAngle ?
		//		if (dotToTarget < Mathf.Acos(_sightAngle))
		//		{
		//			ChangeState(AIState.Attack);
		//			AdvanceNextDecisionTime(100);
		//		}
		//	}
		//}

		if (Time.time > _nextDecisionTime)
		{
			ChangeState(GenerateNextState());
			AdvanceNextDecisionTime();
		}

		UpdateState();
	}

	private AIState GenerateNextState()
	{
		int total = 0;
		int roll = UnityEngine.Random.Range(0, _totalDecisionValue);
		for (int i = 0; i < _decisionChances.Length; i++)
		{
			total += _decisionChances[i];
			if (roll < total)
			{
				return (AIState)i;
			}
		}

		Assert.IsTrue(false);
		return AIState.Idle;
	}

	private void UpdateState()
	{
		switch (_state)
		{
		case AIState.Idle:
		{
			UpdateOnIdle();
			break;
		}
		case AIState.Walking:
		{
			UpdateOnWalking();
			break;
		}
		case AIState.Attack:
		{
			UpdateOnAttack();
			break;
		}
		}
	}

	public void ChangeState(AIState state)
	{
		if (_state == state)
		{
			return;
		}

		Debug.LogFormat("Change enemy {0} state to {1}", name, state);

		switch (state)
		{
		case AIState.Idle:
		{
			StopMoving();

			_animator.SetBool("IsJumping", false);
			break;
		}
		case AIState.Walking:
		{
			bool shouldGoLeft = UnityEngine.Random.Range(0, 2) != 0;
			if (shouldGoLeft)
			{
				_desiredVelocity = -1.0f * _walkingSpeed;
			}
			else
			{
				_desiredVelocity = 1.0f * _walkingSpeed;
			}
			break;
		}
		}


		_state = state;
	}

	private void UpdateOnWalking()
	{
		if (HasReachedEndOfPlatform())
		{
			ChangeState(AIState.Idle);
			AdvanceNextDecisionTime();
		}
		else
		{
			MoveAuto();
		}
	}

	private void UpdateOnIdle()
	{
	}

	private void UpdateOnAttack()
	{
		_desiredVelocity = Mathf.Clamp(_attackedPlayer.transform.position.x - transform.position.x, -1, 1) * _attackSpeed;

		if (HasReachedEndOfPlatform() || !_attackedPlayer.IsInState(PlayerState.Ground))
		{
			StopMoving();
		}
		else
		{
			MoveAuto();
		}
	}

	private void AdvanceNextDecisionTime(float afterSeconds = _nextDecisionCycleInSeconds)
	{
		_nextDecisionTime = Time.time + afterSeconds;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject collider = collision.gameObject;
		if (collider.CompareTag("Player"))
		{
			PlayerController pc = collider.GetComponent<PlayerController>();
			pc.TakeDamage(1);
		}
		else
		{
			// Platform, they should be below, shouldn't they?
			if (collider.transform.position.y < transform.position.y)
			{
				_currentPlatform = collider.GetComponent<BoxCollider2D>();
			}
		}

	}

	private bool HasReachedEndOfPlatform()
	{
		return _boxCollider.bounds.min.x < _currentPlatform.bounds.min.x && _desiredVelocity < 0.0f
			|| _boxCollider.bounds.max.x > _currentPlatform.bounds.max.x && _desiredVelocity > 0.0f;
	}

	private void StopMoving()
	{
		_rigidbody.velocity = Vector2.zero;
		_animator.SetBool("IsWalking", false);
	}

	private void MoveAuto()
	{
		_rigidbody.velocity = new Vector2(_desiredVelocity, _rigidbody.velocity.y);

		if (_rigidbody.velocity.x != 0.0f)
		{
			_spriteRenderer.flipX = Mathf.Sign(_rigidbody.velocity.x) < 0.0f;
		}

		_animator.SetBool("IsWalking", true);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (_attackedPlayer == collision.gameObject)
		{
			ChangeState(AIState.Idle);
			AdvanceNextDecisionTime();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		GameObject collider = collision.gameObject;
		if (collider.CompareTag("Player"))
		{
			_attackedPlayer = collider.gameObject.GetComponent<PlayerController>();
			ChangeState(AIState.Attack);
			AdvanceNextDecisionTime(_chasePlayerSeconds);
		}
	}

}
