using UnityEngine;
using System;
using TMPro;
using DG.Tweening;

public class LogrosUI : MonoBehaviour
{
	#region Properties
	#endregion

	#region Fields
	[SerializeField] private TextMeshProUGUI _logrosTitle;
	[SerializeField] private TextMeshProUGUI _logrosDescription;
	[SerializeField] private CanvasGroup _logrosCanvasGroup;
	[SerializeField] private float _fadeDuration = 2f;

	private Tween _currentTween;
	#endregion

	#region Unity Callbacks
	private void Awake()
	{
		_logrosCanvasGroup.alpha = 0f;
		gameObject.SetActive(false);
	}
	#endregion

	#region Public Methods
	public void ShowLogro(string logroTitle, string logroDescription)
	{
		_currentTween?.Kill();

		gameObject.SetActive(true);

		_logrosTitle.text = logroTitle;
		_logrosDescription.text = logroDescription;
		_logrosCanvasGroup.alpha = 0f;

		Sequence sequence = DOTween.Sequence();
		sequence.Append(_logrosCanvasGroup.DOFade(1f, _fadeDuration / 2f));
		sequence.AppendInterval(_fadeDuration);
		sequence.Append(_logrosCanvasGroup.DOFade(0f, _fadeDuration / 2f));
		sequence.OnComplete(() => gameObject.SetActive(false));
		_currentTween = sequence;
	}
	#endregion

	#region Private Methods
	#endregion

}
