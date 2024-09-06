using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Container
{
    public Vector3 Position;
    public List<Layer> Layers = new List<Layer>();

    public static Container Empty => new Container();
}

public interface IContainer
{
    List<ILayer> Layers { get; set; }
    Container Container { get; set; }

    bool IsEmpty { get; }
    void RemoveContainer();
    void ClearContainer();
    ILayer AddLayer(Layer layer = null);
    void RemoveLayer(ILayer layer = null);
    void RearangeLayers(bool putEmptyLayersBehind = false);
}