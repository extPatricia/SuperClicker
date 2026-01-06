using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public enum SpecialFishType
{
	Bonus,
	Multiplier,
	AutoAgent
}
public class SpecialFish : MonoBehaviour
{
	#region Properties
	[field: SerializeField] public SpecialFishType FishType { get; set; }

	public static Action OnAnySpecialFishDestroyed;
	#endregion

	#region Fields
	[SerializeField] private Button _clickButton;
	[Header("Values")]
	[SerializeField] private int _value;
	[SerializeField] private float _duration;
	[SerializeField] private float _destroyYPosition = -70f;
	[Header("Prefab Points")]
	[SerializeField] private PointsUI _prefabPoint;
	[SerializeField] private AudioClip _soundPoints;

	private AquaController _aquaController;
	private bool _consumed;
	#endregion


	#region Unity 
	private void Awake()
	{
		_aquaController = FindObjectOfType<AquaController>();
	}
	// Start is called before the first frame update
	void Start()
	{
		_clickButton.onClick.AddListener(OnFishClicked);
	}

	// Update is called once per frame
	void Update()
	{
		if (transform.position.y < _destroyYPosition)
		{
			Destroy(gameObject);
		}
	}
	#endregion

	#region Public Methods
	private void OnFishClicked()
	{

		if (_consumed) return;
		_consumed = true;

		// Show animation
		ShowAnimation();
		// Apply reward based on type
		ApplyReward();
		// Destroy fish
		Destroy(gameObject);
	}



	#endregion

	#region Private Methods 
	private void ShowAnimation()
	{
		AudioSource.PlayClipAtPoint(_soundPoints, Camera.main.transform.position);
		PointsUI pointsUI = Instantiate(_prefabPoint, transform.position, Quaternion.identity, transform.parent);
		
		switch (FishType)
		{
			case SpecialFishType.Bonus:
				pointsUI.SetText("+" + _value.ToString());
				break;
			case SpecialFishType.Multiplier:
				pointsUI.SetText("x" + _value.ToString());
				break;
			case SpecialFishType.AutoAgent:
				pointsUI.SetText("Auto Agent +" + _value.ToString() + "!");
				break;
			default:
				pointsUI.SetText(_value.ToString());
				break;
		}

		transform.DOScale(1.3f, 0.15f)
			.SetLoops(2, LoopType.Yoyo);
	}

	private void ApplyReward()
	{
		switch (FishType)
		{
			case SpecialFishType.Bonus:
				// Plus clicks
				_aquaController.AddClicks(_value);
				break;
			case SpecialFishType.Multiplier:
				// Apply score multiplier
				_aquaController.ActivateMultiplier(_value, _duration); 
				break;
			case SpecialFishType.AutoAgent:
				// Trigger auto agent mode
				_aquaController.ActivateAutoClickerAgent(_value, _duration);
				break;
			default:
				Debug.Log("Unknown Special Fish type.");
				break;
		}

	}

	private void OnDestroy()
	{
		OnAnySpecialFishDestroyed?.Invoke();
	}

	#endregion

}
