using UnityEngine;
using System;

public enum LogroType
{
	FirstClick,
	MilClicks,
	OneMillionClicks,
	FirstMultiplier,
	FirstAutoClicker,
	FirstEvolution,
	FirstShopPurchase,
	FirstSpecialFishBonus,
	FirstSpecialFishMultiplier,
	FirstSpecialFishAgent,

}

[CreateAssetMenu(menuName = "Logros/Logro")]
public class LogrosData : ScriptableObject
{
	#region Properties
	public string Id;
	public string Title;
	public string Description;

	public LogroType Type;
	public int TargetAmount;
	[HideInInspector] public bool IsCompleted;
	#endregion

	#region Fields
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
	#endregion
   
}
