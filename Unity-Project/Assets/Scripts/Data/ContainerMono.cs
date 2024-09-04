using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ContainerMono : MonoBehaviour
{
    private static Vector3 layerPosition = new Vector3(0, 2, 0);

    public List<LayerMono> layers = new List<LayerMono>();

    public Container Container => Container.ToModel(this);

    public LayerMono AddLayer()
    {

        var prefab = Resources.Load("Prefabs/Layer") as GameObject;
        var newLayer = Instantiate(prefab).GetComponent<LayerMono>();
        newLayer.transform.parent = transform;
        newLayer.layerPosition = layerPosition;
        PushLayers();
        layers.Insert(0, newLayer);
        return newLayer;
    }

    public LayerMono AddLayer(Layer layer)
    {
        var prefab = Resources.Load("Prefabs/Layer") as GameObject;
        var newLayer = Instantiate(prefab).GetComponent<LayerMono>();
        newLayer.transform.parent = transform;
        newLayer.layerPosition = layerPosition;
        newLayer.LoadLayer(layer);
        PushLayers();
        layers.Insert(0, newLayer);
        return newLayer;
    }
    public void RemoveContainer()
    {
        while (layers.Count > 0)
        {
            layers[0].RemoveLayer();
        }

        transform.parent.GetComponent<LevelBuilderManager>().RemoveContainer(this);
        while(transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0));
        DestroyImmediate(gameObject);
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
}
