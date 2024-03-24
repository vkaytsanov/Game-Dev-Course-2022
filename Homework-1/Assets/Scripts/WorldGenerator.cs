using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using Settings;
using UnityEngine;


public enum PlatformType
{
	Default,
	HorizontalMovement,
	VerticalMovement,
	Droppable,
	Count
}

public class WorldGenerator
{
	private Settings.LevelSettings _settings;

	private Vector3 _spawnPosition;

	private PlatformType _prevPlatformType = PlatformType.Default;
	private List<GameObject> _spawnedPlatforms;
	private List<bool> _hasPropPlatforms;
	private bool _previouslySpawnedSpring = false;
	private int[] _spawnCounts;


	public WorldGenerator(Settings.LevelSettings settings, Vector3 spawnPosition)
	{
		_settings = settings;
		_spawnPosition = spawnPosition;

		_spawnedPlatforms = new List<GameObject>();
		_hasPropPlatforms = new List<bool>();
		_spawnCounts = new int[(int)PlatformType.Count];
		System.Array.Clear(_spawnCounts, 0, _spawnCounts.Length);
	}

	public void Clear()
	{
		foreach (GameObject go in GameObject.FindGameObjectsWithTag(GameTags.PlatformTag))
		{
			GameObject.DestroyImmediate(go);
		}

		foreach (GameObject go in GameObject.FindGameObjectsWithTag(GameTags.EnemyTag))
		{
			GameObject.DestroyImmediate(go);
		}
	}

	public void Generate()
	{
		Clear();

		SpawnInitialPlatform();
		SpawnRandomPlatforms();
		SpawnFinalPlatform();

		SpawnEntitiesOnPlatformsBasedOnCount(_settings._key._prefab, _settings._key._spawnRange);
		SpawnEntitiesOnPlatformsBasedOnCount(_settings._heart._prefab, _settings._heart._spawnRange);
	}

	private void SpawnInitialPlatform()
	{
		GameObject platform = CreateNewPlatform(PlatformType.Default);

		Bounds platformBounds = platform.GetComponent<Collider2D>().bounds;
		platform.transform.position = _spawnPosition - new Vector3(0, platformBounds.size.y, 0);

		_hasPropPlatforms.Add(true);
	}

	private void SpawnFinalPlatform()
	{
		GameObject platform = CreateNewPlatform(PlatformType.Default);
		Vector3 spawnOffset = GetSpawnOffsetOfPlatform(PlatformType.Default);
		platform.transform.position = _spawnedPlatforms[_spawnedPlatforms.Count - 2].transform.position + spawnOffset;

		_hasPropPlatforms.Add(true);

		SpawnOnPlatform(platform, _settings._winCondition);
	}

	private void SpawnRandomPlatforms()
	{
		while (true)
		{
			PlatformType toSpawnType = GetPlatformToSpawn();
			if (toSpawnType == PlatformType.Count)
			{
				break;
			}

			GameObject platform = CreateNewPlatform(toSpawnType);
			Vector3 spawnOffset = GetSpawnOffsetOfPlatform(toSpawnType);

			platform.transform.position = _spawnedPlatforms[_spawnedPlatforms.Count - 2].transform.position + spawnOffset;

			if (_prevPlatformType == PlatformType.VerticalMovement && toSpawnType == PlatformType.VerticalMovement)
			{
				PlatformMovable currentMovable = _spawnedPlatforms[_spawnedPlatforms.Count - 1].GetComponent<PlatformMovable>();
				PlatformMovable prevMovable = _spawnedPlatforms[_spawnedPlatforms.Count - 2].GetComponent<PlatformMovable>();

				if (currentMovable.IsMovingDown() && prevMovable.IsMovingDown())
				{
					currentMovable.MoveUp();
				}
			}

			SpawnableEntity enemy = _settings._enemies[Random.Range(0, _settings._enemies.Length)];
			SpawnableEntity spring = _settings._springPowerup;

			_previouslySpawnedSpring = false;

			bool shouldSpawnEnemy = enemy._spawnChance > Random.Range(0.0f, 1.0f);
			bool shouldSpawnSpring = spring._spawnChance > Random.Range(0.0f, 1.0f);

			if (toSpawnType != PlatformType.Droppable)
			{
				if (shouldSpawnEnemy && toSpawnType == PlatformType.Default)
				{
					SpawnOnPlatform(platform, enemy._prefab);
				}
				else if (shouldSpawnSpring)
				{
					_previouslySpawnedSpring = true;
					SpawnOnPlatform(platform, spring._prefab);
				}
			}

			_hasPropPlatforms.Add(_previouslySpawnedSpring);
			_spawnCounts[(int)toSpawnType]++;
			_prevPlatformType = toSpawnType;
		}
	}

