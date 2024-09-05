using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ContainerMono : MonoBehaviour
{
    #region FIELDS

    public List<LayerMono> layers = new List<LayerMono>();
    public LayerMono ActiveLayer => layers[0];
    public Container Container => Container.ToModel(this);
    #endregion


    #region CREATE & REMOVE CONTAINER

    public static ContainerMono InitializeNew(Container container, Transform parent)
    {
        if (container == null)
            return null;

        var prefab = LevelManager.GetPrefab("Container");

        if (prefab == null)
            return null;

        var newContainer = Instantiate(prefab).GetComponent<ContainerMono>();
        newContainer.transform.parent = parent;
        newContainer.transform.localPosition = container.Position;

        foreach (Layer layer in container.Layers)
        {
            LayerMono layerMono = LayerMono.InitializeNew(layer, newContainer.transform);
            layerMono.name = "Layer " + layer.layerOrder;
            newContainer.layers.Insert(0, layerMono);
        }
        return newContainer;
    }
    public void RemoveContainer()
    {
        while (layers.Count > 0)
        {
            layers[0].RemoveLayer();
        }

        transform.GetComponentInParent<ILevelManager>()?.RemoveContainer(this);

        DestroyImmediate(gameObject);
    }
    #endregion

    #region ADD & REMOVE LAYER

    public LayerMono AddLayer()
    {
        return AddLayer(new Layer());
    }

    public LayerMono AddLayer(Layer layer)
    {
        var newLayer = LayerMono.InitializeNew(layer, transform);
        PushLayers();
        layers.Insert(0, newLayer);

        return newLayer;
    }

    public void RearangeLayers()
    {
        List<LayerMono> newLayers = new List<LayerMono>();

    }

    private void PushLayers(int index = 0)
    {
        foreach (var layer in layers)
        {
            layer.PushLayer();
        }
    }

    private void PullLayers(int index = 0)
    {
        for (int i = index; i < layers.Count; i++)
        {
            layers[i].PullLayer();
        }
        foreach (var layer in layers)
        {
            layer.PullLayer();
        }
    }
    public void RemoveLayer(LayerMono layer)
    {
        var index = layers.IndexOf(layer);
        if (index != -1)
        {
            layers.Remove(layer);
            PullLayers(index);
        }
    }
    #endregion
}
