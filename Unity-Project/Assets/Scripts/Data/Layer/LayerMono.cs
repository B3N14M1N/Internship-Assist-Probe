using UnityEngine;

public class LayerMono : MonoBehaviour, ILayer
{
    #region FIELDS

    private static readonly Vector3 backroundLayerOffset = new Vector3(0.85f, 0.5f, 0f);
    private static readonly float SlotDistance = 6.5f;
    private static readonly Color backColor = new Color(130f / 256f, 130f / 256f, 130f / 256f, 1f);
    private static readonly Color frontColor = Color.white;
    private static readonly int ThisClassMaxSlots = 3;

    public int LayerOrder {  get; set; }
    public LayerStatus Status {  get; set; }
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
    private ISlot[] slots;
    public ISlot[] Slots 
    {
        get
        {
            return slots ??= new ISlot[MaxSlots];
        }
        set
        {
            if (slots != null)
                ClearLayer();
            slots = value;
        }
    }

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
                LayerOrder = LayerOrder,
                Status = Status,
                MaxSlots = MaxSlots,
                Slots = Slots
            };
        }
        set
        {
            value ??= Layer.Empty;

            LayerOrder = value.LayerOrder;
            Status = value.Status;
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
    #endregion


    #region CREATE & REMOVE LAYER

    public void RemoveLayer()
    {
        ClearLayer();

        transform.GetComponentInParent<IContainer>()?.RemoveLayer(this);
        DestroyImmediate(gameObject);
    }

    public void ClearLayer()
    {
        for (int i = 0; i < MaxSlots && Slots != null; i++)
        {
            if (Slots[i] != null)
            {
                Slots[i].RemoveSlot();
            }
        }
    }
    #endregion

    #region LAYER EDITING

    public void RemoveSlot(ISlot slot)
    {
        if(Slots != null)
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

    public int PullLayer()
    {
        return MoveLayer(-1);
    }

    public int PushLayer()
    {
        return MoveLayer(+1);
    }

    private int MoveLayer(int direction)
    {
        if (LayerOrder > 0 && direction < 0 || direction > 0)
            LayerOrder += direction;

        var newStatus = Status;
        if (LayerOrder > 1 && Status != LayerStatus.Hidden)
            newStatus = LayerStatus.Hidden;
        if (LayerOrder == 1 && Status != LayerStatus.Back)
            newStatus = LayerStatus.Back;
        if (LayerOrder == 0 && Status != LayerStatus.Front)
            newStatus = LayerStatus.Front;
        SetStatus(newStatus);

        return LayerOrder;
    }
    public void SetStatus(LayerStatus status)
    {
        for(int i = 0; i < MaxSlots; i++)
        {
            if (Slots[i] != null)
                Slots[i].SlotPosition = new Vector3((-MaxSlots / 2 + i) * SlotDistance, 0f, 0f);
        }
        Status = status;
        if (status == LayerStatus.Front)
        {
            transform.localPosition = Vector3.zero;

            foreach(ISlot slot in Slots)
            {
                slot?.SetStatus(true, true, -LayerOrder, frontColor);
            }
        }
        if (status == LayerStatus.Back)
        {
            transform.localPosition = backroundLayerOffset;

            foreach (ISlot slot in Slots)
            {
                slot?.SetStatus(true, false, -LayerOrder, backColor);
            }
        }
        if ((status & (LayerStatus.Hidden | LayerStatus.Empty)) != 0)
        {
            foreach (ISlot slot in Slots)
            {
                slot?.SetStatus();
            }
        }
    }
    #endregion
}
