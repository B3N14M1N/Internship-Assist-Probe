using System;
using UnityEngine;

namespace GameItemHolders
{
    [Serializable]
    public class Slot
    {
        public int gameItemId;
        public static Slot Empty => new Slot() { gameItemId = 0 };
    }

    public interface ISlot
    {
        #region Fields
        Slot Slot { get; set; }
        Vector3 SlotPosition { get; set; }

        int ItemId { get; set; }
        #endregion

        #region Methods
        void RemoveSlot();
        void ClearSlot();
        void LoadData(GameItemData data);
        void SetStatus(bool render = false, bool draggable = false, int renderOrder = 0, Color? renderColor = null);
        #endregion
    }
}