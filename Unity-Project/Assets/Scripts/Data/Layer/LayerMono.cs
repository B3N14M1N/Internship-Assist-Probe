using UnityEngine;

using GameItemHolders;

public class LayerMono : MonoBehaviour, ILayer
{
    #region FIELDS

    private static readonly Vector3 backroundLayerOffset = new Vector3(0.85f, 0.5f, 0f);
    private static readonly float SlotDistance = 6.5f;
    private static readonly Color backColor = new Color(130f / 256f, 130f / 256f, 130f / 256f, 1f);
    private static readonly Color frontColor = Color.white;
    private static readonly int ThisClassMaxSlots = 3;

    /// <summary>
    /// The status of this layer in the container
    /// Can be in front, second or hidden
    /// </summary>
    public LayerStatus Status { get; private set; }

    /// <summary>
    /// Gets/sets how many slots the layer can have
    /// </summary>
    private int maxSlots;
    public int MaxSlots
    {
        get
        {
            return maxSlots == 0 ? ThisClassMaxSlots : maxSlots;
        }
        private set
        {
            maxSlots = value > 0 ? value : ThisClassMaxSlots;
        }
    }


    /// <summary>
    /// Gets/sets the slots of this layer.
    /// </summary>
    private ISlot[] slots;
    public ISlot[] Slots
    {
        // if the slots is null returns a new array
        get
        {
            return slots ??= new ISlot[MaxSlots];
        }
        // clear the previous data and sets the new slots
        set
        {
            if (slots != null)
                ClearLayer();
            slots = value;
        }
    }

    /// <summary>
    /// Gets/sets the component data from/to a data class for saving/loading
    /// </summary>
    public Layer Layer
    {
        get
        {
            var Slots = new Slot[MaxSlots];
            for (int i = 0; i < MaxSlots; i++)
            {

                Slots[i] = this.Slots[i] == null ? Slot.Empty : this.Slots[i].Slot;
            }
            return new Layer()
            {
                //Status = Status,
                MaxSlots = MaxSlots,
                Slots = Slots
            };
        }
        set
        {
            value ??= Layer.Empty;

            //Status = value.Status;
            MaxSlots = value.MaxSlots;
            ClearLayer();
            Slots = new ISlot[MaxSlots];

            for (int i = 0; i < MaxSlots; i++)
            {
                Slots[i] = PrefabsInstanciatorFactory.InitializeNew(value.Slots?[i], transform);
                (Slots[i] as MonoBehaviour).name = $"Slot {i}";
            }
            SetStatus(Status);
        }
    }

    /// <summary>
    /// Returns true if all slots are empty (ItemId = 0) or null
    /// </summary>
    public bool IsEmpty
    {
        get
        { 
            if (slots == null)
                return true;
            foreach (var slot in slots)
            {
                if (slot?.ItemId != 0)
                    return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Returns true if all slots contain data or the slot is null
    /// </summary>
    public bool IsFull 
    {
        get
        {
            if (slots == null)
                return true;
            foreach (var slot in slots)
            {
                if (slot != null && slot.ItemId == 0)
                    return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Returns true if all slots have the same ItemId
    /// </summary>
    public bool IsCombinable
    {
        get
        {
            if (slots == null)
                return false;
            if (slots[0] == null)
                return false;

            var id = slots[0].ItemId;
            foreach (var slot in slots)
            {
                if (slot == null)
                    return false;
                if (slot.ItemId != id)
                    return false;
            }
            return true;
        }
    }
    #endregion


    #region CLEAR & REMOVE LAYER

    /// <summary>
    /// Clears the layer slots.
    /// </summary>
    /// <param name="removeSlots">If true destorys the slots, else only clears the slots</param>
    public void ClearLayer(bool removeSlots = true)
    {
        for (int i = 0; i < MaxSlots && Slots != null; i++)
        {
            if (removeSlots)
                Slots[i]?.RemoveSlot();
            else
                Slots[i]?.ClearSlot();
        }
    }

    /// <summary>
    /// Destroy this layer and removes (if) the references from the container this layer is in.
    /// </summary>
    public void RemoveLayer()
    {
        ClearLayer();

        transform.GetComponentInParent<IContainer>()?.RemoveLayer(this);
        DestroyImmediate(gameObject);
    }

    /// <summary>
    /// Removes the slot reference from the slots
    /// </summary>
    /// <param name="slot">The slot to be removed</param>
    public void RemoveSlot(ISlot slot)
    {
        if (Slots != null)
        {
            for (int i = 0; i < MaxSlots; i++)
            {
                if (Slots[i] == slot)
                {
                    Slots[i] = null;
                    break;
                }
            }
        }
    }

    #endregion

    #region LAYER EDITING

    /// <summary>
    /// Sets the status of this layer.
    /// </summary>
    /// <param name="status">The new status</param>
    public void SetStatus(LayerStatus status)
    {
        Status = status;

        // Arranges the slots positions based on the number of
        // max slots and the position in the array
        for (int i = 0; i < MaxSlots; i++)
        {
            if (Slots[i] != null)
                Slots[i].SlotPosition = new Vector3((-MaxSlots / 2 + i) * SlotDistance, 0f, 0f);
        }

        // Sets the status for each slot. Render, Draggable, render order, color
        // and the position of the layer 
        if ((status & LayerStatus.Hidden) != 0)
        {
            foreach (ISlot slot in Slots)
            {
                slot?.SetStatus();
            }
        }
        else if ((status & LayerStatus.Front) != 0)
        {
            transform.localPosition = Vector3.zero;

            foreach (ISlot slot in Slots)
            {
                slot?.SetStatus(true, true, -1, frontColor);
            }
        }
        else if ((status & LayerStatus.Back) != 0)
        {
            transform.localPosition = backroundLayerOffset;

            foreach (ISlot slot in Slots)
            {
                slot?.SetStatus(true, false, -2, backColor);
            }
        }
        
    }
    #endregion
}
