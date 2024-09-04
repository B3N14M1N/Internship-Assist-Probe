using System;
using System.Collections.Generic;

[Serializable]
public class LevelModel
{
    public int level;
    public float LevelTime;
    public List<Container> containers;

    public static LevelModel ToModel(List<ContainerMono> containersMono, int level, float LevelTime)
    {
        var model = new LevelModel();

        model.level = level;
        model.LevelTime = LevelTime;

        model.containers = new List<Container>();
        foreach (var container in containersMono)
        {
            model.containers.Add(container.Container);
        }
        return model;
    }
}