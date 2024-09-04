using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

public class LevelBuilderManager : MonoBehaviour
{
    [Inject]
    public LevelManager levelManager;

    public int Level;
    public float LevelTime;

    public List<ContainerMono> containersMono = new List<ContainerMono>();

    private LevelModel LevelModel => LevelModel.ToModel(containersMono, Level, LevelTime);
    public ContainerMono AddContainer()
    {
        var prefab = Resources.Load("Prefabs/Container") as GameObject;
        var newContainer = Instantiate(prefab).GetComponent<ContainerMono>();
        newContainer.transform.parent = transform;
        containersMono.Add(newContainer);
        return newContainer;
    }
    public ContainerMono AddContainer(Container container)
    {
        var prefab = Resources.Load("Prefabs/Container") as GameObject;
        var newContainer = Instantiate(prefab).GetComponent<ContainerMono>();
        newContainer.transform.parent = transform;
        newContainer.transform.position = container.Position;
        foreach (Layer layer in container.Layers)
        {
            newContainer.AddLayer(layer);
        }
        containersMono.Add(newContainer);
        return newContainer;
    }
    public void RemoveContainer(ContainerMono container)
    {
        containersMono.Remove(container);
    }
    public void SaveLevel()
    {
        string json = JsonUtility.ToJson(LevelModel);
        File.WriteAllText(Application.persistentDataPath + "/level1", json);
        Debug.Log(Application.persistentDataPath + "/level1");
        Debug.Log(json);
    }

    public void LoadLevel()
    {
            string json = File.ReadAllText(Application.persistentDataPath + "/level1");

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

    public void ResetLevel()
    {
        while (containersMono.Count > 0) 
        {
            containersMono[0].RemoveContainer();
        }
    }
}