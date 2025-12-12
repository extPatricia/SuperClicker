using UnityEngine;
using System;
using DG.Tweening;
using Random = UnityEngine.Random;
using TMPro;

public class PointsElementUI : MonoBehaviour
{    
    #region Properties
	#endregion

	#region Fields
	[SerializeField] private TextMeshProUGUI _textClicks;
	[SerializeField] private float _duration = 3;
	private GameController _gameController;
	#endregion

	#region Unity Callbacks
	private void Awake()
	{
		_gameController = FindObjectOfType<GameController>();
	}
	// Start is called before the first frame update
	void Start()
    {
		// Set clicks text
		_textClicks.text = "+" + _gameController.ClickRatio.ToString();
		
		// Movement
		transform.DOMoveY(transform.position.y + Random.Range(100, 500), _duration);
		
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
