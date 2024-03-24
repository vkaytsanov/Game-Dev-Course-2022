using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : Singleton<GameUIManager>
{
	[SerializeField]
	private SceneAsset _playScene;

	[SerializeField]
	private GameObject _winMenu;

	[SerializeField]
	private GameObject _loseMenu;

	public void OnLevelWon()
	{
		_winMenu.SetActive(true);
	}

	public void OnLevelLost()
	{
		_loseMenu.SetActive(true);
	}

	public void OnNextLevelClick()
	{
		Game.Instance.CreateLevel();

		GameObject player = FindObjectOfType<PlayerController>().gameObject;
		Game.Instance.RespawnPlayer(player);

		player.GetComponent<PlayerAttributes>().SetAttribute(AttributeType.Keys, 0);

		HideMenus();
	}

	public void OnRestartClick()
	{
		SceneManager.LoadScene(_playScene.name);
	}

	public void OnExitClick()
	{
		Application.Quit();
	}

	private void HideMenus()
	{
		_winMenu.SetActive(false);
		_loseMenu.SetActive(false);
	}
}
