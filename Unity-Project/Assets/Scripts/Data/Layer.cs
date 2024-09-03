using System.Linq.Expressions;
using UnityEngine;


public enum LayerStatus
{
    hidden = 0,
    front = 1,
    back = 2,
}
public class Layer : MonoBehaviour
{
    public static readonly Vector3 backroundLayerOffset = new Vector3(0.85f, 0.5f, 0f);
    public static readonly float SlotDistance = 6.5f;
    public static Color backColor = new Color(130f / 256f, 130f / 256f, 130f / 256f, 1f);
    public static Color frontColor = Color.white;
    [SerializeField]
    private Vector3 layerPosition; 

    [SerializeField]
    private int layerOrder;
    [SerializeField]
    public LayerStatus status;
    [SerializeField]
    public Slot Slot1;
    [SerializeField]
    public Slot Slot2;
    [SerializeField]
    public Slot Slot3;

    public bool IsFull => Slot1 != null && Slot2 != null && Slot3 != null;
    public bool Combine => IsFull && Slot1.ItemId == Slot2.ItemId && Slot2.ItemId == Slot3.ItemId;

    public int PullLayer()
    {
        if (layerOrder > -1)
            layerOrder--;

        var newStatus = status;
        if(layerOrder > 1 && status != LayerStatus.hidden)
            newStatus = LayerStatus.hidden;
        if (layerOrder == 1 && status != LayerStatus.back)
            newStatus = LayerStatus.back;
        if (layerOrder == 0 && status != LayerStatus.front)
            newStatus = LayerStatus.front;
        SetStatus(newStatus);

        return layerOrder;
    }

    public int PushLayer()
    {
        if (layerOrder < 2)
            layerOrder++;

        var newStatus = status;
        if (layerOrder > 1 && status != LayerStatus.hidden)
            newStatus = LayerStatus.hidden;
        if (layerOrder == 1 && status != LayerStatus.back)
            newStatus = LayerStatus.back;
        if (layerOrder == 0 && status != LayerStatus.front)
            newStatus = LayerStatus.front;
        SetStatus(newStatus);

        return layerOrder;
    }

    public void SetItem(Slot slot)
    {

    }

    public void LoadLayer(Slot slot1, Slot slot2, Slot slot3, LayerStatus status, Vector3 layerPosition)
    {
        this.status = status;
        Slot1 = slot1;
        Slot2 = slot2;
        Slot3 = slot3;
        this.layerPosition = layerPosition;
        
        SetStatus(status);
    }
    public void SetStatus(LayerStatus status)
    {
        if (Slot1 != null)
            Slot1.SlotPosition = new Vector3(-SlotDistance, 0f, 0f);
        if (Slot2 != null)
            Slot2.SlotPosition = new Vector3(0, 0f, 0f);
        if (Slot3 != null)
            Slot3.SlotPosition = new Vector3(SlotDistance, 0f, 0f);

        this.status = status;
        if (status == LayerStatus.front) {

            this.transform.localPosition = layerPosition;
            Slot1?.SetStatus(true, true, -layerOrder, frontColor);
            Slot2?.SetStatus(true, true, -layerOrder, frontColor);
            Slot3?.SetStatus(true, true, -layerOrder, frontColor);
        }
        if (status == LayerStatus.back)
        {
            this.transform.localPosition = layerPosition + backroundLayerOffset;
            Slot1?.SetStatus(true, false, -layerOrder, backColor);
            Slot2?.SetStatus(true, false, -layerOrder, backColor);
            Slot3?.SetStatus(true, false, -layerOrder, backColor);
        }
        if (status == LayerStatus.hidden)
        {
            Slot1?.SetStatus();
            Slot2?.SetStatus();
            Slot3?.SetStatus();
        }
    }
}
