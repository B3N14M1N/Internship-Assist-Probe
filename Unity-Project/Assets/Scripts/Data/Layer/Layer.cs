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
        public int MaxSlots;
        public Slot[] Slots;

        public static Layer Empty => new Layer();
    }

    public interface ILayer
    {
        #region Fields
        Layer Layer { get; set; }
        ISlot[] Slots { get; set; }
        LayerStatus Status { get;}

        int MaxSlots { get; }
        bool IsEmpty { get; }
        bool IsFull { get; }
        bool IsCombinable { get; }
        #endregion

        #region Methods
        void RemoveLayer();
        void ClearLayer(bool removeSlots = true);
        void SetStatus(LayerStatus status);
        void RemoveSlot(ISlot slot);
        #endregion
    }
}