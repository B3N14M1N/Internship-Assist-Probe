using System.Collections.Generic;
using UnityEngine;

public class LevelBuilderManager : MonoBehaviour, ILevelManager
{
    public void Start()
    {
        Application.targetFrameRate = 30;
    }

    #region FIELDS

    public int Level;
    public float LevelTime;
    public List<IContainer> containersMono = new List<IContainer>();
    
    private LevelModel LevelModel => LevelModel.ToModel(containersMono, Level, LevelTime);

    #endregion

    #region ADD & REMOVE CONTAINERS

    public IContainer AddContainer()
    {
        return AddContainer(new Container());
    }
    public IContainer AddContainer(Container container)
    {
        IContainer newContainer = PrefabsInstanciatorFactory.InitializeNew(container, transform);
        containersMono.Add(newContainer);
        return newContainer;
    }
    public void RemoveContainer(ContainerMono container)
    {
        containersMono.Remove(container);
    }
    #endregion

    #region SAVE & LOAD & RESET

    public void SaveLevel()
    {
        Debug.Log("Starting Saving");
        AssetsManager.AddLevel(LevelModel);
    }

    public void LoadLevel(int level)
    {
        var levelModel = AssetsManager.GetLevelModel(level);
        if (levelModel != null)
        {
            ResetLevel();
            Level = levelModel.level;
            LevelTime = levelModel.LevelTime;

            foreach (Container container in levelModel.containers)
            {
                AddContainer(container);
            }
        }
    }

    public void ResetLevel()
    {
        while (containersMono.Count > 0)
        {
            containersMono[0].RemoveContainer();
        }
    }
    #endregion
}