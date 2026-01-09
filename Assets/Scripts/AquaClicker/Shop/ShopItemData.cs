using UnityEngine;
using System;

public enum ShopRewardType
{
	Clicker,
	Multiplier,
	AutoAgent
}

[CreateAssetMenu(menuName = "AquaClicker/ShopItemData")]
public class ShopItemData : ScriptableObject
{    
    #region Properties
	public string ItemName;
	public ShopRewardType RewardType;

	public int BaseCost;
	public float CostMultiplier = 1.15f;

	public int Clicker;
	public float Multiplier;
	public float AutoAgent;

	[HideInInspector] public int PurchasedCount;
	#endregion

	#region Fields
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
	#endregion
   
}
