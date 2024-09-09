using System;
using UnityEngine;


namespace GameItemHolders
{
    [SerializeField]
    public enum LayerStatus
    {
        Front = 1,
        Back = 2,
        Hidden = 4,
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
        ISlot[] Slots { get; set; }
        bool IsEmpty { get; }
        bool IsCombinable { get; }
        void RemoveLayer();
        void ClearLayer(bool removeSlots = true);
        void RemoveSlot(ISlot slot);
    }
}