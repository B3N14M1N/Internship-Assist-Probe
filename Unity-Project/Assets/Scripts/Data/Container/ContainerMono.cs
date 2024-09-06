using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ContainerMono : MonoBehaviour, IContainer
{
    #region FIELDS
    public List<ILayer> Layers { get; set; }
    public Container Container
    {
        get
        {
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
            RearangeLayers(true);
        }
    }

    public bool IsEmpty
    {
        get
        {
            if (Layers == null)
                return false;
            foreach(var layer in Layers)
            {
                if ((layer.Status & LayerStatus.Empty) == 0)
                    return false;
            }
            return true;
        }
    }
    #endregion


    #region CLEAR & REMOVE CONTAINER
    public void ClearContainer()
    {
        while (Layers?.Count > 0)
        {
            Layers[0].RemoveLayer();
        }
    }

    public void RemoveContainer()
    {
        ClearContainer();
        transform.GetComponentInParent<ILevelManager>()?.RemoveContainer(this);

        DestroyImmediate(gameObject);
    }

    #endregion

    #region ADD & REARANGE & REMOVE LAYERS


    public ILayer AddLayer(Layer layer = null)
    {
        layer ??= new Layer(); 
        var newLayer = PrefabsInstanciatorFactory.InitializeNew(layer, transform);
        (newLayer as MonoBehaviour).name = $"Layer {Layers.Count}";
        newLayer.SetStatus(LayerStatus.Empty);
        Layers.Add(newLayer);

        return newLayer;
    }

    public void RearangeLayers(bool putEmptyLayersBehind = false)
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
            LayerStatus newStatus = Layers[i].IsEmpty ? LayerStatus.Empty : LayerStatus.Hidden;
            if (i == 0)
                newStatus = LayerStatus.Front;
            if (i == 1)
                newStatus = LayerStatus.Back;

            Layers[i].SetStatus(newStatus);
            (Layers[i] as MonoBehaviour).name = $"Layer {i}";
        }
    }

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
