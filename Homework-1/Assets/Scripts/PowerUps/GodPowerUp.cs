using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodPowerUp : PowerUp
{
	private int _startHP;
	private PlayerAttributes _playerAttributes;


	public GodPowerUp(GameObject gameObject)
		: base(gameObject)
	{}

	public override void OnStart()
	{
		base.OnStart();

		_playerAttributes = gameObject.GetComponent<PlayerAttributes>();
		_startHP = _playerAttributes.GetHealth();
	}

	public override void Update()
	{
		_playerAttributes.SetAttribute(AttributeType.Health, _startHP);
	}
}
