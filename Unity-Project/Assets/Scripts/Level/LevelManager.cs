using System;
using System.Collections.Generic;
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

    public LevelModel GetLevelModel()
    {
        try
        {
            LevelResourcePathScriptableObject levelResourcePath = kvp[currentLevel];
        }
        catch {
            //Debug.LogException(e);
        }
        return null;
    }
    public void AddLevel(LevelModel model, string path, List<ContainerMono> containers)
    {
        var resourceSO = new LevelResourcePathScriptableObject() { path = path, level = model.level };
        kvp.Add(model.level, resourceSO);
        AssetDatabase.CreateAsset(resourceSO, $"ScriptableObjectsFolderPath/PathLevel{model.level}.asset");
        AssetDatabase.SaveAssets();
    }
}