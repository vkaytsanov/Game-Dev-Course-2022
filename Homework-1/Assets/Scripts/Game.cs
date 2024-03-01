using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public static Game Instance { get; private set; }

	[SerializeField]
	private GameObject _spawnPoint;

	[SerializeField]
	private GameObject _playerPrefab;

	public void OnPlayerDead(GameObject player)
	{
		Debug.LogFormat("{0} died", player.name);

		// TODO: Maybe not best to destroy the whole object?
		Destroy(player);

		CreatePlayer();
	}

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		CreatePlayer();
	}

	private void CreatePlayer()
	{
		// TODO: Multiplayer?
		GameObject player = Instantiate(_playerPrefab, _spawnPoint.transform, true);

		CameraFollowPlayer followPlayerComponent = Camera.main.GetComponent<CameraFollowPlayer>();
		followPlayerComponent.SetPlayerToFollow(player);

		Debug.LogFormat("{0} spawned", player.name);
	}
}
