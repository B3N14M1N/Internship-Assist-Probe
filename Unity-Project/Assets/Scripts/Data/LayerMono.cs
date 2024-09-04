using Unity.VisualScripting;
using UnityEngine;

public class LayerMono : MonoBehaviour
{
    private static readonly Vector3 backroundLayerOffset = new Vector3(0.85f, 0.5f, 0f);
    private static readonly float SlotDistance = 6.5f;
    private static readonly Color backColor = new Color(130f / 256f, 130f / 256f, 130f / 256f, 1f);
    private static readonly Color frontColor = Color.white;

    public Vector3 layerPosition;
    public int layerOrder;
    public LayerStatus status;

    public SlotMono Slot1;
    public SlotMono Slot2;
    public SlotMono Slot3;

    public bool IsFull => Slot1 != null && Slot2 != null && Slot3 != null;
    public bool Combine => IsFull && Slot1.ItemId == Slot2.ItemId && Slot2.ItemId == Slot3.ItemId;

    public Layer Layer => Layer.ToModel(this);

    public void LoadLayer(Layer layer)
    {
        var prefab = Resources.Load("Prefabs/Slot") as GameObject;

        layerPosition = layer.layerPosition;
        layerOrder = layer.layerOrder;

        if(layer.Slot1.gameItemId != 0)
        {
            if (Slot1 == null)
            {
                Slot1 = Instantiate(prefab).GetComponent<SlotMono>();
                Slot1.transform.parent = this.transform;
            }
            Slot1.LoadGameItem(layer.Slot1);
        }
        if (layer.Slot2.gameItemId != 0)
        {
            if(Slot2 == null)
            {
                Slot2 = Instantiate(prefab).GetComponent<SlotMono>();
                Slot2.transform.parent = this.transform;
            }
            Slot2.LoadGameItem(layer.Slot2);
        }
        if (layer.Slot3.gameItemId != 0)
        {
            if(Slot3 == null)
            {
                Slot3 = Instantiate(prefab).GetComponent<SlotMono>();
                Slot3.transform.parent = this.transform;
            }
            Slot3.LoadGameItem(layer.Slot3);
        }
        SetStatus(layer.status);
    }

    public void RemoveLayer()
    {
        if(Slot1 != null)
        {
            Slot1.Destroy();
            Slot1 = null;
        }
        if (Slot2 != null)
        {
            Slot2.Destroy();
            Slot2 = null;
        }
        if (Slot3 != null)
        {
            Slot3.Destroy();
            Slot3 = null;
        }

        transform.parent.GetComponent<ContainerMono>().RemoveLayer(this);
        DestroyImmediate(gameObject);
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
        if ((layerOrder > -1 && direction < 0) || (direction > 0 && layerOrder < 2))
            layerOrder += direction;

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
    public void SetStatus(LayerStatus status)
    {
        if (Slot1 != null)
            Slot1.SlotPosition = new Vector3(-SlotDistance, 0f, 0f);
        if (Slot2 != null)
            Slot2.SlotPosition = new Vector3(0, 0f, 0f);
        if (Slot3 != null)
            Slot3.SlotPosition = new Vector3(SlotDistance, 0f, 0f);

        this.status = status;
        if (status == LayerStatus.front)
        {
            transform.localPosition = layerPosition;

            Slot1?.SetStatus(true, true, -layerOrder, frontColor);
            Slot2?.SetStatus(true, true, -layerOrder, frontColor);
            Slot3?.SetStatus(true, true, -layerOrder, frontColor);
        }
        if (status == LayerStatus.back)
        {
            transform.localPosition = layerPosition + backroundLayerOffset;

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
