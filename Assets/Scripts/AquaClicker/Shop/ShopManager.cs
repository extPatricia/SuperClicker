using UnityEngine;
using System;

public class ShopManager : MonoBehaviour, IResettable
{    
    #region Properties
	public static ShopManager Instance { get; set; }

	public static Action<ShopItemData> OnItemPurchased;
	#endregion

	#region Fields
	[SerializeField] private ShopItemData[] _shopItems;
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
	public void ResetData()
	{
		foreach (var item in _shopItems)
			item.PurchasedCount = 0;
	}

	public void SaveData()
	{
		for (int i = 0; i < _shopItems.Length; i++)
		{
			PlayerPrefs.SetInt($"SHOP_ITEM_{i}_PURCHASED_COUNT", _shopItems[i].PurchasedCount);
		}
	}

	public void LoadData()
	{
		for (int i = 0; i < _shopItems.Length; i++)
		{
			_shopItems[i].PurchasedCount = PlayerPrefs.GetInt($"SHOP_ITEM_{i}_PURCHASED_COUNT", 0);
		}
	}

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
