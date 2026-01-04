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
    }

	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	private void OnEnable()
	{
		SpecialFish.OnAnySpecialFishDestroyed += OnSpecialFishDestroyed;
	}

	private void OnDisable()
	{
		SpecialFish.OnAnySpecialFishDestroyed -= OnSpecialFishDestroyed;
	}

	private void OnSpecialFishDestroyed()
	{
		_specialFishOnScreen = false;
	}
	private void TryBonusFish()
	{
		if (_specialFishOnScreen)
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

		_bonusSpawnedFish++;
		_lastBonusSpawnTime = Time.time;
		_specialFishOnScreen = true;

	}
	private void TryMultiplierFish()
	{
		if (AquaController.Instance.IsMultiplierActive)
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

		_multiplierSpawnedFish++;
		_lastMultiplierSpawnTime = Time.time;
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
