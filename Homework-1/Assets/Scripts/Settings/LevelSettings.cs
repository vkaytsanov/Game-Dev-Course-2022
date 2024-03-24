using System;
using UnityEngine;
using Utils.Attributes;

namespace Settings
{
	[Serializable]
	public class SpawnablePlatform
	{
		public GameObject _prefab;

		[Range(0, 100)]
		public int _spawnRange;
	}

	[Serializable]
	public class SpawnableEntity
	{
		public GameObject _prefab;

		[Range(0, 1)]
		public float _spawnChance;
	}

	[CreateAssetMenu]
	public class LevelSettings : UpdatableSettings
	{
		[NamedArrayAttribute(typeof(PlatformType))]
		public SpawnablePlatform[] _platforms = new SpawnablePlatform[(int)PlatformType.Count];

		public SpawnablePlatform _key;

		public SpawnablePlatform _heart;

		public SpawnableEntity[] _enemies;

		public SpawnableEntity _springPowerup;

		public GameObject _winCondition;

		public SpawnablePlatform GetPlatform(PlatformType type)
		{
			return _platforms[(int)type];
		}

		public SpawnablePlatform GetPlatform(int type)
		{
			return _platforms[type];
		}
	}
}