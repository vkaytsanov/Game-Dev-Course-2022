using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
struct ParallaxInstance
{
	public GameObject Object;

	public float EffectAmount;

	[HideInInspector]
	public Vector2 Length;

	[HideInInspector]
	public Vector3 StartPosition;
};

public class ParallaxEffect : MonoBehaviour
{
	[SerializeField]
	private ParallaxInstance[] _instances;

	private Vector3 _prevPosition;

	void Start()
	{
		_prevPosition = transform.position;

		for (int i = 0; i < _instances.Length; ++i)
		{
			_instances[i].StartPosition = _instances[i].Object.transform.position;
			_instances[i].Length = _instances[i].Object.GetComponent<SpriteRenderer>().bounds.size;
		}
	}

	void FixedUpdate()
	{
		for (int i = 0; i < _instances.Length; i++)
		{
			ParallaxInstance instance = _instances[i];
			Vector3 temp = transform.position * (1 - instance.EffectAmount);
			Vector3 dist = transform.position * (instance.EffectAmount);

			Vector3 newPosition = instance.StartPosition + dist;
			instance.Object.transform.position = new Vector3(newPosition.x, newPosition.y, instance.Object.transform.position.z);
			
			if (temp.x > instance.StartPosition.x + instance.Length.x)
			{
				_instances[i].StartPosition.x += instance.Length.x;
			}
			else if (temp.x < instance.StartPosition.x - instance.Length.x)
			{
				_instances[i].StartPosition.x -= instance.Length.x;
			}

			if (temp.y > instance.StartPosition.y + instance.Length.y)
			{
				_instances[i].StartPosition.y += instance.Length.y;
			}
			else if (temp.y < instance.StartPosition.y - instance.Length.y)
			{
				_instances[i].StartPosition.y -= instance.Length.y;
			}
		}

		// set the previousCamPs to the cams pos at the end of the frame
		_prevPosition = transform.position;
	}
}
