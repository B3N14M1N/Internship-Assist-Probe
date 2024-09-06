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
                (layerMono as MonoBehaviour).name = "Layer " + layer.LayerOrder;
                Layers.Insert(0, layerMono);
            }

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


    #region CREATE & REMOVE CONTAINER

    public void RemoveContainer()
    {
        ClearContainer();
        transform.GetComponentInParent<ILevelManager>()?.RemoveContainer(this);

        DestroyImmediate(gameObject);
    }

    public void ClearContainer()
    {
        if (Layers != null)
        {
            while (Layers.Count > 0)
            {
                Layers[0].RemoveLayer();
            }
        }
    }
    #endregion

    #region ADD & REMOVE LAYER


    public ILayer AddLayer(Layer layer = null)
    {
        layer ??= new Layer(); 
        var newLayer = PrefabsInstanciatorFactory.InitializeNew(layer, transform);
        PushLayers();
        Layers.Insert(0, newLayer);

        return newLayer;
    }

    public void RearangeLayers()
    {
        List<ILayer> newLayers = new List<ILayer>();

    }

    private void PushLayers(int index = 0)
    {
        foreach (var layer in Layers)
        {
            layer.PushLayer();
        }
    }

    private void PullLayers(int index = 0)
    {
        for (int i = index; i < Layers.Count; i++)
        {
            Layers[i].PullLayer();
        }
        foreach (var layer in Layers)
        {
            layer.PullLayer();
        }
    }
    public void RemoveLayer(ILayer layer)
    {
        var index = Layers.IndexOf(layer);
        if (index != -1)
        {
            Layers.Remove(layer);
            PullLayers(index);
        }
    }
    #endregion
}
