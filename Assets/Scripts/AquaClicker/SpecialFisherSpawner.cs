using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine.Rendering;

public class SpecialFisherSpawner : MonoBehaviour
{
	#region Properties
	#endregion

	#region Fields
	[SerializeField] private Transform _spawner;
	[SerializeField] private SpecialFish[] _specialFishPrefab;

	//[SerializeField] private int _bonusMinClicks = 2000;
	//[SerializeField] private int _bonusMaxSpawns = 3;
	[SerializeField] private float _duration = 1f;

	private int _bonusSpawnedFish;
	private float _lastBonusSpawnTime;

	//[SerializeField] private int _multiplierMinClicks = 50000;
	//[SerializeField] private int _multiplierMaxSpawns = 4;

	private int _multiplierSpawnedFish;
	private float _lastMultiplierSpawnTime;

	//[SerializeField] private int _autoAgentMinClicks = 250000;
	//[SerializeField] private int _autoAgentMaxSpawns = 3;

	private int _autoAgentSpawnedFish;
	private float _lastAutoAgentSpawnTime;

	private bool _specialFishOnScreen = false;
	#endregion

	#region Unity Callbacks
	// Start is called before the first frame update
	void Start()
    {
    }

	// Update is called once per frame
	void Update()
    {
		//TryBonusFish();
		//TryMultiplierFish();
		//TryAutoAgentFish();
		TrySpawnFish();
    }

	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	private void OnEnable()
	{
		SpecialFish.OnAnySpecialFishDestroyed += OnSpecialFishDestroyed;
		//SpecialFish.OnSpecialFishCollected += OnRegisterCollectedFish;
	}

	private void OnDisable()
	{
		SpecialFish.OnAnySpecialFishDestroyed -= OnSpecialFishDestroyed;
		//SpecialFish.OnSpecialFishCollected -= OnRegisterCollectedFish;
	}

	private void OnSpecialFishDestroyed()
	{
		_specialFishOnScreen = false;
	}

	private void OnRegisterCollectedFish(SpecialFishType fishType)
	{
		switch (fishType)
		{
			case SpecialFishType.Bonus:
				_bonusSpawnedFish++;
				Debug.Log($"Bonus Fish Collected. Total: {_bonusSpawnedFish}");
				break;
			case SpecialFishType.Multiplier:
				_multiplierSpawnedFish++;
				Debug.Log($"Multiplier Fish Collected. Total: {_multiplierSpawnedFish}");
				break;
			case SpecialFishType.AutoAgent:
				_autoAgentSpawnedFish++;
				Debug.Log($"Auto Agent Fish Collected. Total: {_autoAgentSpawnedFish}");
				break;
			default:
				break;
		}
	}

	private void TrySpawnFish()
	{
		// This method can be used to implement a general spawning logic if needed
		if (_specialFishOnScreen) 
			return;
		if (AquaController.Instance.IsAnySpecialFishActive) 
			return;
		foreach (SpecialFish fish in _specialFishPrefab)
		{
			if (fish.CanSpawn(AquaController.Instance.TotalClicks))
			{
				SpawnFish(fish);
				break;
			}
		}
	}

	private void SpawnFish(SpecialFish fish)
	{
		SpecialFish fishSpawn = Instantiate(fish, _spawner.transform, false);
		SpawnPosition(fishSpawn);
		fish.SpawnedCount++;
		_specialFishOnScreen = true;
	}
	//private void TryBonusFish()
	//{
	//	if (_specialFishOnScreen)
	//		return;
	//	if (AquaController.Instance.IsAnySpecialFishActive)
	//		return;

	//	if (AquaController.Instance.TotalClicks < _bonusMinClicks)
	//		return;
	//	if (_bonusSpawnedFish >= _bonusMaxSpawns)
	//		return;
	//	if (Time.time - _lastBonusSpawnTime < _duration)
	//		return;

	//	SpawnBonusFish();
	//}

	//private void SpawnBonusFish()
	//{
	//	SpecialFish fishSpawn = Instantiate(_specialFishPrefab[0], _spawner.transform, false);
	//	SpawnPosition(fishSpawn);

	//	_specialFishOnScreen = true;
	//	_lastBonusSpawnTime = Time.time;
	//}
	//private void TryMultiplierFish()
	//{
	//	if (_specialFishOnScreen)
	//		return;
	//	if (AquaController.Instance.IsAnySpecialFishActive)
	//		return;
	//	if (AquaController.Instance.TotalClicks < _multiplierMinClicks)
	//		return;
	//	if (_multiplierSpawnedFish >= _multiplierMaxSpawns)
	//		return;
	//	if (Time.time - _lastMultiplierSpawnTime < _duration)
	//		return;

	//	SpawnMultiplierFish();
	//}

	//private void SpawnMultiplierFish()
	//{
	//	SpecialFish fishSpawn = Instantiate(_specialFishPrefab[1], _spawner.transform, false);
	//	SpawnPosition(fishSpawn);

	//	_specialFishOnScreen = true;
	//	_lastMultiplierSpawnTime = Time.time;
	//}

	//private void TryAutoAgentFish()
	//{
	//	if (_specialFishOnScreen)
	//		return;
	//	if (AquaController.Instance.IsAnySpecialFishActive)
	//		return;
	//	if (AquaController.Instance.TotalClicks < _autoAgentMinClicks)
	//		return;
	//	if (_autoAgentSpawnedFish >= _autoAgentMaxSpawns)
	//		return;
	//	if (Time.time - _lastAutoAgentSpawnTime < _duration)
	//		return;

	//	SpawnAutoAgentFish();
	//}

	//private void SpawnAutoAgentFish()
	//{
	//	Debug.Log("Spawning Auto Agent Fish");
	//	SpecialFish fishSpawn = Instantiate(_specialFishPrefab[2], _spawner.transform, false);
	//	SpawnPosition(fishSpawn);

	//	_specialFishOnScreen = true;
	//	_lastAutoAgentSpawnTime = Time.time;
	//}

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
