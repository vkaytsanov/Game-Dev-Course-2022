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
		Debug.LogFormat("{0} died", player.name);
	}

	public void RespawnPlayer(GameObject player)
	{
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
		GameObject player = Instantiate(_playerPrefab);
		player.transform.position = _spawnPoint.transform.position;

		player.GetComponent<PlayerController>().RegisterForPlayerDead(OnPlayerDead);

		Debug.LogFormat("{0} spawned", player.name);

		_onPlayerCreatedAction?.Invoke(player);
	}
}
