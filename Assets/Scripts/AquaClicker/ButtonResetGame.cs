using UnityEngine;
using System;
using UnityEngine.UI;
public class ButtonResetGame : MonoBehaviour
{
	#region Properties
	#endregion

	#region Fields
	[SerializeField] private Button _buttonYes;
	[SerializeField] private Button _buttonNo;
	[SerializeField] private ResetGameManager _resetGameManager;
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
	public void ToggleButtonYes()
	{
		Debug.Log("Game Reset!");
		FadeManager.Instance.StartFadeResetGame(() => 
		{
			_resetGameManager.ResetGame();
		});
		gameObject.SetActive(false);
	} 

	public void ToggleButtonNo()
	{
		gameObject.SetActive(false);
	}
	#endregion

	#region Private Methods
	#endregion

}
