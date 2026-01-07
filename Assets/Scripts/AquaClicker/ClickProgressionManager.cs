using UnityEngine;
using System;

public class ClickProgressionManager : MonoBehaviour
{
	#region Properties
	#endregion

	#region Fields
	[SerializeField] private int[] _clicksThresholds = { 0, 100, 500, 1000, 5000, 10000 };
	[SerializeField] private int[] _clicksValues = { 1, 2, 5, 10, 50, 100};

	private int _currentLevel = -1;
	#endregion

	#region Unity Callbacks
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	private void OnEnable()
	{
		AquaController.OnClicksChanged += OnClickProgression;
	}

	private void OnDisable()
	{
		AquaController.OnClicksChanged -= OnClickProgression;
	}

	private void OnClickProgression(int totalClicks)
	{

		for (int i = _clicksThresholds.Length - 1; i >= 0; i--)
		{
			if (totalClicks >= _clicksThresholds[i])
			{
				
				if (_currentLevel != i)
				{
					_currentLevel = i;
					AquaController.Instance.ClickRatio = _clicksValues[i];
				}
				return;
			}
		}
	}
	#endregion

}
