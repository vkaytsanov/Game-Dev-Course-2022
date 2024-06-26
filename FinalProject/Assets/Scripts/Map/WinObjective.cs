using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinObjective : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("OnTriggerEnter2D");
		GameUIManager.Instance.OnLevelWon();
	}
}
