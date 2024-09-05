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
    public List<ContainerMono> containersMono = new List<ContainerMono>();
    
    private LevelModel LevelModel => LevelModel.ToModel(containersMono, Level, LevelTime);

    #endregion

    #region ADD & REMOVE CONTAINERS

    public ContainerMono AddContainer()
    {
        return AddContainer(new Container());
    }
    public ContainerMono AddContainer(Container container)
    {
        ContainerMono newContainer = ContainerMono.InitializeNew(container, transform);
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
        LevelManager.AddLevel(LevelModel);
    }

    public void LoadLevel()
    {
        var levelModel = LevelManager.GetLevelModel(Level);
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