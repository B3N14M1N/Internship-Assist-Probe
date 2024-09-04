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
    public Vector3 layerPosition;
    public int layerOrder;
    public LayerStatus status;
    public Slot Slot1;
    public Slot Slot2;
    public Slot Slot3;

    public static Layer ToModel(LayerMono layerMono)
        => new Layer()
        {
            layerPosition = layerMono.layerPosition,
            layerOrder = layerMono.layerOrder,
            status = layerMono.status,
            Slot1 = layerMono.Slot1 == null ? Slot.Empty : layerMono.Slot1.Slot,
            Slot2 = layerMono.Slot2 == null ? Slot.Empty : layerMono.Slot2.Slot,
            Slot3 = layerMono.Slot3 == null ? Slot.Empty : layerMono.Slot3.Slot,
        };
}