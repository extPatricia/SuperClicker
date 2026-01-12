using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class AquaController : MonoBehaviour, IResettable
{
	#region Properties
	public static AquaController Instance;
	[field: SerializeField] public float ClickRatio { get; set; }
	[field: SerializeField] public float CurrentMultiplier { get; set; }
	public bool IsMultiplierActive => CurrentMultiplier > 1f;
	public bool IsAutoClickerActive => _autoAgentRoutine != null;
	public bool IsAnySpecialFishActive => IsMultiplierActive || IsAutoClickerActive;
	public float ClicksPerSecond => _autoClicksPerSecond;
	public int TotalClicks { get; set; }

	public static Action<int> OnClicksChanged;
	public static Action<float, float> OnMultiplierChanged;
	public static Action<float> OnAutoAgentChanged;
	public static Action<float> OnClicksPerSecondChanged;
	#endregion

	#region Fields
	[SerializeField] private ParticleSystem _particleRain;

	[Header("Multiplier")]
	[SerializeField] private MultiplierUI _multiplierUI;	
	private Coroutine _multiplierRoutine;
	

	[Header("Agent Fish")]
	[SerializeField] private AgentFishVisual _agentPrefab;
	[SerializeField] private Transform _agentTransform;
	[SerializeField] private AutoAgentUI _autoAgentUI;
	private Coroutine _autoAgentRoutine;
	private AgentFishVisual _agentVisual;

	private float _autoClicksPerSecond;

	private const string CLICK_KEY = "TOTAL_CLICKS";
	private const string CLICK_RATIO_KEY = "CLICK_RATIO";
	private const string MULTIPLIER_KEY = "CURRENT_MULTIPLIER";
	private const string AUTO_CLICKS_KEY = "AUTO_CLICKS_PER_SECOND";

	#endregion

	#region Unity 
	private void Awake()
	{
		Instance = this;
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	#endregion

	#region Public Methods
	public int AddClicks(int clicks)
	{
		int finalClicks = Mathf.RoundToInt(clicks * CurrentMultiplier);
		TotalClicks += finalClicks;
		OnClicksChanged?.Invoke(TotalClicks);
		OnClicksPerSecondChanged?.Invoke(_autoClicksPerSecond);
		return finalClicks;
	}
	public void RainParticles()
	{
		_particleRain.Emit(Mathf.Clamp((int)ClickRatio, 0, 2));
	}

	/**
	 *     .--.                    _        .-.     .--. _       .-.   
	 *    : .--'                  :_;       : :    : .-':_;      : :   
	 *    `. `. .---.  .--.  .--. .-. .--.  : :    : `; .-. .--. : `-. 
	 *     _`, :: .; `' '_.''  ..': :' .; ; : :_   : :  : :`._-.': .. :
	 *    `.__.': ._.'`.__.'`.__.':_;`.__,_;`.__;  :_;  :_;`.__.':_;:_;
	 *          : :                                                    
	 *          :_;                                                    
	 */
	public void ActivateMultiplier(float multiplier, float duration)
	{
		if (_multiplierRoutine != null)
		{
			StopCoroutine(_multiplierRoutine);
		}

		CurrentMultiplier = multiplier;
		_multiplierUI.Initialize(duration);
		_multiplierRoutine = StartCoroutine(MultiplierTimer(multiplier, duration));
	}
	public void ActivateAutoClickerAgent(int clicksPerSecond, float duration)
	{
		if (_autoAgentRoutine != null)
		{
			StopCoroutine(_autoAgentRoutine);
		}

		if (_agentVisual == null)
		{
			_agentVisual = Instantiate(_agentPrefab);
			_agentVisual.SetTarget(_agentTransform);
		}
		
		_autoAgentUI.Initialize(duration);
		_autoAgentRoutine = StartCoroutine(AutoClickerAgentTimer(clicksPerSecond, duration));
	}

	/**
	 *      _   _             _   _    
	 *     |_) |_ \    / /\  |_) | \ | 
	 *     | \ |_  \/\/ /--\ | \ |_/ o 
	 *                                 
	 */
	public void ApplyReward(ShopItemData item)
	{
		switch (item.RewardType)
		{
			case ShopRewardType.Clicker:
				ClickRatio += item.Clicker;
				break;
			case ShopRewardType.Multiplier:
				CurrentMultiplier *= item.Multiplier;
				break;
			case ShopRewardType.AutoAgent:
				_autoClicksPerSecond += item.AgentRate;
				StartAutoClickerAgent();
				
				break;
			default:
				break;
		}
	}

	public void DeductClicks(int amount)
	{
		TotalClicks -= amount;
		OnClicksChanged?.Invoke(TotalClicks);
	}

	public void ResetData()
	{
		TotalClicks = 0;
		ClickRatio = 1f;
		CurrentMultiplier = 1f;
		//_autoClicksPerSecond = 0f;
		StopAllCoroutines();

		if (_agentVisual != null)
		{
			Destroy(_agentVisual.gameObject);
			_agentVisual = null;
		}

		OnClicksChanged?.Invoke(TotalClicks);
		OnMultiplierChanged?.Invoke(CurrentMultiplier, 0f);
		OnAutoAgentChanged?.Invoke(0f);
		OnClicksPerSecondChanged?.Invoke(_autoClicksPerSecond);
	}

	public void SaveData()
	{
		PlayerPrefs.SetInt(CLICK_KEY, TotalClicks);
		PlayerPrefs.SetFloat(CLICK_RATIO_KEY, ClickRatio);
		PlayerPrefs.SetFloat(MULTIPLIER_KEY, CurrentMultiplier);
		PlayerPrefs.SetFloat(AUTO_CLICKS_KEY, _autoClicksPerSecond);
	}

	public void LoadData()
	{
		TotalClicks = PlayerPrefs.GetInt(CLICK_KEY, 0);
		ClickRatio = PlayerPrefs.GetFloat(CLICK_RATIO_KEY, 1f);
		CurrentMultiplier = PlayerPrefs.GetFloat(MULTIPLIER_KEY, 1f);
		_autoClicksPerSecond = PlayerPrefs.GetFloat(AUTO_CLICKS_KEY, 0f);

		OnClicksChanged?.Invoke(TotalClicks);
		OnClicksPerSecondChanged?.Invoke(_autoClicksPerSecond);
	}
	#endregion

	#region Private Methods
	private IEnumerator MultiplierTimer(float multiplier, float duration)
	{
		CurrentMultiplier = multiplier;
		float timer = duration;

		while (timer > 0)
		{
			OnMultiplierChanged?.Invoke(CurrentMultiplier, timer);
			timer -= Time.deltaTime;
			yield return null;
		}

		CurrentMultiplier = 1f;
		OnMultiplierChanged?.Invoke(CurrentMultiplier, 0f);
	}


	private IEnumerator AutoClickerAgentTimer(int clicksPerSecond, float duration)
	{
		float timer = duration;

		while (timer > 0)
		{
			AddClicks(clicksPerSecond);
			OnAutoAgentChanged?.Invoke(timer);
			timer -= 1f;
			yield return new WaitForSeconds(1f);
		}

		if (_agentVisual != null)
		{
			Destroy(_agentVisual.gameObject);
			_agentVisual = null;
		}

		OnAutoAgentChanged?.Invoke(0f);
		_autoAgentRoutine = null;
	}

	private void OnEnable()
	{
		FishButtonUI.OnChangeParticleSprite += ChangeCanvasParticleSystem;
	}

	private void OnDisable()
	{
		FishButtonUI.OnChangeParticleSprite -= ChangeCanvasParticleSystem;
	}

	private void ChangeCanvasParticleSystem(Sprite sprite)
	{
		var texture = _particleRain.textureSheetAnimation;
		texture.enabled = true;
		texture.SetSprite(0, sprite);
	}

	private void StartAutoClickerAgent()
	{
		if (_autoAgentRoutine != null)
			return;

		_autoAgentRoutine = StartCoroutine(AutoClickerAgent());
	}

	private IEnumerator AutoClickerAgent()
	{
		while (true)
		{
			if (_autoClicksPerSecond > 0)
			{
				AddClicks(Mathf.RoundToInt(_autoClicksPerSecond));
			}

			yield return new WaitForSeconds(1f);
		}
	}


	#endregion

}
