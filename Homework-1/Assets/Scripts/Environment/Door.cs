using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	[SerializeField]
	private Sprite _closedDoorSprite;

	[SerializeField]
	private Sprite _openedDoorSprite;

	private SpriteRenderer _spriteRenderer;

	private PlayerAttributes _playerAttributes;

	private bool _isOpen = false;

	private void OnEnable()
	{
		Game.Instance.RegisterForPlayerCreated(OnPlayerCreated);
	}

	private void OnDisable()
	{
		if (_playerAttributes)
		{
			_playerAttributes.UnregisterForEvent(AttributeType.Keys, OnKeysChanged);
		}

		//Game.Instance.UnregisterForPlayerCreated(OnPlayerCreated);
	}

	private void OnPlayerCreated(GameObject player)
	{
		_playerAttributes = player.GetComponent<PlayerAttributes>();
		_playerAttributes.RegisterForEvent(AttributeType.Keys, OnKeysChanged);
	}

	private void OnKeysChanged(int count)
	{
		// TODO: Remove this hardcoded number
		if (count == 3)
		{
			_spriteRenderer.sprite = _openedDoorSprite;
			_isOpen = true;
		}
	}


	private void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_spriteRenderer.sprite = _closedDoorSprite;
		_isOpen = false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (GameTags.IsPlayer(collision))
		{
			if (_isOpen)
			{
				// Win state
				Debug.Log("All keys collected!");
				Game.Instance.OnPlayerWon();
			}
			else
			{
				Debug.Log("Not all keys collected!");
				// Show UI message ?
			}
		}
	}
}
