using System;
using UnityEngine;

public class LevelInitializator : MonoBehaviour
{
    private LevelResourcePath levelResourcePath;
    private LevelModel levelModel;
    public void Awake()
    {
        levelResourcePath = LevelManager.Instance.GetLevelResourcePath();
        GetLevelModelFromPath(levelResourcePath.path);
    }

    public void GetLevelModelFromPath(string path)
    {
        // read from JSON
    }
    public void InitializeScene()
    {
        levelModel.DebugLog();
    }

}

[Serializable]
public class LevelModel
{
    public int level;
    [SerializeField]
    public float LevelTime;

    public void DebugLog()
    {
        Debug.Log($"Level:{level}\nLevelTime:{LevelTime}");
    }
}