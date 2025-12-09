using UnityEngine;
using System;
using TMPro;

public class MobileController : MonoBehaviour
{
	#region Properties
	#endregion

	#region Fields
	[SerializeField] private TextMeshProUGUI _testText;
	private float _initialTime;
	#endregion

	#region Unity Callbacks
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
		{
			Application.Quit();
		}

		// Si al menos tocamos con un dedo la pantalla
		if (Input.touchCount > 0)
		{
			// Moviendo el dedo por la pantalla
			if (Input.GetTouch(0).phase == TouchPhase.Began)
				_initialTime = Time.realtimeSinceStartup;

			if (Input.GetTouch(0).phase == TouchPhase.Moved)
				_testText.text = "Moving";

			if (Input.GetTouch(0).phase == TouchPhase.Stationary)
				_testText.text = "Stationary";

			// Al levantar el dedo de la pantalla
			if (Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				float finalTime = Time.realtimeSinceStartup - _initialTime;
				_testText.text = "Touch position: " + Input.GetTouch(0).position + "\n" + finalTime;
			}
		}		
	
	}
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	#endregion
   
}
