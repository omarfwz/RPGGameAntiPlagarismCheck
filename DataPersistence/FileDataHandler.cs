using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public PlayerData Load(string profileID)
    {
        if(profileID == null)
        {
            return null;
        }
        string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
        PlayerData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<PlayerData>(dataToLoad);

            } catch
            {
                Debug.LogError("error occured when tryna load data from file " + fullPath);
            }
        }
        return loadedData;
    }
    public void Save(PlayerData data, string profileID)
    {
        if (profileID == null)
        {
            return;
        }
        string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);
            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch
        {
            Debug.LogError("womp womp womp... save data cannot go to file :P");
        }
    }

    public Dictionary<String, PlayerData> LoadAllProfiles()
    {
        Dictionary<string, PlayerData> profileDictionary = new Dictionary<string, PlayerData>();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach(DirectoryInfo dirInfo in dirInfos)
        {
            string profileID = dirInfo.Name;
            string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping folder " + profileID + " that does not contain data.");
                continue;
            }

            PlayerData profileData = Load(profileID);
            if (profileID != null)
            {
                profileDictionary.Add(profileID, profileData);
            }
            else
            {
                Debug.LogError("Tried to load profile but something went wrong. ProfileID: " + profileID);
            }
        }

        return profileDictionary;
    }

    public string GetMostRecentlyUpdatedProfileID()
    {
        string mostRecentProfileID = null;
        Dictionary<string, PlayerData> profilesData = LoadAllProfiles();
        foreach(KeyValuePair<string, PlayerData> pair in profilesData)
        {
            string profileID = pair.Key;
            PlayerData playerData = pair.Value;
            if(playerData == null)
            {
                continue;
            }

            if(mostRecentProfileID == null)
            {
                mostRecentProfileID = profileID;
            }
            else
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesData[mostRecentProfileID].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(playerData.lastUpdated);
                if (newDateTime > mostRecentDateTime)
                {
                    mostRecentProfileID = profileID;
                }
            }
        }
        return mostRecentProfileID;
    }
    
}
