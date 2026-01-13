using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class PoolSystem : MonoBehaviour
{
	#region Properties
	#endregion

	#region Fields
	[SerializeField] private PointsUI _pointsPrefab;
	[SerializeField] private Queue<PointsUI> _pooledObjects = new Queue<PointsUI>();
	#endregion

	#region Unity Callbacks
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	#endregion

	#region Public Methods
	public PointsUI GetPooledObject(Transform transform)
	{
		if (_pooledObjects.Count > 0)
		{
			return _pooledObjects.Dequeue();
		}
		else
		{
			PointsUI newObj = Instantiate(_pointsPrefab, transform);
			return newObj;
		}
	}

	public void AddToPool(PointsUI pointsObject, float duration)
	{
		StartCoroutine(WaitAndAddToPool(pointsObject, duration));		
	}


	#endregion

	#region Private Methods
	private IEnumerator WaitAndAddToPool(PointsUI pointsObject, float duration)
	{
		yield return new WaitForSeconds(duration);
		pointsObject.gameObject.SetActive(false);
		_pooledObjects.Enqueue(pointsObject);
	}
	#endregion

}
