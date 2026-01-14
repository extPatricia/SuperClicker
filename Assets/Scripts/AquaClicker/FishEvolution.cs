using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FishEvolution : MonoBehaviour
{
	#region Properties
	public static Action<int> OnEvolutionStageChanged;
	#endregion

	#region Fields
	[Header("Evolution Stages")]
	[SerializeField] private Image _fishImage;
	[SerializeField] private Sprite[] _evolutionStages;
	[Header("Audio")]
	[SerializeField] private AudioSource _audioSource;
	[SerializeField] private AudioClip _evolutionSound;

	private int _currentStage = 0;
	#endregion

	#region Unity Callbacks
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
	#endregion

	#region Private Methods
	private void OnEnable()
	{
		AquaController.OnClicksChanged += CheckEvolution;	
	}

	private void OnDisable()
	{
		AquaController.OnClicksChanged -= CheckEvolution;
	}

	private void CheckEvolution(int totalClicks)
	{
		int newStage = GetEvolutionIndex(totalClicks);

		if (newStage != _currentStage && newStage < _evolutionStages.Length)
		{
			_currentStage = newStage;
			_fishImage.sprite = _evolutionStages[_currentStage];
			OnEvolutionStageChanged?.Invoke(_currentStage);
			transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo);
			_audioSource.PlayOneShot(_evolutionSound);
		}
	}

	private int GetEvolutionIndex(int totalClicks)
	{
		if (totalClicks >= 10000000) return 5;
		if (totalClicks >= 1500000) return 4;
		if (totalClicks >= 250000) return 3;
		if (totalClicks >= 50000) return 2;
		if (totalClicks >= 2000) return 1;
		return 0;
	}
	#endregion

}
