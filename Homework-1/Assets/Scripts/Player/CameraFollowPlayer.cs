using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
	[SerializeField]
	private Transform _playerTransform;

	private Vector3 _offset;

	public void SetPlayerToFollow(GameObject player)
	{
		_playerTransform = player.transform;
	}

	// Start is called before the first frame update
	void Start()
	{
		_offset = transform.position - _playerTransform.position;
	}

	// Update is called once per frame
	void Update()
	{
		transform.position = Vector3.Lerp(transform.position, _playerTransform.position + _offset, Time.deltaTime);
	}
}
