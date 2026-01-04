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
		if (multiplier <= 0)
		{
			gameObject.SetActive(false);
			return;
		}

		gameObject.SetActive(true);
		_multiplierText.text = "x4\n" + Mathf.CeilToInt(duration) + "s";

		if (duration <= 0)
		{
			gameObject.SetActive(false);
		}
	}
	#endregion

}
