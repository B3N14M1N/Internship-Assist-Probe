using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ScriptableObjectsManager : MonoBehaviour
{
    [Inject]
    public List<GameItemScriptableObject> items;

    private static List<GameItemScriptableObject> staticList;


    public void Awake()
    {
        staticList = items;
        Debug.Log(staticList.Count);
    }
    public static GameItemData GetGameItemData(int gameItemId)
    {
        Debug.Log($"Searching for ID: {gameItemId}.");
        for (int i = 0; i < staticList.Count; i++)
        {
            if (staticList[i].item.gameItemId == gameItemId)
                return staticList[i].item;
        }
        Debug.Log("Not Found");
        return null;
    }
}
