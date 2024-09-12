using System;
using System.Collections.Generic;
using UnityEngine;

using GameItemHolders;

[Serializable]
public class ContainerMono : MonoBehaviour, IContainer
{
    #region FIELDS
    public List<ILayer> Layers { get; set; }

    /// <summary>
    /// Gets/sets the component data from/to a data class for saving/loading
    /// </summary>
    public Container Container
    {
        get
        {
            // Returns a pure data class for saving the Container data
            var container = new Container()
            {
                Position = transform.localPosition,
                Layers = new List<Layer>()
            };

            foreach (var layer in Layers)
            {
                container.Layers.Add(layer.Layer);
            }
            return container;
        }
        // loads the data from a Container class to this monobehaviour
        // clear the previous data and instantiates the components (Layers, Slots)
        set
        {
            value ??= Container.Empty;
            ClearContainer();
            Layers = new List<ILayer>();
            foreach (Layer layer in value.Layers)
            {
                ILayer layerMono = PrefabsInstanciatorFactory.InitializeNew(layer, transform);
                Layers.Add(layerMono);
            }
            RearrangeLayers(true);
        }
    }

    /// <summary>
    /// Returns true if all layers are empty or the container doesn't contain any layers
    /// </summary>
    public bool IsEmpty
    {
        get
        {
            if (Layers == null)
                return true;
            foreach(var layer in Layers)
            {
                if (!layer.IsEmpty)
                    return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Returns true if there is not even a single slot empty 
    /// </summary>
    public bool IsFull
    {
        get
        {
            if (Layers == null)
                return true;
            foreach (var layer in Layers)
            {
                if (!layer.IsFull)
                    return false;
            }
            return true;
        }
    }
    #endregion


    #region CLEAR & REMOVE CONTAINER


    /// <summary>
    /// Clears the container based on different settings
    /// </summary>
    /// <param name="keepTopLayer">Keeps at least one layer (the first one in the list)</param>
    /// <param name="clearOnlyEmpty">Keeps only the layers that are not empty</param>
    public void ClearContainer(bool keepTopLayer = false, bool clearOnlyEmpty = false)
    {
        if (Layers != null)
        {
            for (int i = (keepTopLayer ? 1 : 0); i < Layers.Count; i++)
            {
                if (Layers[i] != null && 
                    (!clearOnlyEmpty || (clearOnlyEmpty && Layers[i].IsEmpty)))
                {
                    Layers[i].RemoveLayer();
                    i--;
                }
            }
        }
    }

    /// <summary>
    /// Remove this container, clears all layers and slots 
    /// and if a manager has a reference to this, removes it
    /// then destroys this
    /// </summary>
    public void RemoveContainer()
    {
        ClearContainer();
        transform.GetComponentInParent<ILevelManager>()?.RemoveContainer(this);

        DestroyImmediate(gameObject);
    }

    #endregion

    #region ADD & REARANGE & REMOVE LAYERS

    /// <summary>
    /// Adds a layer in the back to this container.
    /// Sets the status based on the Position in the layer
    /// </summary>
    /// <param name="layer">If this is null adds a new empty layer</param>
    /// <returns></returns>
    public ILayer AddLayer(Layer layer = null)
    {
        layer ??= new Layer(); 
        var newLayer = PrefabsInstanciatorFactory.InitializeNew(layer, transform);
        (newLayer as MonoBehaviour).name = $"Layer {Layers.Count}";
        newLayer.SetStatus(Layers.Count == 0 ? LayerStatus.Front : Layers.Count == 1 ? LayerStatus.Back : LayerStatus.Hidden);
        Layers.Add(newLayer);

        return newLayer;
    }

    /// <summary>
    /// Rearanges the layers in the list, updating the status and names for each layer
    /// </summary>
    /// <param name="putEmptyLayersBehind">If true, the empty layers are put in the back (end of list)</param>
    public void RearrangeLayers(bool putEmptyLayersBehind = false)
    {
        if (putEmptyLayersBehind)
        {
            List<ILayer> newLayers = new List<ILayer>();
            int lastNotEmpty = 0;
            foreach (var layer in Layers)
            {
                if (layer.IsEmpty)
                {
                    newLayers.Add(layer);
                }
                else
                {
                    newLayers.Insert(lastNotEmpty++, layer);
                }
            }
            Layers = newLayers;
        }
        //Update status & rename Layers
        for (int i = 0; i < Layers.Count; i++)
        {
            LayerStatus newStatus = LayerStatus.Hidden;
            if (i == 0)
                newStatus = LayerStatus.Front;
            if (i == 1)
                newStatus = LayerStatus.Back;

            Layers[i].SetStatus(newStatus);
            (Layers[i] as MonoBehaviour).name = $"Layer {i}";
        }
    }

    /// <summary>
    /// Removes a referenced layer from the list
    /// </summary>
    /// <param name="layer"></param>
    public void RemoveLayer(ILayer layer)
    {
        var index = Layers.IndexOf(layer);
        if (index != -1)
        {
            Layers.Remove(layer);
        }
    }
    #endregion
}
