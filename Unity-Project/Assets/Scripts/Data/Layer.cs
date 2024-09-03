using System;
using UnityEngine;


public enum LayerStatus
{
    hidden = 0,
    front = 1,
    back = 2,
}

[Serializable]
public class Layer
{
    [SerializeField]
    public Vector3 layerPosition;
    [SerializeField]
    public int layerOrder;
    [SerializeField]
    public LayerStatus status;
    [SerializeField]
    public Slot Slot1;
    [SerializeField]
    public Slot Slot2;
    [SerializeField]
    public Slot Slot3;
}