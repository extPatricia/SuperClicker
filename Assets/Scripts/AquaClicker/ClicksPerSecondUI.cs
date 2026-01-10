using UnityEngine;
using System;
using TMPro;

public class ClicksPerSecondUI : MonoBehaviour
{
	#region Properties
	#endregion

	#region Fields
	[SerializeField] private TextMeshProUGUI _clicksPerSecondText;
	#endregion

	#region Unity Callbacks
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	private void OnEnable()
	{
		if (AquaController.Instance == null)
			return;

		AquaController.OnClicksPerSecondChanged += UpdateClicksPerSecondDisplay;
		UpdateClicksPerSecondDisplay(AquaController.Instance.ClicksPerSecond);
	}

	private void OnDisable()
	{
		AquaController.OnClicksPerSecondChanged -= UpdateClicksPerSecondDisplay;
	}
	private void UpdateClicksPerSecondDisplay(float clicksPerSecond)
	{
		_clicksPerSecondText.text = $"{clicksPerSecond:0.0} clicks per second";
	}
	#endregion

}
