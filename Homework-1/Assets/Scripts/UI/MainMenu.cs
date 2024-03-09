using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField]
	private SceneAsset _playScene;

	public void OnPlayClick()
	{
		SceneManager.LoadScene(_playScene.name);
	}

	public void OnOptionsClick()
	{
		// TODO:
		Debug.Log(name + " OnOptionsClick!");
	}

	public void OnExitClick() 
	{
		Debug.Log("Request Quit");
		Application.Quit();
	}
}
