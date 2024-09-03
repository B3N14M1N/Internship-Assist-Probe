using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameItemData", menuName = "Scriptable Objects/Game Item", order = 1)]
public class GameItemScriptableObject : ScriptableObject
{
    [SerializeField]
    public GameItemData item;
}
