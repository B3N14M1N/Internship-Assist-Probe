using System;
using UnityEngine;


[SerializeField]
public enum LayerStatus
{
    hidden = 0,
    front = 1,
    back = 2,
}

[Serializable]
public class Layer
{
    public int layerOrder;
    public int status;
    public Slot Slot1;
    public Slot Slot2;
    public Slot Slot3;

    public static Layer ToModel(LayerMono layerMono)
    {
        //Debug.Log("Converting LayerMono to Layer");
        return new Layer()
        {
            layerOrder = layerMono.layerOrder,
            status = (int)layerMono.status,
            Slot1 = layerMono.Slot1 == null ? Slot.Empty : layerMono.Slot1.Slot,
            Slot2 = layerMono.Slot2 == null ? Slot.Empty : layerMono.Slot2.Slot,
            Slot3 = layerMono.Slot3 == null ? Slot.Empty : layerMono.Slot3.Slot,
        };
    }
}