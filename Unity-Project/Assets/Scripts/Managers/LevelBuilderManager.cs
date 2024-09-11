using System.Collections.Generic;
using UnityEngine;

using GameItemHolders;

public class LevelBuilderManager : MonoBehaviour, ILevelManager
{
    public void Start()
    {
        Application.targetFrameRate = 30;
    }

    #region FIELDS

    public int Level;
    public float LevelTime;
    public int NumberOfItems { get; set; }
    public int Seed;
    private System.Random rnd;
    public LevelDifficultyMode Difficulty;

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

    public void RearangeContainers()
    {
        Debug.Log("Rearranging containers layers. Putting empty layers behind.");
        foreach (var container in containersMono)
        {
            container.RearangeLayers(true);
        }
    }
    #endregion

    #region POPULATE CONTAINERS

    public enum LevelDifficultyMode
    {
        Normal = 70,
        Hard = 50,
        SuperHard = 30
    }
    public bool ValidatePossibleGeneration()
    {
        int numberOfLayers = 0;
        foreach(var container in containersMono)
        {
            if(container!=null && container.Layers!=null)
                numberOfLayers += container.Layers.Count;
        }

        return numberOfLayers > NumberOfItems;
    }


    public void ClearAllItems()
    {
        foreach (var container in containersMono)
        {
            foreach(var layer in container.Layers)
            {
                layer.ClearLayer(false);
            }
        }
    }

    public void PopulateContainers()
    {
        if (!ValidatePossibleGeneration())
        {
            Debug.LogError("Not enaugh slots!");
        }
        else
        {
            ClearAllItems();
            rnd = new System.Random((int)Time.time);
            var queue = new Queue<int>();
            for (int id = 1; id <= NumberOfItems; id++)
            {
                /// better check if id exists....
                /// but for now its ok
                queue.Enqueue(id);
                queue.Enqueue(id);
                queue.Enqueue(id);
            }

            while (queue.Count > 0)
            {
                var container = GetRandomEmptyContainer();
                if (container != null && !container.IsFull)
                {
                    var layer = GetRandomEmptyLayer(container);
                    if(layer != null && !layer.IsFull)
                    {
                        var slot = GetRandomEmptySlot(layer);
                        if(slot != null)
                        {
                            slot.LoadData(AssetsManager.GetGameItemData(queue.Dequeue()));
                        }
                        if (!layer.IsFull && rnd.Next(0, 100) <= (int)Difficulty && queue.Count > 0)
                        {
                            slot = GetRandomEmptySlot(layer);
                            if (slot != null)
                            {
                                slot.LoadData(AssetsManager.GetGameItemData(queue.Dequeue()));
                            }
                        }
                    }
                }
            }
            RearangeContainers();
        }
    }
    private IContainer GetRandomEmptyContainer()
    {
        if(containersMono != null && containersMono.Count > 0)
        {
            return containersMono[rnd.Next(0, containersMono.Count)];
        }
        return null;
    }

    private ILayer GetRandomEmptyLayer(IContainer container)
    {
        if (container != null && container.Layers != null && !container.IsFull)
        {
            return container.Layers[rnd.Next(0, container.Layers.Count)];
        }
        return null;
    }
    private ISlot GetRandomEmptySlot(ILayer layer)
    {
        if (layer != null && layer.Slots != null && !layer.IsFull)
        {
            int slot = rnd.Next(0, layer.Slots.Length);
            while (layer.Slots[slot]?.ItemId != 0)
                slot = rnd.Next(0, layer.Slots.Length);
            return layer.Slots[slot];
        }
        return null;
    }
    #endregion

    #region SAVE & LOAD & RESET

    public void SaveLevel()
    {
        Debug.Log("Starting Saving");
        RearangeContainers();
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