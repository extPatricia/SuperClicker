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
	public static Action<SpecialFishType> OnSpecialFishCollected;

	#endregion

	#region Fields
	[SerializeField] private Button _clickButton;
	[Header("Bonus Fish Settings")]
	[SerializeField] private float _bonusPercentange = 0.03f;
	[Header("Values")]
	[SerializeField] private int _minTotalClicks;
	[SerializeField] private int _maxSpawns;
	[SerializeField] private float _value;
	[SerializeField] private float _duration;
	[SerializeField] private float _destroyYPosition = -70f;
	[Header("Prefab Points")]
	[SerializeField] private PointsUI _prefabPoint;
	private AudioSource _audioSource;
	[SerializeField] private AudioClip _soundPoints;

	private AquaController _aquaController;
	private bool _consumed;
	#endregion


	#region Unity 
	private void Awake()
	{
		_aquaController = FindObjectOfType<AquaController>();
		_audioSource = Camera.main.GetComponent<AudioSource>();
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
	public bool CanSpawn(int totalClicks, int spawnedCount)
	{
		if (totalClicks < _minTotalClicks)
			return false;
		if (spawnedCount >= _maxSpawns)
			return false;
		return true;
	}


	#endregion

	#region Private Methods 
	private void OnFishClicked()
	{

		if (_consumed) return;
		_consumed = true;

		// Show animation
		ShowAnimation();
		// Apply reward based on type
		ApplyReward();
		// Notify listeners
		OnSpecialFishCollected?.Invoke(FishType);
		// Destroy fish
		Destroy(gameObject);
	}
	private void ShowAnimation()
	{
		_audioSource.PlayOneShot(_soundPoints);
		PointsUI pointsUI = Instantiate(_prefabPoint, transform.position, Quaternion.identity, transform.parent);
		
		switch (FishType)
		{
			case SpecialFishType.Bonus:
				pointsUI.SetText("+BONUS!");
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
				int bonus = Mathf.CeilToInt(_aquaController.TotalClicks * _bonusPercentange);
				_aquaController.AddClicks(bonus);
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
