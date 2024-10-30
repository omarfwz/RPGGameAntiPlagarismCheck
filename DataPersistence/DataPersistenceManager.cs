using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{

    [SerializeField] private string fileName;

    private PlayerData data;
    private FileDataHandler dataHandler;
    public static DataPersistenceManager instance { get; private set; }
    private List<IDataPersistence> dataPersistenceObjects;

    public string selectedProfileID = "";

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager");
            Destroy(this.gameObject);
            return;
            
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        selectedProfileID = dataHandler.GetMostRecentlyUpdatedProfileID();

    }

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        dataPersistenceObjects = FindAllDataPersistences();
        if (scene.name == "MainMenu")
        {
            LoadGame();
        }
    }
  
    public void ChangeSelectedProfileID(string newProfileID)
    {
        selectedProfileID = newProfileID;
        LoadGame();
    }
    public void NewGame()
    {
        this.data = new PlayerData();
        SaveGame();

    }
    public void SaveGame()
    {
        if(data == null)
        {
            Debug.LogWarning("no data found.");
            return;
        }
        foreach (IDataPersistence dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.SaveData(ref data);
        }
        data.lastUpdated = System.DateTime.Now.ToBinary();
        dataHandler.Save(data, selectedProfileID);
    }
    public void LoadGame()
    {
        data = dataHandler.Load(selectedProfileID);//
        //load data
        if(this.data == null)
        {
            Debug.Log("No loaded data. New game needs to be made.");
            return;
        }
        foreach(IDataPersistence dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.LoadData(data);
        }
    }
    private List<IDataPersistence> FindAllDataPersistences()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    public bool HasData()
    {
        return data != null;
    }

    public Dictionary<string, PlayerData> GetAllProfilesData()
    {
        return dataHandler.LoadAllProfiles();
    }

    public void SetSavePoint(string savePoint)
    {
        data.savePoint = savePoint;
    }

    public void StartLoadedGame()
    {
        SceneManager.LoadSceneAsync("Gameplay");
        SceneManager.LoadSceneAsync("TempleLandingSpawn", LoadSceneMode.Additive);
    }
}
