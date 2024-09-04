using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

public class LevelBuilderManager : MonoBehaviour
{
    #region FIELDS

    [Inject, HideInInspector]
    public LevelManager levelManager;

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
        string json = JsonUtility.ToJson(LevelModel, true);
        File.WriteAllText(Application.persistentDataPath + $"/Level{Level}", json);
        Debug.Log(Application.persistentDataPath + $"/Level{Level}");
        Debug.Log(json);
    }

    public void LoadLevel()
    {
        try
        {
            string json = File.ReadAllText(Application.persistentDataPath + $"/level{Level}");

            var levelModel = JsonUtility.FromJson<LevelModel>(json);

            ResetLevel();
            this.Level = levelModel.level;
            this.LevelTime = levelModel.LevelTime;

            foreach (Container container in levelModel.containers)
            {
                AddContainer(container);
            }

            Debug.Log(json);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
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