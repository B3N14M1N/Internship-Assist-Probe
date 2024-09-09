using System;
using System.Collections;
using UnityEngine;

using GameItemHolders;

[Serializable]
public class SlotMono : MonoBehaviour, ISlot
{
    #region FIELDS

    private BoxCollider2D slotCollider;
    //public CircleCollider2D slotCollider;
    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        slotCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

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
            LoadData(AssetsManager.GetGameItemData(value.gameItemId));
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

        ItemId = data.gameItemId;
        spriteRenderer.sprite = data.sprite;
        spriteRenderer.transform.localPosition = data.position;
        spriteRenderer.transform.localScale = data.scale;
    }

    public void SetStatus(bool render = false, bool draggable = false, int renderOrder = 0, Color? renderColor = null)
    {
        Render = render;
        Draggable = draggable;
        RenderColor = renderColor != null ? renderColor.Value : Color.white;
        RenderOrder = renderOrder;
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

    #endregion

    #region Methods
    //private int previousRenderOrder;
    private Vector3 offset;
    private int previousLayer;
    private Vector3 DragPosition()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    private void OnMouseDown()
    {
        bool ok = GameEventsManager.Instance != null ?
            !GameEventsManager.Instance.SlotSelected && !GameEventsManager.Instance.Paused
            : true;
        if (ItemId != 0 && ok)
        {
            offset = transform.position - DragPosition();
            //previousRenderOrder = RenderOrder;
            RenderOrder = 3001;
            previousLayer = spriteRenderer.gameObject.layer;
            spriteRenderer.gameObject.layer = 6; // TOP LAYER RENDER
            GameEventsManager.Instance?.SelectedSlot(this);
        }
    }

    private void OnMouseDrag()
    {
        bool ok = GameEventsManager.Instance != null ? !GameEventsManager.Instance.Paused: true;
        if (ItemId != 0 && ok)
        {
            transform.position = DragPosition() + offset;
        }
    }

    private void OnMouseUp()
    {
        bool ok = GameEventsManager.Instance != null ? !GameEventsManager.Instance.Paused : true;
        if (ItemId != 0 && ok)
        {
            slotCollider.enabled = false;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            slotCollider.enabled = true;
            //RenderOrder = previousRenderOrder;
            spriteRenderer.gameObject.layer = previousLayer;
            if (hit.collider != null)
            {
                ISlot hitSlot = hit.collider.GetComponent<ISlot>();
                if (hitSlot?.ItemId == 0)
                {
                    hitSlot.LoadData(AssetsManager.GetGameItemData(ItemId));
                    ClearSlot();
                    GameEventsManager.Instance?.ChangedSlots(hitSlot, this);
                }
            }


            // if instant return wanted uncomment this section
            /*
            GameEventsManager.Instance?.UnselectedSlot(null);
            transform.localPosition = this.SlotPosition;
            */
            // and comment this section
            if (this != null)
                GameEventsManager.Instance?.UnselectedSlot(this);
        }
    }
    #endregion
}