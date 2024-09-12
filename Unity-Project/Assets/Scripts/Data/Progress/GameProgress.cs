using System;
using System.IO;
using UnityEngine;

public class GameProgress
{
    public int CurrentLevel;

    private const string Path = "/Progress.josn";


    /// <summary>
    /// Saves the current progress.
    /// better to not save as json *_*
    /// </summary>
    public void SaveProgress()
    {
        string json = JsonUtility.ToJson(this, true);
        string jsonPath = Application.persistentDataPath + Path;
        File.WriteAllText(jsonPath, json);
    }

    /// <summary>
    /// Load the progress if it exists
    /// </summary>
    public static GameProgress LoadProgress()
    {
        try
        {
            if(File.Exists(Application.persistentDataPath + Path))
            {
                string json = File.ReadAllText(Application.persistentDataPath + Path);
                return JsonUtility.FromJson<GameProgress>(json);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        return new GameProgress() { CurrentLevel = 0 };
    }
}
