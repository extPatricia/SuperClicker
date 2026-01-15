using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class SpecialFisherSpawner : MonoBehaviour
{
	#region Properties
	public static SpecialFisherSpawner Instance;
	#endregion

	#region Fields
	[SerializeField] private Transform _spawner;
	[SerializeField] private SpecialFish[] _specialFishPrefab;
	private Dictionary<SpecialFishType, int> _spawnedCounts = new();

	private float _spawnTimer = 60f;
	private float _lastSpawnTime;
	private bool _specialFishOnScreen = false;
	#endregion

	#region Unity Callbacks
	private void Awake()
	{
		Instance = this;

		// Initialize spawned counts
		foreach (var fish in _specialFishPrefab)
		{
			if (!_spawnedCounts.ContainsKey(fish.FishType))
				_spawnedCounts.Add(fish.FishType, 0);
		}
	}
	// Start is called before the first frame update
	void Start()
    {
    }

	// Update is called once per frame
	void Update()
    {
		TrySpawnFish();
    }

	#endregion

	#region Public Methods
	public void SaveData()
	{
		foreach (var fish in _spawnedCounts)
		{
			PlayerPrefs.SetInt($"SPECIAL_FISH_{fish.Key}_SPAWNED_COUNT", fish.Value);
		}
	}

	public void LoadData()
	{
		foreach (var fish in _specialFishPrefab)
		{
			int count = PlayerPrefs.GetInt($"SPECIAL_FISH_{fish.FishType}_SPAWNED_COUNT", 0);
			_spawnedCounts[fish.FishType] = count;
		}
	}
	#endregion

	#region Private Methods
	private void OnEnable()
	{
		SpecialFish.OnAnySpecialFishDestroyed += OnSpecialFishDestroyed;
		SpecialFish.OnSpecialFishCollected += OnSpecialFishCollected;
	}

	private void OnDisable()
	{
		SpecialFish.OnAnySpecialFishDestroyed -= OnSpecialFishDestroyed;
		SpecialFish.OnSpecialFishCollected -= OnSpecialFishCollected;
	}

	private void OnSpecialFishDestroyed()
	{
		_specialFishOnScreen = false;
	}

	private void OnSpecialFishCollected(SpecialFishType fishType)
	{
		if (_spawnedCounts.ContainsKey(fishType))
		{
			_spawnedCounts[fishType]++;
		}
	}

	private void TrySpawnFish()
	{
		// This method can be used to implement a general spawning logic if needed
		if (_specialFishOnScreen) 
			return;
		if (AquaController.Instance.IsAnySpecialFishActive) 
			return;
		if (Time.time - _lastSpawnTime < _spawnTimer)
			return;

		foreach (SpecialFish fish in _specialFishPrefab)
		{
			int spawned = _spawnedCounts[fish.FishType];

			if (fish.CanSpawn(AquaController.Instance.TotalClicks, spawned))
			{
				Debug.Log("Spawning special fish: " + fish.FishType);
				SpawnFish(fish);
				break;
			}
		}
	}

	private void SpawnFish(SpecialFish fish)
	{
		SpecialFish fishSpawn = Instantiate(fish, _spawner.transform, false);
		_lastSpawnTime = Time.time;
		SpawnPosition(fishSpawn);
		_specialFishOnScreen = true;
	}

	private void SpawnPosition(SpecialFish fishSpawn)
	{
		// Obtain RectTransform
		RectTransform fishRect = fishSpawn.GetComponent<RectTransform>();
		RectTransform spawnerRect = _spawner.GetComponent<RectTransform>();

		// Random position inside spawner
		float randomX = Random.Range(-(spawnerRect.rect.width / 2), spawnerRect.rect.width / 2);
		fishRect.anchoredPosition3D = new Vector3(randomX, 0, 0);
	}
	#endregion

}
