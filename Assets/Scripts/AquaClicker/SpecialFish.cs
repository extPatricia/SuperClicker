using UnityEngine;
using System;

public enum SpecialFishType
{
	Bonus,
	Multiplier,
	Frenzy
}
public class SpecialFish : MonoBehaviour
{
	#region Properties
	[field: SerializeField] public SpecialFishType FishType { get; set; }

	[Header("Values")]
	public float Value;
	public float Duration;
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
