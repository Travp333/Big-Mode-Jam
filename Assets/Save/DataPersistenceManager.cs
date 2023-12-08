using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
	[Header("Save file name")]
	[SerializeField]
	private string fileName;
	private GameData gameData;
	private List<IDataPersistence> dataPersistenceObjects;
	private FileDataHandler dataHandler;
	public static DataPersistenceManager instance { get;  private set;}
	private void Awake()
	{
		if (instance != null && instance != this) {
			Destroy(instance);
		}
		instance = this;
	}
	private void Start()
	{
		this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
		this.dataPersistenceObjects = FindAllDataPersistenceObjects();
		LoadGame();
	}
	public void NewGame() {
		this.gameData = new GameData();
	}
	public void LoadGame() {
		//TODO - Load any saved data from file
		this.gameData = dataHandler.Load();

		if (this.gameData == null) {
			Debug.Log("No save data was found, making fresh save");
			NewGame();
		}
		// push the loaded data to all other scripts
			foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) {
			dataPersistenceObj.LoadData(gameData);
			}
		Debug.Log(gameData.playerHp + " , " + gameData.playerPos);
	}
	public void SaveGame() 
	{
		//pass the data to other scripts
		foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
		{
			dataPersistenceObj.SaveData(ref gameData);
		}

		//save the data to file
		Debug.Log(gameData.playerHp + " , " + gameData.playerPos);

		dataHandler.Save(gameData);
	}
	private void OnApplicationQuit()
	{
		SaveGame();
	}
	private List<IDataPersistence> FindAllDataPersistenceObjects() {
		IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
			.OfType<IDataPersistence>();
		return new List<IDataPersistence>(dataPersistenceObjects);
	}
}
