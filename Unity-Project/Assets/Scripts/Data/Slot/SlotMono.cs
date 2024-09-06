using System;
using UnityEngine;

[Serializable]
public class SlotMono : MonoBehaviour, ISlot
{
    #region FIELDS

    public CircleCollider2D slotCollider;
    public SpriteRenderer spriteRenderer;

    private Vector3 slotPosition;
    
    public int ItemId { get; set; }
    public Vector3 SlotPosition { get { return slotPosition; } set { transform.localPosition = value; slotPosition = value; } }
    public bool Draggable { get { return slotCollider.enabled; } set { slotCollider.enabled = value; } }
    public bool Render { set { gameObject.SetActive(value); } get { return gameObject.activeSelf; } }
    public int RenderOrder { get { return spriteRenderer.sortingOrder; } set { spriteRenderer.sortingOrder = value; } }
    public Color RenderColor { set { spriteRenderer.color = value; } }
    public Slot Slot 
    { 
        get {
            return new Slot()
            {
                gameItemId = ItemId
            };
        }
        set
        {
            value ??= Slot.Empty;
            ItemId = value.gameItemId;
            LoadData(AssetsManager.GetGameItemData(ItemId));
        }
    }
    #endregion

    #region CREATE & LOAD & REMOVE

    public void LoadData()
    {
        LoadData(AssetsManager.GetGameItemData(ItemId));
    }

    public void LoadData(GameItemData data)
    {
        data ??= new GameItemData();

        spriteRenderer.sprite = data.sprite;
        spriteRenderer.transform.localPosition = data.position;
        spriteRenderer.transform.localScale = data.scale;
    }

    public void RemoveSlot()
    {
        transform.GetComponentInParent<ILayer>()?.RemoveSlot(this);
        DestroyImmediate(gameObject);
    }

    public void ClearSlot()
    {
        LoadData(null);
    }

    public void SetStatus(bool render = false, bool draggable = false, int renderOrder = 0, Color? renderColor = null)
    {
        Render = render;
        Draggable = draggable;
        RenderColor = renderColor != null ? renderColor.Value : Color.white;
        RenderOrder = renderOrder;
    }
    #endregion

    #region Methods

    private ILayer previousLayer;
    private IContainer previousContainer;
    private int previousRenderOrder;
    private Vector3 offset;
    private Vector3 DragPosition()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    private void OnMouseDown()
    {
        previousLayer = transform.GetComponentInParent<ILayer>();
        previousContainer = (previousLayer as MonoBehaviour).transform.GetComponentInParent<IContainer>();
        offset = transform.position - DragPosition();
        previousRenderOrder = RenderOrder;
        RenderOrder = 10;
    }
    private void OnMouseDrag()
    {
        transform.position = DragPosition() + offset;
    }
    private void OnMouseUp()
    {
        transform.localPosition = slotPosition;
        RenderOrder = previousRenderOrder;
    }

    #endregion
}