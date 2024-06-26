using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : Singleton<GameUIManager>
{
	[SerializeField]
	private string _playSceneName;

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

	public void OnRestartClick()
	{
		SceneManager.LoadScene(_playSceneName);
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
