using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class AquaController : MonoBehaviour
{
	#region Properties
	public static AquaController Instance;
	[field: SerializeField] public float ClickRatio { get; set; }
	[field: SerializeField] public float CurrentMultiplier { get; set; }
	public bool IsMultiplierActive => CurrentMultiplier > 1f;
	public bool IsAutoClickerActive => _autoAgentRoutine != null;

	public int TotalClicks { get; set; }

	public static Action<int> OnClicksChanged;
	public static Action<float, float> OnMultiplierChanged;
	public static Action<float> OnAutoAgentChanged;
	#endregion

	#region Fields
	[SerializeField] private ParticleSystem _particleRain;
	[SerializeField] private TextMeshProUGUI _clickText;
	[SerializeField] private TextMeshProUGUI _rewardText;
	private Coroutine _multiplierRoutine;
	private Coroutine _autoAgentRoutine;

	[SerializeField] private AgentFishVisual _agentPrefab;
	[SerializeField] private Transform _agentTransform;
	private AgentFishVisual _agentVisual;

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
		return finalClicks;
	}
	public void RainParticles()
	{
		_particleRain.Emit(Mathf.Clamp((int)ClickRatio, 0, 13));
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
		
		_autoAgentRoutine = StartCoroutine(AutoClickerAgentTimer(clicksPerSecond, duration));
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



	/**
	*       ____    U _____ u                 _       ____     ____    _    
	*    U |  _"\ u \| ___"|/__        __ U  /"\  uU |  _"\ u |  _"\ U|"|u  
	*     \| |_) |/  |  _|"  \"\      /"/  \/ _ \/  \| |_) |//| | | |\| |/  
	*      |  _ <    | |___  /\ \ /\ / /\  / ___ \   |  _ <  U| |_| |\|_|   
	*      |_| \_\   |_____|U  \ V  V /  U/_/   \_\  |_| \_\  |____/ u(_)   
	*      //   \\_  <<   >>.-,_\ /\ /_,-. \\    >>  //   \\_  |||_   |||_  
	*     (__)  (__)(__) (__)\_)-'  '-(_/ (__)  (__)(__)  (__)(__)_) (__)_) 
	*/
	private void GetReward(Reward reward)
	{
		ShowReward(reward);
		// Apply reward
		if (reward.RewardType == RewardType.Plus)
		{
			ClickRatio += reward.Value;
			_clickText.text = "x" + ClickRatio;
			return;
		}

		if (reward.RewardType == RewardType.Multi)
		{
			ClickRatio *= reward.Value;
			_clickText.text = "x" + ClickRatio;
			return;
		}

		//if (reward.RewardType == RewardType.Agent)
		//{
		//	//TODO: Implement agent reward
		//	Agent newAgent = Instantiate(_agents[(int)reward.Value], transform.position, Quaternion.identity);
		//	newAgent.destiny = reward.ObjectReward;
		//	return;
		//}
	}
	private void ShowReward(Reward reward)
	{
		// Initialize text
		if (!_rewardText.gameObject.activeSelf)
		{
			_rewardText.gameObject.SetActive(true);
			_rewardText.transform.localScale = Vector3.zero;
		}

		// Update text
		_rewardText.text = "REWARD\n" + reward.RewardType + " " + reward.Value + " clicks";

		// Crear secuencia
		Sequence mySequence = DOTween.Sequence();

		// Añadir animaciones a la secuencia
		mySequence.Append(_rewardText.transform.DOScale(1, 1));
		mySequence.Append(_rewardText.transform.DOShakeRotation(1, new Vector3(0, 0, 30)));
		mySequence.Append(_rewardText.transform.DOScale(0, 1));

		// Iniciar la secuencia
		mySequence.Play();

	}

	private void OnEnable()
	{
		FishButtonUI.OnSlotReward += GetReward;
	}

	private void OnDisable()
	{
		FishButtonUI.OnSlotReward -= GetReward;
	}


	#endregion

}
