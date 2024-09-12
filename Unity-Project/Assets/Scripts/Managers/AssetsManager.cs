using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Zenject;

/// <summary>
/// This class manages the game assets
/// </summary>
public class AssetsManager
{
    #region Fields

    /// <summary>
    /// The levels path for adding new levels
    /// </summary>
    private static string LevelPaths { get; set; }

    /// <summary>
    /// Dictionary with all levels paths
    /// </summary>
    private static Dictionary<int, LevelResourcePathScriptableObject> kvpLevels = new Dictionary<int, LevelResourcePathScriptableObject>();

    /// <summary>
    /// Dictionar with all game items | not ok to load all when large/many assets are used... but for 20 items is ok
    /// </summary>
    private static Dictionary<int, GameItemScriptableObject> kvpSO = new Dictionary<int, GameItemScriptableObject>();

    /// <summary>
    /// Dictionary with all prefabs
    /// </summary>
    private static Dictionary<string, GameObject> kvpPrefabs = new Dictionary<string, GameObject>();

    /// <summary>
    /// Dictionary with all the game effects
    /// </summary>
    private static Dictionary<string, EffectsScriptableObject> kvpEffects = new Dictionary<string, EffectsScriptableObject>();
    #endregion

    #region Constructor

    /// <summary>
    /// Populates the static fields with the data loaded from the reeived paths
    /// </summary>
    /// <param name="levelPaths"></param>
    /// <param name="ItemsPath"></param>
    /// <param name="PrefabsPath"></param>
    /// <param name="EffectsPath"></param>
    [Inject]
    public AssetsManager(string levelPaths, string ItemsPath, string PrefabsPath, string EffectsPath)
    {
        LevelPaths = levelPaths;
        foreach (LevelResourcePathScriptableObject resource in Resources.LoadAll<LevelResourcePathScriptableObject>(levelPaths).ToArray())
        {
            kvpLevels.Add(resource.level, resource);
        }
        Debug.Log($"Loaded paths for {kvpLevels.Count} levels.");

        foreach (var item in Resources.LoadAll<GameItemScriptableObject>(ItemsPath).ToList())
        {
            kvpSO.TryAdd(item.item.GameItemId, item);
        }
        Debug.Log($"Loaded {kvpSO.Count} items");


        foreach(var prefab in Resources.LoadAll<GameObject>(PrefabsPath))
        {
            kvpPrefabs.TryAdd(prefab.name, prefab);
        }
        Debug.Log($"Loaded {kvpPrefabs.Count} prefabs");


        foreach (var effect in Resources.LoadAll<EffectsScriptableObject>(EffectsPath))
        {
            kvpEffects.TryAdd(effect.Identificator, effect);
        }
        Debug.Log($"Loaded {kvpEffects.Count} effects");
    }
    #endregion


    #region Methods

    /// <summary>
    /// Adds a new level to the assets | this method is for editor only
    /// </summary>
    /// <param name="levelModel"></param>
    public static void AddLevel(LevelModel levelModel)
    {
        Debug.Log($"Saving Level {levelModel.Level}");
        string json = JsonUtility.ToJson(levelModel, true);
        string jsonPath = $"/Levels/Level{levelModel.Level}.json";
        File.WriteAllText(Application.streamingAssetsPath + jsonPath, json);

#if UNITY_EDITOR
        LevelResourcePathScriptableObject sobj = ScriptableObject.CreateInstance<LevelResourcePathScriptableObject>();
        sobj.level = levelModel.Level;
        sobj.path = jsonPath;
        AssetDatabase.CreateAsset(sobj, $"Assets/Resources/{LevelPaths}/Level{sobj.level}.asset");
        AssetDatabase.SaveAssets();

        kvpLevels.TryAdd(sobj.level, sobj);
#endif
    }

    /// <summary>
    /// Returns the level model for the wanted level
    /// from the streaming assets 
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static LevelModel GetLevelModel(int level)
    {
        if (kvpLevels.TryGetValue(level, out var path))
        {
            try
            {
                var fullPath = Application.streamingAssetsPath + path.path;
                UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(fullPath);
                www.SendWebRequest();
                while (!www.downloadHandler.isDone);

                string jsonString = www.downloadHandler.text;
                return JsonUtility.FromJson<LevelModel>(jsonString);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        Debug.Log($"Level {level} not found.");
        return null;
    }

    /// <summary>
    /// Returns the next level if it exists, else return the same level
    /// </summary>
    /// <param name="currentLevel"></param>
    /// <returns></returns>
    public static int GetNextLevel(int currentLevel)
    {
        if (kvpLevels.TryGetValue(currentLevel+1, out var path))
        {
            return currentLevel + 1;
        }
        return currentLevel;
    }

    /// <summary>
    /// Returns the item data with the requested id
    /// </summary>
    /// <param name="gameItemId"></param>
    /// <returns></returns>
    public static GameItemData GetGameItemData(int gameItemId)
    {
        if (kvpSO.TryGetValue(gameItemId, out GameItemScriptableObject value))
        {
            return value.item;
        }
        Debug.Log($"Game Item with ID: {gameItemId} not found");
        return null;
    }

    /// <summary>
    /// Returns the prefab with the requested prefab name
    /// </summary>
    /// <param name="prefabName"></param>
    /// <returns></returns>
    public static GameObject GetPrefab(string prefabName)
    {
        if (kvpPrefabs.TryGetValue(prefabName, out GameObject value))
        {
            return value;
        }
        Debug.Log($"Prefab with name: {prefabName} not found");
        return null;
    }

    /// <summary>
    /// Returns the effect with the requested identificator
    /// </summary>
    /// <param name="identificator"></param>
    /// <returns></returns>
    public static EffectsScriptableObject GetEffects(string identificator)
    {
        if (kvpEffects.TryGetValue(identificator, out EffectsScriptableObject value))
        {
            return value;
        }
        Debug.Log($"Effects with identificator: {identificator} not found");
        return null;
    }
    #endregion
}