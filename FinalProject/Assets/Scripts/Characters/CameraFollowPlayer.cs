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

	private void Start()
	{
		_offset = transform.position - _playerTransform.position;
	}

	// Update is called once per frame
	void Update()
	{
		if (_playerTransform)
		{
			Vector3 destination = _playerTransform.position + _offset;
			transform.position = new Vector3(Mathf.Lerp(transform.position.x, destination.x, Time.deltaTime),
			                                 Mathf.Lerp(transform.position.y, destination.y, Time.deltaTime),
			                                 transform.position.z);

			for (int i = 0; i < _backgrounds.Length; i++)
			{
				_backgrounds[i].transform.position = new Vector3(Mathf.Lerp(_backgrounds[i].transform.position.x, destination.x, Time.deltaTime),
				                                                 Mathf.Lerp(_backgrounds[i].transform.position.y, destination.y, Time.deltaTime),
				                                                 _backgrounds[i].transform.position.z);
			}
		}
	}
}
