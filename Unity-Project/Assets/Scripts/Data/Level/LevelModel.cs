using System;
using System.Collections.Generic;
using UnityEngine;

using GameItemHolders;

[Serializable]
public class LevelModel
{
    public int Level;
    public float LevelTime;
    public List<Container> Containers;

    public static LevelModel ToModel(List<IContainer> containersMono, int level, float levelTime)
    {
        Debug.Log("Converting Level to LevelModel");
        var model = new LevelModel();

        model.Level = level;
        model.LevelTime = levelTime;

        model.Containers = new List<Container>();
        foreach (var container in containersMono)
        {
            model.Containers.Add(container.Container);
        }
        return model;
    }
}