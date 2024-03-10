using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : Singleton<Game>
{
	[SerializeField]
	private GameObject _spawnPoint;

	[SerializeField]
	private GameObject _playerPrefab;

	private Action<GameObject> _onPlayerCreatedAction;

	Game()
	{
	}

	public void OnPlayerDead(GameObject player)
	{
		PlayerAttributes playerAttributes = player.GetComponent<PlayerAttributes>();

		int health = playerAttributes.GetHealth();
		playerAttributes.SetAttribute(AttributeType.Health, health - 1);

		if (health <= 1)
		{
			Debug.LogFormat("{0} died", player.name);

			PlayerController pc = player.GetComponent<PlayerController>();
			pc.ChangePlayerState(PlayerState.Dead);

			Rigidbody2D rb = pc.GetComponent<Rigidbody2D>();
			rb.velocity = Vector2.zero;
			rb.AddForce(Vector2.up * 50);

			// Show game over screen
			return;
		}

		Debug.LogFormat("{0} respawned", player.name);
		player.transform.position = _spawnPoint.transform.position;
	}

	// Start is called before the first frame update
	void Start()
	{
		CreatePlayer();
	}

	public void RegisterForPlayerCreated(Action<GameObject> func)
	{
		_onPlayerCreatedAction += func;
	}

	public void UnregisterForPlayerCreated(Action<GameObject> func)
	{
		_onPlayerCreatedAction -= func;
	}

	private void CreatePlayer()
	{
		// TODO: Multiplayer?
		GameObject player = Instantiate(_playerPrefab, _spawnPoint.transform, true);

		Debug.LogFormat("{0} spawned", player.name);

		_onPlayerCreatedAction?.Invoke(player);
	}
}
