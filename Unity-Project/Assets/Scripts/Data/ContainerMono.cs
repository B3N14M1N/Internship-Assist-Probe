using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ContainerMono : MonoBehaviour
{
    #region FIELDS

    private static Vector3 layerPosition = new Vector3(0, 2, 0);
    public List<LayerMono> layers = new List<LayerMono>();
    public LayerMono ActiveLayer => layers[0];
    public Container Container => Container.ToModel(this);
    #endregion


    #region CREATE & REMOVE CONTAINER

    public static ContainerMono InitializeNew(Container container, Transform parent)
    {
        if (container == null)
            return null;

        var prefab = Resources.Load("Prefabs/Container") as GameObject;
        var newContainer = Instantiate(prefab).GetComponent<ContainerMono>();
        newContainer.transform.parent = parent;
        newContainer.transform.localPosition = container.Position;

        foreach (Layer layer in container.Layers)
        {
            //newContainer.PushLayers();
            newContainer.layers.Insert(0, LayerMono.InitializeNew(layer,newContainer.transform));
        }
        return newContainer;
    }
    public void RemoveContainer()
    {
        while (layers.Count > 0)
        {
            layers[0].RemoveLayer();
        }

        transform.parent.GetComponent<LevelBuilderManager>().RemoveContainer(this);

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
