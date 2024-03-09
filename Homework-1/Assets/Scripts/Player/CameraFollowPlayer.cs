using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraFollowPlayer : MonoBehaviour
{
	[SerializeField]
	private Transform _playerTransform;

	[SerializeField]
	private Transform _backgroundTransform;

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
		Game.Instance.UnregisterForPlayerCreated(SetPlayerToFollow);
	}

	// Update is called once per frame
	void Update()
	{
		if (_playerTransform)
		{
			float cameraDepth = transform.position.z;
			transform.position = new Vector3(Mathf.Lerp(transform.position.x, _playerTransform.position.x, Time.deltaTime),
			                                 Mathf.Lerp(transform.position.y, _playerTransform.position.y, Time.deltaTime),
			                                 transform.position.z);

			if (_backgroundTransform)
			{
				_backgroundTransform.position = new Vector3(transform.position.x, transform.position.y, _backgroundTransform.position.z);
			}
		}
	}
}
