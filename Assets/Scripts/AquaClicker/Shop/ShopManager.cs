using UnityEngine;
using System;

public class ShopManager : MonoBehaviour
{    
    #region Properties
	public static ShopManager Instance { get; set; }

	public static Action<ShopItemData> OnItemPurchased;
	#endregion

	#region Fields
	#endregion

	#region Unity Callbacks
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	#endregion

	#region Public Methods
	public int GetItemCost(ShopItemData itemData)
	{
		return Mathf.RoundToInt(itemData.BaseCost * Mathf.Pow(itemData.CostMultiplier, itemData.PurchasedCount));
	}

	public bool PurchaseItem(ShopItemData itemData)
	{
		int cost = GetItemCost(itemData);

		if (AquaController.Instance.TotalClicks < cost)
			return false;

		AquaController.Instance.DeductClicks(cost);
		itemData.PurchasedCount++;

		OnItemPurchased?.Invoke(itemData);
		
		AquaController.Instance.ApplyReward(itemData);
		return true;

	}
	#endregion

	#region Private Methods
	#endregion

}
