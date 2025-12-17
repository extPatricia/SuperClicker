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
	private AquaController _aquaController;
	private RectTransform _rectTransform;
	#endregion

	#region Unity Callbacks
	private void Awake()
	{
		_aquaController = FindObjectOfType<AquaController>();
		_rectTransform = GetComponent<RectTransform>();
	}
	// Start is called before the first frame update
	void Start()
    {
		// Set clicks text
		_textClicks.text = "+" + _aquaController.ClickRatio.ToString();

		// Movement
		_rectTransform.DOAnchorPosY(_rectTransform.anchoredPosition.y + UnityEngine.Random.Range(100, 500), _duration);

		// Color Fade Out
		_textClicks.DOColor(new Color(0, 0, 0, 0), _duration);
		Destroy(gameObject, _duration);

	}

	// Update is called once per frame
	void Update()
    {
        
    }
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	#endregion
   
}
