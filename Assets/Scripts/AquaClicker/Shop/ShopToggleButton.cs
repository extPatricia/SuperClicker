using UnityEngine;
using System;
using UnityEngine.UI;

public class ShopToggleButton : MonoBehaviour
{
	#region Properties
	#endregion

	#region Fields
	[SerializeField] private GameObject _shopPanel;
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
	public void ToggleShop()
	{
		_shopPanel.SetActive(!_shopPanel.activeSelf);
	}
	#endregion

	#region Private Methods
	#endregion
   
}
