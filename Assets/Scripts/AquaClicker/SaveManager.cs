using UnityEngine;
using System;

public class SaveManager : MonoBehaviour
{    
    #region Properties
	#endregion

	#region Fields
	#endregion

	#region Unity Callbacks
	// Start is called before the first frame update
	void Start()
    {
		LoadGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	#endregion

	#region Public Methods
	public void SaveGame()
	{
		AquaController.Instance.SaveData();
		ShopManager.Instance.SaveData();
		LogrosManager.Instance.SaveData();
		ClickProgressionManager.Instance.SaveData();
		FishEvolution.Instance.SaveData();
		Debug.Log("Game Saved!");
	}

	public void LoadGame()
	{
		AquaController.Instance.LoadData();
		ShopManager.Instance.LoadData();
		LogrosManager.Instance.LoadData();
		ClickProgressionManager.Instance.LoadData();
		FishEvolution.Instance.LoadData();
		Debug.Log("Game Loaded!");
	}
	#endregion

	#region Private Methods
	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			SaveGame();
		}
	}

	private void OnApplicationQuit()
	{
		SaveGame();
	}
	#endregion

}
