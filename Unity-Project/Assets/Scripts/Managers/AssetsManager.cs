using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Zenject;

public class AssetsManager
{
    private static Dictionary<int, LevelResourcePathScriptableObject> kvpLevels = new Dictionary<int, LevelResourcePathScriptableObject>();

    private static Dictionary<int, GameItemScriptableObject> kvpSO = new Dictionary<int, GameItemScriptableObject>();

    private static Dictionary<string, GameObject> kvpPrefabs = new Dictionary<string, GameObject>();

    private static Dictionary<string, EffectsAndAnimationsScriptableObject> kvpEffects = new Dictionary<string, EffectsAndAnimationsScriptableObject>();

    [Inject]
    public AssetsManager(string progressPath, string levelPaths, string ItemsPath, string PrefabsPath, string EffectsPath)
    {
        // read current progress

        foreach (LevelResourcePathScriptableObject resource in Resources.LoadAll<LevelResourcePathScriptableObject>(levelPaths).ToArray())
        {
            kvpLevels.Add(resource.level, resource);
        }
        Debug.Log($"Loaded paths for {kvpLevels.Count} levels.");

        foreach (var item in Resources.LoadAll<GameItemScriptableObject>(ItemsPath).ToList())
        {
            kvpSO.TryAdd(item.item.gameItemId, item);
        }
        Debug.Log($"Loaded {kvpSO.Count} items");


        foreach(var prefab in Resources.LoadAll<GameObject>(PrefabsPath))
        {
            kvpPrefabs.TryAdd(prefab.name, prefab);
        }
        Debug.Log($"Loaded {kvpPrefabs.Count} prefabs");


        foreach (var effect in Resources.LoadAll<EffectsAndAnimationsScriptableObject>(EffectsPath))
        {
            kvpEffects.TryAdd(effect.Identificator, effect);
        }
        Debug.Log($"Loaded {kvpEffects.Count} effects");
    }

    public static void AddLevel(LevelModel levelModel)
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

        kvpLevels.TryAdd(sobj.level, sobj);
    }

    public static LevelModel GetLevelModel(int level)
    {
        if (kvpLevels.TryGetValue(level, out var path))
        {
            try
            {
                string json = File.ReadAllText(kvpLevels[level].path);
                return JsonUtility.FromJson<LevelModel>(json);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        Debug.Log($"Level {level} not found.");
        return null;
    }

    public static GameItemData GetGameItemData(int gameItemId)
    {
        if (kvpSO.TryGetValue(gameItemId, out GameItemScriptableObject value))
        {
            return value.item;
        }
        Debug.Log($"Game Item with ID: {gameItemId} not found");
        return null;
    }

    public static GameObject GetPrefab(string prefabName)
    {
        if (kvpPrefabs.TryGetValue(prefabName, out GameObject value))
        {
            return value;
        }
        Debug.Log($"Prefab with name: {prefabName} not found");
        return null;
    }

    public static EffectsAndAnimationsScriptableObject GetEffects(string identificator)
    {
        if (kvpEffects.TryGetValue(identificator, out EffectsAndAnimationsScriptableObject value))
        {
            return value;
        }
        Debug.Log($"Effects with identificator: {identificator} not found");
        return null;
    }
}