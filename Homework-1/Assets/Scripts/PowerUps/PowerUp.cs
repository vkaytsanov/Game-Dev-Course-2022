using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PowerUp : ScriptableObject
{
	public readonly float _duration;

	private float _effectEndTime;

	protected GameObject gameObject;

	public PowerUp(GameObject gameObject)
	{
		this.gameObject = gameObject;
	}

	public virtual void OnStart()
	{
		_effectEndTime = Time.time + _duration;
	}

	public virtual void OnEnd()
	{

	}

	public bool HasFinished()
	{
		return _effectEndTime > Time.time;
	}

	public virtual void Update()
	{

	}
}
