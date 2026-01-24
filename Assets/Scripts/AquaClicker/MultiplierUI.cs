using UnityEngine;
using System;
using TMPro;

public class MultiplierUI : MonoBehaviour
{    
    #region Properties
	#endregion

	#region Fields
	[SerializeField] private TextMeshProUGUI _multiplierText;
	#endregion

	#region Unity Callbacks
	#endregion

	#region Public Methods
	public void Initialize(float multiplier, float duration)
	{
		gameObject.SetActive(true);
		_multiplierText.text = "x" + multiplier + "\n" + Mathf.CeilToInt(duration) + "s";
	}
	#endregion

	#region Private Methods
	private void OnEnable()
	{
		AquaController.OnMultiplierChanged += UpdateMultiplierDisplay;
	}

	private void OnDisable()
	{
		AquaController.OnMultiplierChanged -= UpdateMultiplierDisplay;
	}

	private void UpdateMultiplierDisplay(float multiplier, float duration)
	{
		if (duration <= 0)
		{
			gameObject.SetActive(false);
			return;
		}

		_multiplierText.text = "x" + multiplier + "\n" + Mathf.CeilToInt(duration) + "s";

	}
	#endregion

}
