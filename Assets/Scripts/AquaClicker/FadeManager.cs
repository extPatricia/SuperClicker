using UnityEngine;
using System;
using TMPro;
using DG.Tweening;

public class FadeManager : MonoBehaviour
{    
    #region Properties
	public static FadeManager Instance { get; set; }
	#endregion

	#region Fields
	[Header("Fade Settings")]
	[SerializeField] private CanvasGroup _fadeCanvasGroup;
	[SerializeField] private float _fadeDuration = 2f;
	[SerializeField] private TextMeshProUGUI _newGameText;
	[SerializeField] private ParticleSystem _bubbleParticleSystem;
	[SerializeField] private AudioClip _resetSound;
	#endregion

	#region Unity Callbacks
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			_fadeCanvasGroup.alpha = 0f;
		}
		else
		{
			Destroy(gameObject);
		}
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
	public void StartFadeResetGame(System.Action onReset)
	{
		// Cancel any existing animations
		_fadeCanvasGroup.DOKill();
		// Create a new sequence
		Sequence fadeSequence = DOTween.Sequence();
		// Fade to black
		fadeSequence.Append(_fadeCanvasGroup.DOFade(1f, _fadeDuration).SetEase(Ease.InOutQuad));
		// Callback to reset game
		fadeSequence.AppendCallback(() => 
		{
			onReset?.Invoke();
			// Play reset sound
			AudioSource.PlayClipAtPoint(_resetSound, Camera.main.transform.position);
			// Vibrate device (if supported)
#if UNITY_ANDROID || UNITY_IOS
			Handheld.Vibrate();
#endif
			// Show "New Game" text
			_newGameText.gameObject.SetActive(true);
			// Play bubble particle effect
			_bubbleParticleSystem.Play();
		});

		// Wait for a moment
		fadeSequence.AppendInterval(2f);
		// Fade back to transparent
		fadeSequence.Append(_fadeCanvasGroup.DOFade(0f, _fadeDuration).SetEase(Ease.InOutQuad));


	}
#endregion

	#region Private Methods
	#endregion

}
