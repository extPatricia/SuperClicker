using UnityEngine;
using System;

public class AquaController : MonoBehaviour
{
	#region Properties
	[field: SerializeField] public float ClickRatio { get; set; }

	public int TotalClicks { get; set; }
	#endregion

	#region Fields
	[SerializeField] private ParticleSystem _particleRain;
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
	public void AddClicks(int clicks)
	{
		TotalClicks += clicks;
	}
	public void RainParticles()
	{
		_particleRain.Emit(Mathf.Clamp((int)ClickRatio, 0, 13));
	}
	#endregion

	#region Private Methods
	#endregion

}
