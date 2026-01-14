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
		TrySpawnFish();
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
