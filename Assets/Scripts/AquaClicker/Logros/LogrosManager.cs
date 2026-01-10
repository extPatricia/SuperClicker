using UnityEngine;
using System;
using System.Collections.Generic;

public class LogrosManager : MonoBehaviour
{
	#region Properties
	public static LogrosManager Instance;
	#endregion

	#region Fields
	[SerializeField] private LogrosUI _logrosUI;
	[SerializeField] private List<LogrosData> _logrosData;
	#endregion

	#region Unity Callbacks
	private void Awake()
	{
		Instance = this;
	}
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	private void OnEnable()
	{
		AquaController.OnClicksChanged += OnClicksChanged;
		ShopManager.OnItemPurchased += OnItemPurchased;
		FishEvolution.OnEvolutionStageChanged += OnEvolutionChanged;
		SpecialFish.OnSpecialFishCollected += OnSpecialFishCollected;
	}

	private void OnDisable()
	{
		AquaController.OnClicksChanged -= OnClicksChanged;
		ShopManager.OnItemPurchased -= OnItemPurchased;
		FishEvolution.OnEvolutionStageChanged -= OnEvolutionChanged;
		SpecialFish.OnSpecialFishCollected -= OnSpecialFishCollected;
	}

	private void OnSpecialFishCollected(SpecialFishType fishType)
	{
		for (int i = 0; i < _logrosData.Count; i++)
		{
			if (!_logrosData[i].IsCompleted)
			{
				if (_logrosData[i].Type == LogroType.FirstSpecialFishBonus && fishType == SpecialFishType.Bonus)
				{
					CheckLogroCompletion(_logrosData[i]);
				}
				else if (_logrosData[i].Type == LogroType.FirstSpecialFishMultiplier && fishType == SpecialFishType.Multiplier)
				{
					CheckLogroCompletion(_logrosData[i]);
				}
				else if (_logrosData[i].Type == LogroType.FirstSpecialFishAgent && fishType == SpecialFishType.AutoAgent)
				{
					CheckLogroCompletion(_logrosData[i]);
				}
			}
		}
	}

	private void OnItemPurchased(ShopItemData data)
	{
		for (int i = 0; i < _logrosData.Count; i++)
		{
			if (!_logrosData[i].IsCompleted)
			{
				if (_logrosData[i].Type == LogroType.FirstShopPurchase)
				{
					CheckLogroCompletion(_logrosData[i]);
				}
			}
		}
	}

	private void OnClicksChanged(int totalClicks)
	{
		for (int i = 0; i < _logrosData.Count; i++)
		{
			if (!_logrosData[i].IsCompleted)
			{
				if (_logrosData[i].Type == LogroType.FirstClick && totalClicks >= 1)
				{
					CheckLogroCompletion(_logrosData[i]);
				}
				else if (_logrosData[i].Type == LogroType.MilClicks && totalClicks >= _logrosData[i].TargetAmount)
				{
					CheckLogroCompletion(_logrosData[i]);
				}
				else if (_logrosData[i].Type == LogroType.OneMillionClicks && totalClicks >= _logrosData[i].TargetAmount)
				{
					CheckLogroCompletion(_logrosData[i]);
				}
			}
		}
	}

	private void OnEvolutionChanged(int obj)
	{
		for (int i = 0; i < _logrosData.Count; i++)
		{
			if (!_logrosData[i].IsCompleted)
			{
				if (_logrosData[i].Type == LogroType.FirstEvolution)
				{
					CheckLogroCompletion(_logrosData[i]);
				}
			}
		}
	}

	private void CheckLogroCompletion(LogrosData logroData)
	{
		logroData.IsCompleted = true;
		_logrosUI.ShowLogro(logroData.Title);
	}
	#endregion

}
