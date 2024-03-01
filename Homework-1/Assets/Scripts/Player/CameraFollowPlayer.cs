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
		_offset = Vector3.zero;
	}

	// Start is called before the first frame update
	void Start()
	{
		
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
		}
	}
}
