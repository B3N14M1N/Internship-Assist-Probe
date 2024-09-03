using Unity.Mathematics;
using UnityEngine;

public class LayerMono : MonoBehaviour
{
    private static readonly Vector3 backroundLayerOffset = new Vector3(0.85f, 0.5f, 0f);
    private static readonly float SlotDistance = 6.5f;
    private static readonly Color backColor = new Color(130f / 256f, 130f / 256f, 130f / 256f, 1f);
    private static readonly Color frontColor = Color.white;

    public Layer layer;

    public SlotMono Slot1;
    public SlotMono Slot2;
    public SlotMono Slot3;

    public bool IsFull => Slot1 != null && Slot2 != null && Slot3 != null;
    public bool Combine => IsFull && Slot1.ItemId == Slot2.ItemId && Slot2.ItemId == Slot3.ItemId;
    public void LoadLayer(Layer layer, LayerStatus status, Vector3 layerPosition)
    {
        this.layer = layer;
        this.layer.layerPosition = layerPosition;

        SetStatus(status);
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
        if ((layer.layerOrder > -1 && direction < 0) || (direction > 0 && layer.layerOrder < 2))
            layer.layerOrder += direction;

        var newStatus = layer.status;
        if (layer.layerOrder > 1 && layer.status != LayerStatus.hidden)
            newStatus = LayerStatus.hidden;
        if (layer.layerOrder == 1 && layer.status != LayerStatus.back)
            newStatus = LayerStatus.back;
        if (layer.layerOrder == 0 && layer.status != LayerStatus.front)
            newStatus = LayerStatus.front;
        SetStatus(newStatus);

        return layer.layerOrder;
    }
    public void SetStatus(LayerStatus status)
    {
        if (Slot1 != null)
            Slot1.SlotPosition = new Vector3(-SlotDistance, 0f, 0f);
        if (Slot2 != null)
            Slot2.SlotPosition = new Vector3(0, 0f, 0f);
        if (Slot3 != null)
            Slot3.SlotPosition = new Vector3(SlotDistance, 0f, 0f);

        this.layer.status = status;
        if (status == LayerStatus.front)
        {
            transform.localPosition = layer.layerPosition;

            Slot1?.SetStatus(true, true, -layer.layerOrder, frontColor);
            Slot2?.SetStatus(true, true, -layer.layerOrder, frontColor);
            Slot3?.SetStatus(true, true, -layer.layerOrder, frontColor);
        }
        if (status == LayerStatus.back)
        {
            transform.localPosition = layer.layerPosition + backroundLayerOffset;

            Slot1?.SetStatus(true, false, -layer.layerOrder, backColor);
            Slot2?.SetStatus(true, false, -layer.layerOrder, backColor);
            Slot3?.SetStatus(true, false, -layer.layerOrder, backColor);
        }
        if (status == LayerStatus.hidden)
        {
            Slot1?.SetStatus();
            Slot2?.SetStatus();
            Slot3?.SetStatus();
        }
    }
    public void DrawSlots()
    {
        //Create || draw SlotMono
    }
}
