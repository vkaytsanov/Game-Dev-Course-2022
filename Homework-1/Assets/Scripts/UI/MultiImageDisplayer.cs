using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MultiImageDisplayer : MonoBehaviour
{
	[SerializeField]
	private GameObject[] _images;

	[SerializeField]
	private AttributeType _type;

	private PlayerAttributes _playerAttributes;

	private void OnEnable()
	{
		Game.Instance.RegisterForPlayerCreated(SetPlayer);
	}

	private void OnDisable()
	{
		if (_playerAttributes)
		{
			_playerAttributes.UnregisterForEvent(_type, SetActiveCount);
		}

		Game.Instance.UnregisterForPlayerCreated(SetPlayer);
	}

	private void SetPlayer(GameObject player)
	{
		_playerAttributes = player.GetComponent<PlayerAttributes>();
		_playerAttributes.RegisterForEvent(_type, SetActiveCount);
	}

	void SetActiveCount(int count)
	{
		for (int i = 0; i < _images.Length; i++)
		{
			_images[i].SetActive(i < count);
		}
	}
}
