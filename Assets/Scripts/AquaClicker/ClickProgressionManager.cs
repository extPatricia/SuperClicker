using UnityEngine;
using System;

public class ClickProgressionManager : MonoBehaviour, IResettable
{
	#region Properties
	public static ClickProgressionManager Instance;
	#endregion

	#region Fields
	[SerializeField] private int[] _clicksThresholds = { 0, 100, 500, 2000, 10000, 50000, 250000, 1500000, 10000000 };
	[SerializeField] private float[] _clicksValues = { 1f, 1.2f, 1.5f, 2f, 2.5f, 3f, 4f, 5f, 6f};

	private int _currentLevel = -1;
	#endregion

	#region Unity Callbacks
	private void Awake()
	{
		Instance = this;
	}
	#endregion

	#region Public Methods
	public void ResetData()
	{
		_currentLevel = -1;
		AquaController.Instance.ClickRatio = _clicksValues[0];
	}

	public void SaveData()
	{
		PlayerPrefs.SetInt("CLICK_PROGRESSION_LEVEL", _currentLevel);
	}

	public void LoadData()
	{
		_currentLevel = PlayerPrefs.GetInt("CLICK_PROGRESSION_LEVEL", -1);
		if (_currentLevel >= 0 && _currentLevel < _clicksValues.Length)
		{
			AquaController.Instance.ClickRatio = _clicksValues[_currentLevel];
		}
		else
		{
			_currentLevel = -1;
			AquaController.Instance.ClickRatio = _clicksValues[0];
		}
	}
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
