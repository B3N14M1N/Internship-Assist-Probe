using System.Collections.Generic;
using UnityEngine;

using GameItemHolders;

/// <summary>
/// The level difficulty - value is probability to get the same item on the same layer
/// </summary>
public enum LevelDifficultyMode
{
    Normal = 70,
    Hard = 50,
    SuperHard = 30
}

public class LevelBuilderManager : MonoBehaviour, ILevelManager
{
    public void Start()
    {
        Application.targetFrameRate = 30;
    }

    #region FIELDS

    /// <summary>
    /// The level to be saved
    /// </summary>
    public int Level;

    /// <summary>
    /// The allocated level time in seconds
    /// </summary>
    public float LevelTime;

    /// <summary>
    /// The number of items to populate the level with
    /// </summary>
    public int NumberOfItems { get; set; }
    private System.Random rnd;

    /// <summary>
    /// The difficulty of the level and how the items are placed.
    /// </summary>
    public LevelDifficultyMode Difficulty = LevelDifficultyMode.Normal;

    /// <summary>
    /// The container list
    /// </summary>
    public List<IContainer> containersMono = new List<IContainer>();
    
    /// <summary>
    /// Returns the level data model to be saved
    /// </summary>
    private LevelModel LevelModel => LevelModel.ToModel(containersMono, Level, LevelTime);
    #endregion

    #region ADD & REARRANGE & REMOVE CONTAINERS

    /// <summary>
    /// Adds a new container component to the level
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    public IContainer AddContainer() => AddContainer(new Container());
    public IContainer AddContainer(Container container)
    {
        IContainer newContainer = PrefabsInstanciatorFactory.InitializeNew(container, transform);
        containersMono.Add(newContainer);
        return newContainer;
    }

    /// <summary>
    /// Removes the requested container component from the 
    /// </summary>
    /// <param name="container"></param>
    public void RemoveContainer(IContainer container)
    {
        containersMono.Remove(container);
    }

    /// <summary>
    /// Rearrange the layers foreach container
    /// </summary>
    public void RearrangeContainers()
    {
        Debug.Log("Rearranging Containers layers. Putting empty layers behind.");
        foreach (var container in containersMono)
        {
            container.RearrangeLayers(true);
        }
    }
    #endregion

    #region POPULATE CONTAINERS

    /// <summary>
    /// Validates if there are enaugh slots for the requested amount of items to be placed in containers
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Clear all items from the slots
    /// </summary>
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

    /// <summary>
    /// Validates and populates the containers withe the selected difficulty and number of items
    /// </summary>
    public void PopulateContainers()
    {
        if (!ValidatePossibleGeneration())
        {
            Debug.LogError("Not enaugh slots!");
        }
        else
        {
            // clears the slots
            ClearAllItems();

            // new random
            rnd = new System.Random((int)Time.time);

            // selects the items
            var queue = new Queue<int>();
            for (int id = 1; id <= NumberOfItems; id++)
            {
                /// better check if id exists....
                /// but for now its ok
                queue.Enqueue(id);
                queue.Enqueue(id);
                queue.Enqueue(id);
            }

            // put all items in random slots
            while (queue.Count > 0)
            {
                // gets a random container from the level
                var container = GetRandomEmptyContainer();
                // if container is valid go on
                if (container != null && !container.IsFull)
                {
                    // gets a random layer from the container
                    var layer = GetRandomEmptyLayer(container);
                    // if layer is valid go on
                    if(layer != null && !layer.IsFull)
                    {
                        // gets a random slot from the layer
                        var slot = GetRandomEmptySlot(layer);
                        int id = queue.Peek();
                        // if the slot is valid go on
                        if(slot != null)
                        {
                            // loads the data in the slot
                            slot.LoadData(AssetsManager.GetGameItemData(queue.Dequeue()));
                        }
                        // set the next item on the same layer, if its not empty, based on probability of the selected difficutly
                        if (!layer.IsFull && rnd.Next(0, 100) <= (int)Difficulty && queue.Count > 0 && id == queue.Peek())
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
            // rearrange the containers
            RearrangeContainers();
        }
    }

    /// <summary>
    /// returns a container that is not full and has empty slots
    /// </summary>
    /// <returns></returns>
    private IContainer GetRandomEmptyContainer()
    {
        if(containersMono != null && containersMono.Count > 0)
        {
            return containersMono[rnd.Next(0, containersMono.Count)];
        }
        return null;
    }

    /// <summary>
    /// Returns a layer that is not full and has empty slots from the selected container
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    private ILayer GetRandomEmptyLayer(IContainer container)
    {
        if (container != null && container.Layers != null && !container.IsFull)
        {
            return container.Layers[rnd.Next(0, container.Layers.Count)];
        }
        return null;
    }

    /// <summary>
    /// Returns an empty slot from the selected layer
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Saves the level model
    /// </summary>
    public void SaveLevel()
    {
        Debug.Log("Starting Saving");
        RearrangeContainers();
        AssetsManager.AddLevel(LevelModel);
    }

    /// <summary>
    /// Loads the level
    /// </summary>
    /// <param name="level"></param>
    public void LoadLevel(int level)
    {
        var levelModel = AssetsManager.GetLevelModel(level);
        if (levelModel != null)
        {
            ResetLevel();
            Level = levelModel.Level;
            LevelTime = levelModel.LevelTime;

            foreach (Container container in levelModel.Containers)
            {
                AddContainer(container);
            }
        }
    }

    /// <summary>
    /// Clears the level builder
    /// </summary>
    public void ResetLevel()
    {
        while (containersMono.Count > 0)
        {
            containersMono[0].RemoveContainer();
        }
    }
    #endregion
}