	private Vector3 GetSpawnOffsetOfPlatform(PlatformType currentType)
	{
		Bounds platformBounds = _spawnedPlatforms[_spawnedPlatforms.Count - 2].GetComponent<Collider2D>().bounds;

		bool chooseRightSide = Random.Range(0, 2) == 0;
		float minSpawnX = chooseRightSide ? 3.0f : -3.0f;
		float maxSpawnX = chooseRightSide ? -5.0f : -3.0f;

		if (_prevPlatformType == PlatformType.HorizontalMovement)
		{
			PlatformMovable movable = _spawnedPlatforms[_spawnedPlatforms.Count - 2].GetComponent<PlatformMovable>();
			if (chooseRightSide)
			{
				minSpawnX = movable.GetRightWorldBoundary() + platformBounds.size.x * 0.5f;
			}
			else
			{
				minSpawnX = movable.GetLeftWorldBoundary() - platformBounds.size.x * 0.5f;
			}

			maxSpawnX = minSpawnX + 3.0f;
		}

		float minSpawnY = 3.0f;
		float maxSpawnY = 4.0f;
		
		if (_prevPlatformType == PlatformType.VerticalMovement)
		{
			PlatformMovable movable = _spawnedPlatforms[_spawnedPlatforms.Count - 2].GetComponent<PlatformMovable>();
			minSpawnY = movable.GetUpWorldBoundary() + platformBounds.size.y * 2;
			maxSpawnY = minSpawnY + 2.0f;
		}

		if (_previouslySpawnedSpring)
		{
			maxSpawnY += 6.0f;
		}

		if (currentType == PlatformType.VerticalMovement)
		{
			PlatformMovable movable = _spawnedPlatforms[_spawnedPlatforms.Count - 1].GetComponent<PlatformMovable>();
			minSpawnY = Mathf.Max(minSpawnY, movable.GetDownBoundary()) + platformBounds.size.y * 2;
		}

		float spawnX = Random.Range(minSpawnX, maxSpawnX);
		float spawnY = Random.Range(minSpawnY, maxSpawnY);
		return new Vector3(spawnX, spawnY, 0);
	}

	private GameObject CreateNewPlatform(PlatformType type)
	{
		GameObject platform = GameObject.Instantiate(_settings.GetPlatform(type)._prefab);
		_spawnedPlatforms.Add(platform);
		return platform;
	}

	private PlatformType GetPlatformToSpawn()
	{
		int retries = 0;
		do
		{
			int toSpawnType = Random.Range(0, (int)PlatformType.Count);
			SpawnablePlatform spawnablePlatform = _settings.GetPlatform(toSpawnType);

			if (spawnablePlatform._spawnRange - _spawnCounts[toSpawnType] > 0)
			{
				return (PlatformType)toSpawnType;
			}

			retries++;
		}
		while (retries < 10);

		return PlatformType.Count;
	}

	private void SpawnEntitiesOnPlatformsBasedOnCount(GameObject entity, int count)
	{
		for (int i = 0; i < count; i++)
		{
			// without first and final platforms
			int retries = 0;
			int randomIndex;
			do
			{
				randomIndex = Random.Range(1, _spawnedPlatforms.Count - 1);
				retries++;

				if (retries > 10)
				{
					return;
				}
			}
			while (_hasPropPlatforms[randomIndex]);

			_hasPropPlatforms[randomIndex] = true;
			SpawnOnPlatform(_spawnedPlatforms[randomIndex], entity);
		}
	}

	private void SpawnOnPlatform(GameObject platform, GameObject toSpawn)
	{
		Bounds platformBounds = platform.GetComponent<Collider2D>().bounds;
		Bounds toSpawnBounds = toSpawn.GetComponent<SpriteRenderer>().bounds;

		float kCenterOffsetX = Random.Range(-platformBounds.size.x * 0.35f, platformBounds.size.x * 0.35f);

		GameObject spawned = GameObject.Instantiate(toSpawn, platform.transform);
		spawned.transform.position = platform.transform.position + new Vector3(kCenterOffsetX, (platformBounds.size.y + toSpawnBounds.size.y) * 0.5f, 0);
	}
}
