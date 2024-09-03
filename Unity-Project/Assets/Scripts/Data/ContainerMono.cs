using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class ContainerMono : MonoBehaviour
{
    [SerializeField]
    public Container container;
    [SerializeField]
    public Container test;

    public void Awake()
    {
        GetAllLayer();
    }

    private void GetAllLayer()
    {
        container = new Container();
        foreach (LayerMono layer in GetComponentsInChildren<LayerMono>())
            container.Layers.Add(layer.layer);
    }

    public void SaveToJSON()
    {
        GetAllLayer();
        string json = JsonUtility.ToJson(container);
        File.WriteAllText(Application.persistentDataPath + "/level1", json);
        Debug.Log(Application.persistentDataPath + "/level1");
        Debug.Log(json);
    }
    public void ReadFromJSON()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/level1");

        test = JsonUtility.FromJson<Container>(json);
        Debug.Log(json);
    }
}

[Serializable]
public class Container
{
    [SerializeField]
    public List<Layer> Layers = new List<Layer>();
}