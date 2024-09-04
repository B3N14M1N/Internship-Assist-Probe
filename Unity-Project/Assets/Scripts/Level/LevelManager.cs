using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Zenject;

public class LevelManager
{
    private int currentLevel;
    private string levelPaths;
    private string progressPath;

    private Dictionary<int, LevelResourcePathScriptableObject> kvp = new Dictionary<int, LevelResourcePathScriptableObject>();

    [Inject]
    public LevelManager(string levelPaths, string progressPath)
    {
        this.levelPaths = levelPaths;
        this.progressPath = progressPath;

        Debug.Log($"Progress path:{progressPath}\nLevelsPaths:{levelPaths}");

        // read current progress

        foreach (LevelResourcePathScriptableObject resource in Resources.LoadAll<LevelResourcePathScriptableObject>(levelPaths).ToArray())
        {
            kvp.Add(resource.level, resource);
        }
        Debug.Log($"Loaded paths for {kvp.Count} levels.");
    }

    public LevelModel GetLevelModel(int level)
    {
        try
        {
            string json = File.ReadAllText(kvp[level].path);

            return JsonUtility.FromJson<LevelModel>(json);
        }
        catch(Exception e) {
            Debug.LogException(e);
        }
        return null;
    }
    public void AddLevel(LevelModel levelModel)
    {
        Debug.Log($"Saving Level {levelModel.level}");
        string json = JsonUtility.ToJson(levelModel, true);
        string jsonPath = Application.dataPath + $"/Resources/Levels/Level{levelModel.level}.json";
        File.WriteAllText(jsonPath, json);

        LevelResourcePathScriptableObject sobj = ScriptableObject.CreateInstance<LevelResourcePathScriptableObject>();
        sobj.level = levelModel.level;
        sobj.path = jsonPath;
        AssetDatabase.CreateAsset(sobj, $"Assets/Resources/LevelPaths/Level{sobj.level}.asset");
        AssetDatabase.SaveAssets();

        kvp.TryAdd(sobj.level, sobj);
    }
}