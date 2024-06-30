using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] string filename;
    GameData gameData;
    List<IDataPersistence> dataPersistenceObjs;
    FileDataHandler DataHandler;

    // This Manager Class is a singleton
    public static DataPersistenceManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Found more than one DataPersistenceManager in the scene. Duplicates were destroyed.");
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        Instance = this;
    }

    void Start()
    {
        DataHandler = new FileDataHandler(Application.persistentDataPath, filename);
        dataPersistenceObjs = FindAllDataPersistenceObjs();
    }

    public void SaveGame(string profileID)
    {
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjs)
            dataPersistenceObj.SaveData(gameData);

        DataHandler.Save(gameData, profileID);
    }

    public void LoadGame(string profileID)
    {
        gameData = DataHandler.Load(profileID);
        if (gameData == null)
            Debug.Log("No Data was found for loading");

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjs)
            dataPersistenceObj.LoadData(gameData);
    }

    List<IDataPersistence> FindAllDataPersistenceObjs()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects 
            = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return DataHandler.LoadAllProfiles();
    }
}