using UnityEngine;
using System;

public enum RewardFishType
{
	Plus,
	Multi,
	Agent
}
public class RewardFish : MonoBehaviour
{
	#region Properties
	[field: SerializeField] public RewardFishType RewardFishType { get; set; }
	[field: SerializeField] public float Value { get; set; }
	public FishButtonUI ObjectReward { get; set; }
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
