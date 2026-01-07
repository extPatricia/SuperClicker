using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

public class SpecialFisherSpawner : MonoBehaviour
{
	#region Properties
	#endregion

	#region Fields
	[SerializeField] private Transform _spawner;
	[SerializeField] private SpecialFish[] _specialFishPrefab;

	[Header("Fish Bonus +1000")]
	[SerializeField] private int _bonusMinClicks = 100;
	[SerializeField] private int _bonusMaxSpawns = 5;
	[SerializeField] private float _duration = 1f;

	private int _bonusSpawnedFish;
	private float _lastBonusSpawnTime;

	[Header("Fish Multiplier x4")]
	[SerializeField] private int _multiplierMinClicks = 1500;
	[SerializeField] private int _multiplierMaxSpawns = 5;

	private int _multiplierSpawnedFish;
	private float _lastMultiplierSpawnTime;

	[Header("Fish Auto Agent")]
	[SerializeField] private int _autoAgentMinClicks = 3000;
	[SerializeField] private int _autoAgentMaxSpawns = 3;

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
		TryBonusFish();
		TryMultiplierFish();
		TryAutoAgentFish();
    }

	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	private void OnEnable()
	{
		SpecialFish.OnAnySpecialFishDestroyed += OnSpecialFishDestroyed;
		SpecialFish.OnSpecialFishCollected += OnRegisterCollectedFish;
	}

	private void OnDisable()
	{
		SpecialFish.OnAnySpecialFishDestroyed -= OnSpecialFishDestroyed;
		SpecialFish.OnSpecialFishCollected -= OnRegisterCollectedFish;
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
				break;
			case SpecialFishType.Multiplier:
				_multiplierSpawnedFish++;
				break;
			case SpecialFishType.AutoAgent:
				_autoAgentSpawnedFish++;
				break;
			default:
				break;
		}
	}
	private void TryBonusFish()
	{
		if (_specialFishOnScreen)
			return;
		if (AquaController.Instance.IsAnySpecialFishActive)
			return;
		if (AquaController.Instance.TotalClicks < _bonusMinClicks)
			return;
		if (_bonusSpawnedFish >= _bonusMaxSpawns)
			return;
		if (Time.time - _lastBonusSpawnTime < _duration)
			return;

		SpawnBonusFish();
	}

	private void SpawnBonusFish()
	{
		SpecialFish fishSpawn = Instantiate(_specialFishPrefab[0], _spawner.transform, false);
		SpawnPosition(fishSpawn);

		_specialFishOnScreen = true;
		_lastBonusSpawnTime = Time.time;
	}
	private void TryMultiplierFish()
	{
		if (_specialFishOnScreen)
			return;
		if (AquaController.Instance.IsAnySpecialFishActive)
			return;
		if (AquaController.Instance.TotalClicks < _multiplierMinClicks)
			return;
		if (_multiplierSpawnedFish >= _multiplierMaxSpawns)
			return;
		if (Time.time - _lastMultiplierSpawnTime < _duration)
			return;

		SpawnMultiplierFish();
	}

	private void SpawnMultiplierFish()
	{
		SpecialFish fishSpawn = Instantiate(_specialFishPrefab[1], _spawner.transform, false);
		SpawnPosition(fishSpawn);

		_specialFishOnScreen = true;
		_lastMultiplierSpawnTime = Time.time;
	}

	private void TryAutoAgentFish()
	{
		if (_specialFishOnScreen)
			return;
		if (AquaController.Instance.IsAnySpecialFishActive)
			return;
		if (AquaController.Instance.TotalClicks < _autoAgentMinClicks)
			return;
		if (_autoAgentSpawnedFish >= _autoAgentMaxSpawns)
			return;
		if (Time.time - _lastAutoAgentSpawnTime < _duration)
			return;

		SpawnAutoAgentFish();
	}

	private void SpawnAutoAgentFish()
	{
		SpecialFish fishSpawn = Instantiate(_specialFishPrefab[2], _spawner.transform, false);
		SpawnPosition(fishSpawn);

		_specialFishOnScreen = true;
		_lastAutoAgentSpawnTime = Time.time;
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
