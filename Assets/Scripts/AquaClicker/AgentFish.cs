using DG.Tweening;
using System;
using UnityEngine;

public class AgentFish : MonoBehaviour
{
	#region Properties
	public FishButtonUI destiny { get; set; }
	[field: SerializeField] public float RepeatRate { get; set; }
	#endregion

	#region Fields
	#endregion

	#region Unity Callbacks
	// Start is called before the first frame update
	void Start()
	{
		Movement();
		InvokeRepeating(nameof(Click), 1, RepeatRate);
		//SlotButtonUI.OnSlotClicked += SetDestiny;
	}

	// Update is called once per frame
	void Update()
	{

	}
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	protected void Movement()
	{
		transform.DOMove(destiny.transform.position, 1);
	}

	private void Click()
	{
		//destiny.Click(1, true);
		//// Only angel
		//if (destiny.ClicksLeft < 0)
		//{
		//	Destroy(gameObject);
		//}
	}
	private void SetDestiny(FishButtonUI newDestiny)
	{
		destiny = newDestiny;
		Movement();
	}
	#endregion

}
