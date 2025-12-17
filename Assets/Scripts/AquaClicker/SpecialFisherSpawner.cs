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
	#endregion

	#region Unity Callbacks
	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine(SpawnFishes());
    }


	// Update is called once per frame
	void Update()
    {
        
    }
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	IEnumerator SpawnFishes()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(3f, 6f));

			int fishIndex = Random.Range(0, _specialFishPrefab.Length);
			SpecialFish fishSpawn = Instantiate(_specialFishPrefab[fishIndex], _spawner.transform, false);

			// Obtain RectTransform
			RectTransform fishRect = fishSpawn.GetComponent<RectTransform>();
			RectTransform spawnerRect = _spawner.GetComponent<RectTransform>();

			// Random position inside spawner
			float randomX = Random.Range(-(spawnerRect.rect.width / 2), spawnerRect.rect.width / 2);
			fishRect.anchoredPosition3D = new Vector3(randomX, 0, 0);

		}
	}
	#endregion

}
