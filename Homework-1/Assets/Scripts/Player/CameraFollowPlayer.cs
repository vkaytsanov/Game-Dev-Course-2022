using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraFollowPlayer : MonoBehaviour
{
	[SerializeField]
	private Transform _playerTransform;

	[SerializeField]
	private Transform[] _backgrounds;

	private Vector3 _offset;

	public void SetPlayerToFollow(GameObject player)
	{
		_playerTransform = player.transform;
		_offset = Vector3.zero;
	}
	void OnEnable()
	{
		Game.Instance.RegisterForPlayerCreated(SetPlayerToFollow);
	}

	void OnDisable()
	{
		//Game.Instance.UnregisterForPlayerCreated(SetPlayerToFollow);
	}

	// Update is called once per frame
	void Update()
	{
		if (_playerTransform)
		{
			Vector3 prevPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			transform.position = new Vector3(Mathf.Lerp(transform.position.x, _playerTransform.position.x, Time.deltaTime),
			                                 Mathf.Lerp(transform.position.y, _playerTransform.position.y, Time.deltaTime),
			                                 transform.position.z);

			Vector3 deltaPosition = transform.position - prevPosition;

			for (int i = 0; i < _backgrounds.Length; i++)
			{
				Vector3 newBackgroundPosition = _backgrounds[i].position + deltaPosition;
				_backgrounds[i].position = new Vector3(newBackgroundPosition.x,
				                                       newBackgroundPosition.y,
				                                       _backgrounds[i].position.z);
			}
		}
	}
}
