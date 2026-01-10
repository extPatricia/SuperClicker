using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
	#region Properties
	#endregion

	#region Fields
	[SerializeField] private ShopItemData _shopItemData;
	[SerializeField] private TextMeshProUGUI _itemNameText;
	[SerializeField] private TextMeshProUGUI _itemPriceText;
	[SerializeField] private Button _itemButton;
	[SerializeField] private AudioClip _soundBuyItem;
	[SerializeField] private TextMeshProUGUI _rewardText;
	#endregion

	#region Unity Callbacks
	#endregion

	#region Public Methods
	public void BuyItemClicked()
	{
		Debug.Log("Clicked Buy Item: " + _shopItemData.ItemName);
		bool canBuy = ShopManager.Instance.PurchaseItem(_shopItemData);
	
		if (canBuy)
		{
			UpdateItemPrice();
			PlayBuyAnimation();
		}
	}
	#endregion

	#region Private Methods
	private void OnEnable()
	{
		UpdateItemPrice();
		AquaController.OnClicksChanged += UpdateButton;
	}

	private void OnDisable()
	{
		AquaController.OnClicksChanged -= UpdateButton;
	}

	private void UpdateItemPrice()
	{
		Debug.Log("Updating Item Price for: " + _shopItemData.ItemName);
		Debug.Log("Purchased Count: " + _shopItemData.PurchasedCount);
		_itemNameText.text = _shopItemData.ItemName;
		_itemPriceText.text = ShopManager.Instance.GetItemCost(_shopItemData).ToString() + "clicks";

		UpdateButton(AquaController.Instance.TotalClicks);
	}

	private void UpdateButton(int currentClicks)
	{
		int itemCost = ShopManager.Instance.GetItemCost(_shopItemData);
		if (currentClicks >= itemCost)
		{
			_itemButton.interactable = true;
		}
		else
		{
			_itemButton.interactable = false;
		}
	}

	private void PlayBuyAnimation()
	{
		PlayRewardText();
		PlayBuySound();
	}

	private void PlayRewardText()
	{
		_rewardText.text = $"{_shopItemData.ItemName} {_shopItemData.RewardType}!";
		_rewardText.gameObject.SetActive(true);
		_rewardText.alpha = 0f;
		_rewardText.transform.localPosition = Vector3.zero;

		Sequence seq = DOTween.Sequence();
		seq.Append(_rewardText.DOFade(1f, 0.2f));
		seq.Join(_rewardText.transform.DOLocalMoveY(40f, 0.6f));
		seq.AppendInterval(0.8f);
		seq.Append(_rewardText.DOFade(0f, 0.3f));
		seq.OnComplete(() =>
		{
			_rewardText.gameObject.SetActive(false);
		});

	}

	private void PlayBuySound()
	{
		if (_soundBuyItem != null)
		{
			AudioSource.PlayClipAtPoint(_soundBuyItem, Camera.main.transform.position);
		}
	}
	#endregion

}
