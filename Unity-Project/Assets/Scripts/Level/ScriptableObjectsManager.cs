using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Zenject;

public class ScriptableObjectsManager : MonoBehaviour
{
    [Inject]
    public List<GameItemScriptableObject> items;

    private static Dictionary<int , GameItemScriptableObject> kvp;

    public void Awake()
    {
        kvp = new Dictionary<int , GameItemScriptableObject>();
        foreach (var item in items)
        {
            kvp.TryAdd(item.item.gameItemId, item);
        }
        Debug.Log($"Loaded {kvp.Count} items");
    }
    public static GameItemData GetGameItemData(int gameItemId)
    {
        if(kvp.TryGetValue(gameItemId, out GameItemScriptableObject value))
        {
            return value.item;
        }
        Debug.Log($"Game Item with ID: {gameItemId}. Not Found");
        return null;
    }
}
