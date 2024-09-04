using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Container
{
    public Vector3 Position;
    public List<Layer> Layers = new List<Layer>();

    public static Container ToModel(ContainerMono containerMono)
    {

        Debug.Log("Converting ContainerMono to Container");
        var container = new Container() 
        {
            Position = containerMono.transform.localPosition,
            Layers = new List<Layer>()
        };

        foreach (var layer in containerMono.layers)
        {
            container.Layers.Add(layer.Layer);
        }
        return container;
    }
}