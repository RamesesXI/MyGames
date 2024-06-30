using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    string dataDirPath = "";
    string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public void Save(GameData data, string profileID)
    {
        // Use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);

        try
        {
            // Create the directory the file will be written if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize the GameData object into JSON
            string dataToStore = JsonUtility.ToJson(data, true);

            // Write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
                
        }
        catch(Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "/n" + e);
        }
    }
    
    public GameData Load(string profileID)
    {
        // Use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);

        GameData loadedData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                // Load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // Deserialize the data from JSON back into the C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "/n" + e);
            }
        }

        return loadedData;
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        // loop over all directory names in the data directory path
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            // defensive programming - check if the data file exists
            // if it doesn't, then this folder isn't a profile and should be skipped
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: "
                    + profileId);
                continue;
            }

            // load the game data for this profile and put it in the dictionary
            GameData profileData = Load(profileId);
            // defensive programming - ensure the profile data isn't null,
            // because if it is then something went wrong and we should let ourselves know
            if (profileData != null)
                profileDictionary.Add(profileId, profileData);
            else
                Debug.LogError("Tried to load profile but something went wrong (null). ProfileId: " + profileId);
        }

        return profileDictionary;
    }
}
