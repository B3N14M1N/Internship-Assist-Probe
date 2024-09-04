using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelInitializator : MonoBehaviour
{
    [Inject]
    public LevelManager levelManager;

    private LevelModel levelModel;
    public void Awake()
    {
        levelModel = levelManager.GetLevelModel(0);
        if (levelModel != null)
        {
            InitializeScene();
        }
    }

    public void InitializeScene()
    {
        Debug.Log($"Level: {levelModel.level}");
    }

}
