using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class PointsUI : MonoBehaviour
{
	#region Properties
	#endregion

	#region Fields
	[SerializeField] private TextMeshProUGUI _textClicks;
	[SerializeField] private float _duration = 2;
	private RectTransform _rectTransform;
	private AquaController _aquaController;
	#endregion

	#region Unity Callbacks
	private void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
		_aquaController = AquaController.Instance;
	}
	// Start is called before the first frame update
	void Start()
    {
		DoEffect();
	}

	// Update is called once per frame
	void Update()
    {
        
    }
	#endregion

	#region Public Methods
	public void Inizialite()
	{
		_textClicks.color = Color.white;
		gameObject.SetActive(true);
		DoEffect();
	}

	public void DoEffect()
	{
		// Movement
		_rectTransform.DOAnchorPosY(_rectTransform.anchoredPosition.y + UnityEngine.Random.Range(100, 500), _duration);

		// Color Fade Out
		_textClicks.DOColor(new Color(0, 0, 0, 0), _duration);

		_aquaController.PoolSystem.AddToPool(this, _duration);
		//Destroy(gameObject, _duration);

	}
	public void SetText(string text)
	{
		_textClicks.text = text;
	}
	#endregion

	#region Private Methods
	#endregion

}
