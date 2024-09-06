using System;
using UnityEngine;


[SerializeField]
public enum LayerStatus
{
    Empty = 1,
    Front = 2,
    Back = 4,
    Hidden = 8,
}

[Serializable]
public class Layer
{
    public LayerStatus Status;
    public int MaxSlots;
    public Slot[] Slots;

    public static Layer Empty => new Layer();
}

public interface ILayer
{
    void SetStatus(LayerStatus status);
    Layer Layer { get; set; }
    LayerStatus Status { get; set; }
    int MaxSlots { get; }
    ISlot[] Slots {  get; set; }
    bool IsEmpty {  get; }
    void RemoveLayer();
    void ClearLayer();
    void RemoveSlot(ISlot slot);
}