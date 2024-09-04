using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ScriptableObjectsManager : MonoBehaviour
{
    [Inject]
    private static List<GameItemScriptableObject> items;

    public static GameItemData GetGameItemData(int gameItemId)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item.gameItemId == gameItemId)
                return items[i].item;
        }
        return null;
    }
}
