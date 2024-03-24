using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerTests
{
	GameObject _platform;
	
	GameObject _player;
	PlayerController _pc;
	PlayerAttributes _attributes;

	[SetUp]
	public void SetUp()
	{
		_platform = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Environment/Platforms/Platform_Default"));

		_player = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player/Player"));
		_pc = _player.GetComponent<PlayerController>();
		_attributes = _player.GetComponent<PlayerAttributes>();
	}

	[TearDown]
	public void TearDown()
	{
		GameObject.Destroy(_player);
	}

	[UnityTest]
	public IEnumerator TakeDamageFromEnemy()
	{
		// ========================
		// Sets the player and enemy to a same position on a platform, expects the enemy to attack and do damage
		// ========================
		GameObject enemy = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Enemy"));

		int startHP = _attributes.GetHealth();
		_platform.transform.position = Vector3.zero;

		_player.transform.position = _platform.transform.position;
		_player.transform.position += new Vector3(0, _platform.GetComponent<SpriteRenderer>().bounds.size.y, 0);

		enemy.transform.position = _player.transform.position;

		yield return 1;

		int endHP = _attributes.GetHealth();
		Assert.True(endHP < startHP);

		GameObject.Destroy(enemy);
	}

	[UnityTest]
	public IEnumerator JumpFromSpring()
	{
		// ========================
		// Sets the player on top of a spring, should shoot him in the air
		// ========================

		float startY = _player.transform.position.y;
		_platform.transform.position = Vector3.zero;

		GameObject spring = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PowerUps/Spring"));

		spring.transform.position = _platform.transform.position;
		spring.transform.position += new Vector3(0, _platform.GetComponent<SpriteRenderer>().bounds.size.y, 0);

		_player.transform.position = spring.transform.position + new Vector3(0, spring.GetComponent<SpriteRenderer>().bounds.size.y, 0);

		yield return new WaitForSeconds(1);

		float endY = _player.transform.position.y;
		Assert.True(startY < endY);

		GameObject.Destroy(spring);

		_player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
	}
}
