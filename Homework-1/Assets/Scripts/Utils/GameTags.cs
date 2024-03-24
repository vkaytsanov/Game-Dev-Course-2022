using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTags : MonoBehaviour
{
	public static readonly string PlayerTag = "Player";
	public static readonly string PlatformTag = "Platform";
	public static readonly string EnemyTag = "Enemy";

	public static bool IsPlayer(GameObject obj)
	{
		return obj.CompareTag(PlayerTag);
	}

	public static bool IsPlayer(Collider2D collider)
	{
		return IsPlayer(collider.gameObject);
	}
	public static bool IsPlayer(Collision2D collider)
	{
		return IsPlayer(collider.gameObject);
	}

	public static bool IsPlatform(GameObject obj)
	{
		return obj.CompareTag(PlatformTag);
	}

	public static bool IsPlatform(Collider2D collider)
	{
		return IsPlatform(collider.gameObject);
	}
	public static bool IsPlatform(Collision2D collider)
	{
		return IsPlatform(collider.gameObject);
	}
}
