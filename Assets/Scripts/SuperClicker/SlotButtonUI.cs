using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class SlotButtonUI : MonoBehaviour
{
	#region Properties
	[field: SerializeField] public int ClicksLeft { 
		get 
		{ 
			return _clicksLeft;
		}
		set 
		{
			_clicksLeft = value;
			if (_clicksLeft <= 0)
			{
				Reward();
				//TODO: Hacer que cada objeto cueste un 15% mas de clicks que el anterior
				_initialStock--;
				if (_initialStock > 0)
				{
					_clicksLeft = _initialClicks;
				}
				else
				{
					Destroy(gameObject);
				}
				RefreshClicksText();
			}
		} 
	}
	public event Action OnSlotClicked;
	#endregion

	#region Fields
	[Header("UI")]
	[SerializeField] private Button _clickButton;
	[SerializeField] private TextMeshProUGUI _clickText;
	[SerializeField] private ParticleSystem _particles;
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
	}
	// Start is called before the first frame update
	void Start()
    {
		ClicksLeft = _initialClicks;
		int clickRatio = Mathf.RoundToInt(_gameController.ClickRatio);
		_clickButton.onClick.AddListener(() => Click(clickRatio));
    }


	// Update is called once per frame
	void Update()
    {
        
    }
	#endregion

	#region Public Methods
	public void Click(int clickCount)
	{
		_particles.Emit(clickCount);
		ClicksLeft -= clickCount;
		RefreshClicksText();
		Instantiate(_prefabPoint, transform);
	}

	private void RefreshClicksText()
	{
		_clickText.text = ClicksLeft.ToString();
	}

	private void Reward()
	{

	}

	#endregion

	#region Private Methods
	#endregion

}
