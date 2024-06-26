using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;


enum MapWarpFlags
{
	None = 0,
	HasSetupToAdvance = 1,
	SegmentLeavesWarp = 2,
	SegmentEntersFullyWarp = 4,
}

class MapWarp
{
	public GameObject Parent;
	public GameObject Clone;
	public List<GameObject> Tiles;
	public List<GameObject> NoticableObjects;

	public int TileCheckpoint;
	public int DirectionToAdvanceWarp;
	public int TileToAdvanceWarp;

	public int PrevWarpIndex;
	public int NextWarpIndex;

	public MapWarpFlags Flags;

	public MapWarp()
	{
		Tiles = new List<GameObject>();
		NoticableObjects = new List<GameObject>();

		TileCheckpoint = int.MinValue;
		DirectionToAdvanceWarp = 0;
		TileToAdvanceWarp = int.MinValue;

		PrevWarpIndex = int.MinValue;
		NextWarpIndex = int.MinValue;

		Flags = MapWarpFlags.None;
	}

	public bool HasPrevious()
	{
		return PrevWarpIndex != int.MinValue;
	}
};

public class MapController : MonoBehaviour
{
	[SerializeField]
	private GameObject[] _tiles;

	[SerializeField]
	private GameObject[] _noticableObjects;

	[SerializeField]
	private float _groundPosition = -4.0f;

	[SerializeField]
	private float _tilePixelSize = 16.0f;

	[SerializeField]
	private GameObject _winObjective;

	[SerializeField]
	private Transform _playerTransform; // todo remove

	private PixelPerfectCamera _camera;

	private List<MapWarp> _warps;

	private int _currentWarpIndex;

	private int _tilesToRenderPerWarp;

	private int _currentWarpPosX;
	private int _currentPosWorldX;

	private const int kWarpSize = 32;


	// Start is called before the first frame update
	void Start()
	{
		_camera = Camera.main.GetComponent<PixelPerfectCamera>();
		Debug.Assert(_camera);

		_tilesToRenderPerWarp = Mathf.CeilToInt(_camera.refResolutionX / _tilePixelSize);

		_warps = new List<MapWarp>();
		_currentWarpIndex = 0;
		_currentPosWorldX = _currentWarpPosX = Mathf.FloorToInt(_playerTransform.position.x);

		GenerateWarp();

		_warps[_currentWarpIndex].Parent.SetActive(true);
		_warps[_currentWarpIndex].Clone.SetActive(true);

		UpdateBufferWarp(0, 0, 1);
	}

	// Update is called once per frame
	void Update()
	{
		int playerAbsoluteX = Mathf.RoundToInt(_playerTransform.position.x);
		int updatedWarpPosX = playerAbsoluteX % kWarpSize;
		if (updatedWarpPosX < 0)
		{
			updatedWarpPosX += kWarpSize;
		}
		if (_currentWarpPosX != updatedWarpPosX)
		{
			int moveDirection = updatedWarpPosX - _currentWarpPosX;
			_currentWarpPosX = updatedWarpPosX;
			_currentPosWorldX = playerAbsoluteX;
			OnTileChanged(updatedWarpPosX, playerAbsoluteX, moveDirection);
		}
	}

	void GenerateWarp(int prevWarpIndex = int.MinValue)
	{
		int warpIndex = _warps.Count;

		MapWarp warp = new MapWarp();
		warp.Parent = new GameObject("Warp " + warpIndex);
		warp.Parent.SetActive(false);
		warp.PrevWarpIndex = prevWarpIndex;
		if (prevWarpIndex != int.MinValue)
		{
			_warps[prevWarpIndex].NextWarpIndex = warpIndex;
		}

		var chosenTile = _tiles[Math.Min(warpIndex, _tiles.Length - 1)];
		for (var i = 0; i < kWarpSize; i++)
		{
			Vector3 spawnPosition = new Vector3(i - kWarpSize / 2, _groundPosition, 0.0f);
			var go = Instantiate(chosenTile, _camera.RoundToPixel(spawnPosition), Quaternion.identity);
			go.transform.parent = warp.Parent.transform;
			warp.Tiles.Add(go);
		}

		var chosenObject = warpIndex < _noticableObjects.Length ? _noticableObjects[warpIndex] : _winObjective;
		{
			Vector3 spawnPosition = new Vector3(1, _groundPosition + 1, 0.0f);
			var go = Instantiate(chosenObject, _camera.RoundToPixel(spawnPosition), Quaternion.identity);
			go.transform.parent = warp.Parent.transform;
			warp.NoticableObjects.Add(go);
		}

		warp.TileCheckpoint = warpIndex % 2 != 0 ? 2 : kWarpSize - 2;

		warp.Clone = Instantiate(warp.Parent);
		warp.Clone.SetActive(false);
		_warps.Add(warp);

		Debug.LogFormat("Generating Warp {0}: C: {1}", warpIndex, warp.TileCheckpoint);
	}

