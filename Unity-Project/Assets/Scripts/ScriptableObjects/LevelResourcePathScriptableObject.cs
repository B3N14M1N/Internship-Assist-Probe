using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelResourcePathScriptableObject", menuName = "Scriptable Objects/Level Resource Path", order = 1)]
public class LevelResourcePathScriptableObject : ScriptableObject
{
    public int level;
    public string path;
}