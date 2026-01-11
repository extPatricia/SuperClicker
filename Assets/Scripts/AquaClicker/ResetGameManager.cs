using UnityEngine;
using System;
using System.Linq;

public class ResetGameManager : MonoBehaviour
{    
    #region Properties
	#endregion

	#region Fields
	private IResettable[] _resettables;
	#endregion

	#region Unity Callbacks
	private void Awake()
	{
		_resettables = FindObjectsOfType<MonoBehaviour>(true).OfType<IResettable>().ToArray();
	}
	#endregion

	#region Public Methods
	public void ResetGame()
	{
		foreach (var resettable in _resettables)
		{
			resettable.ResetData();
		}

		Debug.Log("Game has been reset.");
		PlayerPrefs.DeleteAll();
	}
	#endregion

	#region Private Methods
	#endregion

}
