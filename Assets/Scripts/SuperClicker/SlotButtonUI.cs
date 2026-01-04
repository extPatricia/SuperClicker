using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SlotButtonUI : MonoBehaviour
{
	#region Properties
	[field: SerializeField] public Reward Reward;
	public int ClicksLeft { 
		get 
		{ 
			return _clicksLeft;
		}
		set 
		{
			_clicksLeft = value;
			// Reward time
			if (_clicksLeft <= 0)
			{
				// Reward event
				OnSlotReward?.Invoke(Reward);
				//TODO: Hacer que cada objeto cueste un 15% mas de clicks que el anterior
				_initialStock--;
				if (_initialStock > 0)
				{
					_clicksLeft = _initialClicks;
					// almacenar el ultimo valor guardado e ir incrementado los initial clicks en un 15%
					

				}
				else
				{
					GetComponent<Image>().enabled = false;
					_clickButton.interactable = false;
					_clickText.enabled = false;
				}
				RefreshClicksText();
			}
		} 
	}

	// Only one event for all slots
	public static event Action<Reward> OnSlotReward;
	public static event Action<SlotButtonUI> OnSlotClicked;
	#endregion

	#region Fields
	[Header("UI")]
	[SerializeField] private Button _clickButton;
	[SerializeField] private TextMeshProUGUI _clickText;
	[SerializeField] private ParticleSystem _particles;
	[SerializeField] private int _materialParticleIndex;
	[Header("Prefab points")]
	[SerializeField] private PointsElementUI _prefabPoint;
	[SerializeField] private int _initialClicks = 10;
	// Initial stock
	
	private GameController _gameController;
	private int _initialStock = 5;
	private int _clicksLeft = 0;
	#endregion

	#region Unity Callbacks
	private void Awake()
	{
		_gameController = FindObjectOfType<GameController>();
		Reward.ObjectReward = this;
	}
	// Start is called before the first frame update
	void Start()
    {
		Initialize();

		_clickButton.onClick.AddListener(Click);
		//_clickButton.onClick.AddListener(() =>
		//{
		//	int clickRatio = Mathf.RoundToInt(_gameController.ClickRatio);
		//	Click(clickRatio);
		//});

		RefreshClicksText();
	}
	#endregion

	#region Public Methods
	public void Click(int clickCount, bool agent = false)
	{
		_particles.Emit(Mathf.Clamp(clickCount, 1, 15));
		ClicksLeft -= clickCount;
		RefreshClicksText();
		Camera.main.DOShakePosition(Mathf.Clamp(0.1f * clickCount, 0, 2));
		
		if (!agent)
		{
			Instantiate(_prefabPoint, transform);
			_gameController.RainParticles();
		}
	}


	#endregion

	#region Private Methods
	private void Initialize()
	{
		ClicksLeft = _initialClicks;

		// Particle frame
		float segment = 1f / 28f;
		float frame = segment * _materialParticleIndex;
		var texture = _particles.textureSheetAnimation;
		texture.startFrame = frame;
	}
	private void RefreshClicksText()
	{
		_clickText.text = ClicksLeft.ToString();
	}
	private void Click()
	{
		OnSlotClicked?.Invoke(this);
		int clickRatio = Mathf.RoundToInt(_gameController.ClickRatio);
		Click(clickRatio);
	}
	#endregion

}
