using UnityEngine;

public class LayerMono : MonoBehaviour
{
    #region FIELDS

    private static readonly Vector3 backroundLayerOffset = new Vector3(0.85f, 0.5f, 0f);
    private static readonly float SlotDistance = 6.5f;
    private static readonly Color backColor = new Color(130f / 256f, 130f / 256f, 130f / 256f, 1f);
    private static readonly Color frontColor = Color.white;

    public int layerOrder;
    public LayerStatus status;

    public SlotMono Slot1;
    public SlotMono Slot2;
    public SlotMono Slot3;

    public bool IsFull => Slot1 != null && Slot2 != null && Slot3 != null;
    public bool Combine => IsFull && Slot1.ItemId == Slot2.ItemId && Slot2.ItemId == Slot3.ItemId;
    public Layer Layer => Layer.ToModel(this);
    #endregion


    #region CREATE & REMOVE LAYER

    public static LayerMono InitializeNew(Layer layer, Transform parent)
    {
        if (layer == null)
            return null;

        var prefab = Resources.Load("Prefabs/Layer") as GameObject;
        var newLayer = Instantiate(prefab).GetComponent<LayerMono>();
        newLayer.transform.parent = parent;

        newLayer.layerOrder = layer.layerOrder;
        newLayer.status = (LayerStatus)layer.status;


        newLayer.Slot1 = SlotMono.InitializeNew(layer.Slot1, newLayer.transform);
        newLayer.Slot1.name = "Slot 1";
        newLayer.Slot2 = SlotMono.InitializeNew(layer.Slot2, newLayer.transform);
        newLayer.Slot2.name = "Slot 2";
        newLayer.Slot3 = SlotMono.InitializeNew(layer.Slot3, newLayer.transform);
        newLayer.Slot3.name = "Slot 3";

        newLayer.SetStatus((LayerStatus)layer.status);
        return newLayer;
    }

    public void RemoveLayer()
    {
        if(Slot1 != null)
        {
            Slot1.RemoveSlot();
            Slot1 = null;
        }
        if (Slot2 != null)
        {
            Slot2.RemoveSlot();
            Slot2 = null;
        }
        if (Slot3 != null)
        {
            Slot3.RemoveSlot();
            Slot3 = null;
        }

        transform.GetComponentInParent<ContainerMono>()?.RemoveLayer(this);
        DestroyImmediate(gameObject);
    }
    #endregion

    #region LAYER EDITING

    public void RemoveSlot(SlotMono slot)
    {
        if (Slot1 == slot) Slot1 = null;
        if (Slot2 == slot) Slot2 = null;
        if (Slot3 == slot) Slot3 = null;
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
        if (layerOrder > -1 && direction < 0 || direction > 0) //&& direction < 0) || (direction > 0 && layerOrder < 2))
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
            transform.localPosition = Vector3.zero;

            Slot1?.SetStatus(true, true, -layerOrder, frontColor);
            Slot2?.SetStatus(true, true, -layerOrder, frontColor);
            Slot3?.SetStatus(true, true, -layerOrder, frontColor);
        }
        if (status == LayerStatus.back)
        {
            transform.localPosition = backroundLayerOffset;

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
    #endregion
}