	void OnTileChanged(int newTile, int worldTile, int moveDirection)
	{
		Debug.LogFormat("Tile changed to Warp: {0}, R:{1}, W:{2}", _currentWarpIndex, newTile, worldTile);

		CheckGenerateWarp(newTile, worldTile, moveDirection);
		UpdateBufferWarp(newTile, worldTile, moveDirection);
	}

	void CheckGenerateWarp(int relativeTile, int worldTile, int moveDirection)
	{
		MapWarp currentWarp = _warps[_currentWarpIndex];

		bool bIsOnCurrentAdvanceTile = currentWarp.TileCheckpoint == relativeTile;
		//bool bHasPassedCurrentAdvanceTile = !bIsOnCurrentAdvanceTile && currentWarp.TileToAdvanceWarp == (newTile + moveDirection * -1);

		// Check generation
		if (bIsOnCurrentAdvanceTile)
		{
			if (!currentWarp.Flags.HasFlag(MapWarpFlags.HasSetupToAdvance))
			{
				currentWarp.Flags |= MapWarpFlags.HasSetupToAdvance;
				// Opposite direction exits the warp
				currentWarp.DirectionToAdvanceWarp = moveDirection * -1;
				currentWarp.TileToAdvanceWarp = worldTile + currentWarp.DirectionToAdvanceWarp * (kWarpSize / 4);
				GenerateWarp(_currentWarpIndex);
			}
		}

		if (currentWarp.Flags.HasFlag(MapWarpFlags.HasSetupToAdvance) && worldTile == currentWarp.TileToAdvanceWarp)
		{
			Debug.Log("Passed current warp advance point " + worldTile);
			if (currentWarp.DirectionToAdvanceWarp == moveDirection)
			{
				if (!currentWarp.Flags.HasFlag(MapWarpFlags.SegmentLeavesWarp))
				{
					currentWarp.Flags |= MapWarpFlags.SegmentLeavesWarp;
				}
			}
		}
	}

	void UpdateBufferWarp(int newTile, int worldTile, int moveDirection)
	{
		int leftWarpSegment = TileToWarp(worldTile - _tilesToRenderPerWarp);
		int rightWarpSegment = TileToWarp(worldTile + _tilesToRenderPerWarp);
		int warpSegment = TileToWarp(worldTile);
		int nextWarpSegment = warpSegment != leftWarpSegment ? leftWarpSegment : rightWarpSegment;
		//Debug.LogFormat("UpdateBufferWarp {0}({1}): L:{2} R:{3} N:{4} ", worldTile, warpSegment, leftWarpSegment, rightWarpSegment, nextWarpSegment);
		if (warpSegment != nextWarpSegment)
		{
			MapWarp currentWarp = _warps[_currentWarpIndex];
			if (currentWarp.Flags.HasFlag(MapWarpFlags.SegmentEntersFullyWarp))
			{
				MapWarp prevWarp = _warps[currentWarp.PrevWarpIndex];
				if (prevWarp.Parent.transform.position == currentWarp.Clone.transform.position)
				{
					prevWarp.Parent.SetActive(false);
					prevWarp.Clone.SetActive(false);
				}
				
			}

			if (currentWarp.Flags.HasFlag(MapWarpFlags.SegmentLeavesWarp))
			{
				currentWarp.Parent.transform.position = Vector3.right * warpSegment * kWarpSize;
				currentWarp.Clone.SetActive(false);

				MapWarp nextWarp = _warps[currentWarp.NextWarpIndex];
				nextWarp.Parent.transform.position = Vector3.right * nextWarpSegment * kWarpSize;
				nextWarp.Parent.SetActive(true);
				nextWarp.Clone.SetActive(true);
				nextWarp.Flags |= MapWarpFlags.SegmentEntersFullyWarp;
				OnWarpChanged(currentWarp.NextWarpIndex, _currentWarpIndex);
			}
			else
			{
				currentWarp.Parent.transform.position = Vector3.right * warpSegment * kWarpSize;
				currentWarp.Clone.transform.position = Vector3.right * nextWarpSegment * kWarpSize;
			}
		}
		
	}

	void OnWarpChanged(int newWarp, int oldWarp)
	{
		Debug.LogFormat("Switched warp {0} -> {1}", oldWarp, newWarp);
		_currentWarpIndex = newWarp;
	}

	private int TileToWarp(int worldTile)
	{
		return (worldTile + kWarpSize / 2 * Math.Sign(worldTile)) / kWarpSize;
	}
}
