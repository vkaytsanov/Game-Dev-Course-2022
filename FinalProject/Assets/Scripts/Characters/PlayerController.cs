using System;
using UnityEngine;

public enum PlayerState
{
	Ground,
}

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private float _speedModifier = 1.0f;

	private SpriteRenderer _spriteRenderer;

	private Animator _animator;

	private AudioSource _walkAudio;

	private PlayerState _state;
	private float _desiredSpeed = 0.0f;
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

		_state = newState;
	}

	// Start is called before the first frame update
	void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_animator = GetComponent<Animator>();
		_walkAudio = GetComponent<AudioSource>();

		_state = PlayerState.Ground;
	}

	// Update is called once per frame
	void Update()
	{
		_desiredSpeed = Input.GetAxis("Horizontal") * _speedModifier;
	}

	private void FixedUpdate()
	{
		UpdateCurrentState();
	}

	private void UpdateCurrentState()
	{
		UpdateMovement();
	}

	private void UpdateMovement()
	{
		transform.position += new Vector3(_desiredSpeed * Time.fixedDeltaTime, 0, 0);
		_animator.SetInteger("MovementSpeed", Mathf.CeilToInt(_desiredSpeed));

		if (_desiredSpeed != 0.0f)
		{
			_spriteRenderer.flipX = Mathf.Sign(_desiredSpeed) < 0.0f;

			if (!_walkAudio.isPlaying)
			{
				_walkAudio.Play();
			}
		}
		else
		{
			_walkAudio.Stop();
		}
	}
}
