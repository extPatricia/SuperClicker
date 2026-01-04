using DG.Tweening;
using System;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishButtonUI : MonoBehaviour
{
	#region Properties
	public int ClicksLeft
	{
		get
		{
			return _clicksLeft;
		}
		set
		{
			_clicksLeft = value;
		}
	}

	public static event Action<Reward> OnSlotReward;
	#endregion

	#region Fields
	[Header("UI")]
	[SerializeField] private Button _clickButton;
	[SerializeField] private ParticleSystem _particles;
	[SerializeField] private Sprite[] _particleSprites; 
	[SerializeField] private TextMeshProUGUI _clickText;
	[Header("Prefab Points")]
	[SerializeField] private PointsUI _prefabPoint;

	private AquaController _aquaController;
	private int _clicksLeft;	
	#endregion

	#region Unity 
	private void Awake()
	{
		_aquaController = FindObjectOfType<AquaController>();
	}
	// Start is called before the first frame update
	void Start()
    {
		Inizialite();

		_clickButton.onClick.AddListener(Click);
	}
	#endregion

	#region Public Methods
	public void Click(int clickCount)
	{
		_particles.Emit(Mathf.Clamp(clickCount, 1, 15));
		ClicksLeft -= clickCount;
		Camera.main.DOShakePosition(Mathf.Clamp(0.1f * clickCount, 0, 2));

		// Update Aqua Clicks
		int finalClicks = _aquaController.AddClicks(clickCount);

		// Instantiate points UI
		PointsUI point = Instantiate(_prefabPoint, transform);

		RectTransform buttonRect = GetComponent<RectTransform>();
		RectTransform pointRect = point.GetComponent<RectTransform>();

		Vector2 randomPos = new Vector2(
			UnityEngine.Random.Range(-buttonRect.rect.width / 2, buttonRect.rect.width / 2),
			UnityEngine.Random.Range(-buttonRect.rect.height / 2, buttonRect.rect.height / 2)
		);

		pointRect.anchoredPosition = randomPos;
		point.SetText("+" + finalClicks.ToString());

		_aquaController.RainParticles();

	}
	#endregion

	#region Private Methods
	private void Inizialite()
	{
		ClicksLeft = 100;

		// Particle frame
		float segment = 1f / 28f;
		float frame = segment;
		var texture = _particles.textureSheetAnimation;
		texture.startFrame = frame;
	}
	private void Click()
	{
		int clickRatio = Mathf.RoundToInt(_aquaController.ClickRatio);
		Click(clickRatio);
	}
	private void RefreshClicksText(int totalClicks)
	{
		_clickText.text = totalClicks + " fishes!";
	}

	private void OnEnable()
	{
		AquaController.OnClicksChanged += RefreshClicksText;
		FishEvolution.OnEvolutionStageChanged += ChangeParticleSprite;
	}

	private void OnDisable() 
	{
		AquaController.OnClicksChanged -= RefreshClicksText;
		FishEvolution.OnEvolutionStageChanged -= ChangeParticleSprite;
	}
	private void ChangeParticleSprite(int evolutionIndex)
	{
		var texture = _particles.textureSheetAnimation;
		texture.enabled = true;
		texture.mode = ParticleSystemAnimationMode.Sprites;

		texture.RemoveSprite(0);
		texture.AddSprite(_particleSprites[evolutionIndex]);
	}
	#endregion	

}
