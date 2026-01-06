using UnityEngine;
using System;
using DG.Tweening;

public class AgentFishVisual : MonoBehaviour
{
	#region Properties
	#endregion

	#region Fields
	[SerializeField] private Vector3 _finalOffset = new Vector3(1.5f, 0.3f, 0);
	[SerializeField] private float _moveDuration = 0.6f;

	[SerializeField] private float _biteDistance = 0.8f;
	[SerializeField] private float _biteDuration = 0.3f;

	// Fish button
	private Transform _target;
	// Floating animation
	private Tween _biteTween;
	private bool _initialized = false;

	private Vector3 _localRestPosition;
	#endregion

	#region Unity Callbacks
	#endregion

	#region Public Methods
	public void SetTarget(Transform target)
	{
		if (_initialized) return;

		_initialized = true;
		_target = target;
		PlayAnimation();
	}

	public void PlayAnimation()
	{
		transform.position = Vector3.zero;
		// Move to target
		transform.DOMove(
			_target.position + _finalOffset,
			_moveDuration)
			.SetEase(Ease.OutBack)
			.OnComplete(() => 
			{
				_localRestPosition = transform.localPosition;
				PlayBiteAnimation();
			});

	}
	#endregion

	#region Private Methods
	private void PlayBiteAnimation()
	{
		_biteTween = transform.DOLocalMove(_target.position + Vector3.right * _biteDistance, _biteDuration)
			.SetLoops(-1, LoopType.Yoyo)
			.SetEase(Ease.InOutQuad);
	}

	private void OnDisable()
	{
		_biteTween?.Kill();
	}
	#endregion

}
