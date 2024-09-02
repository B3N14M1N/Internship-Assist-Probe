using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int currentLevel;
    private string ScriptableObjectsFolderPath = "LevelPaths";

    private Dictionary<int, LevelResourcePath> kvp = new Dictionary<int, LevelResourcePath>();

    private static LevelManager _instance;
    public static LevelManager Instance 
    { 
        get => _instance; 
        private set { _instance = value; }  
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        foreach (LevelResourcePath resource in Resources.LoadAll<LevelResourcePath>(ScriptableObjectsFolderPath).ToArray())
        {
            kvp.Add(resource.level, resource);
        }
        Debug.Log($"Loaded paths for {kvp.Count} levels.");
        DontDestroyOnLoad(gameObject);

    }

    public LevelResourcePath GetLevelResourcePath()
    {
        return kvp[currentLevel];
    }
    public void AddLevel(int level, string path)
    {
        var resourceSO = new LevelResourcePath() { path = path, level = level };
        kvp.Add(level, resourceSO);
        AssetDatabase.CreateAsset(resourceSO, $"ScriptableObjectsFolderPath/PathLevel{level}.asset");
        AssetDatabase.SaveAssets();
    }

}

[CreateAssetMenu(fileName ="LevelResourcePath", menuName = "Scriptable Objects/Level Resource Path", order = 1)]
public class LevelResourcePath: ScriptableObject
{
    public int level;
    public string path;
}
