using System;
using TMPro;
using UnityEngine;

public class AutoAgentUI : MonoBehaviour
{
	#region Properties
	#endregion

	#region Fields
	[SerializeField] private TextMeshProUGUI _autoAgentText;
	#endregion

	#region Unity Callbacks
	#endregion

	#region Public Methods
	public void Initialize(float duration)
	{
		gameObject.SetActive(true);
		 _autoAgentText.text = Mathf.CeilToInt(duration) + "s";
	}
	#endregion

	#region Private Methods
	private void OnEnable()
	{
		AquaController.OnAutoAgentChanged += UpdateAutoAgentDisplay;
	}

	private void OnDisable()
	{
		AquaController.OnAutoAgentChanged -= UpdateAutoAgentDisplay;
	}

	private void UpdateAutoAgentDisplay(float timeLeft)
	{
		if (timeLeft <= 0)
		{
			gameObject.SetActive(false);
			return;
		}

		_autoAgentText.text = Mathf.CeilToInt(timeLeft ) + "s";
	}
	#endregion

}
