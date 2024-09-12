using System;
using UnityEngine;

using GameItemHolders;

[Serializable]
public class SlotMono : MonoBehaviour, ISlot
{
    #region FIELDS

    /// <summary>
    /// Private components set at runtime
    /// </summary>
    private BoxCollider2D slotCollider;
    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        slotCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    
    /// <summary>
    /// The Item id that is currently loaded in this slot
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// The position of the slot in the localSpace of the layer
    /// </summary>
    private Vector3 slotPosition;
    public Vector3 SlotPosition { get { return slotPosition; } set { transform.localPosition = value; slotPosition = value; } }

    /// <summary>
    /// If the slot can be dragged
    /// </summary>
    public bool Draggable { get { return slotCollider.enabled; } set { slotCollider.enabled = value; } }

    /// <summary>
    /// If the slot should be rendered
    /// </summary>
    public bool Render { set { gameObject.SetActive(value); } get { return gameObject.activeSelf; } }

    /// <summary>
    /// The render order ( -1 front in front layer | -2 int back layer)
    /// </summary>
    public int RenderOrder { get { return spriteRenderer.sortingOrder; } set { spriteRenderer.sortingOrder = value; } }

    /// <summary>
    /// The render color (white front, darker back)
    /// </summary>
    public Color RenderColor { set { spriteRenderer.color = value; } }

    /// <summary>
    /// Gets/sets the component data from/to a data class for saving/loading
    /// </summary>
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

    #region LOAD & EDIT & REMOVE

    /// <summary>
    /// Loads the current item in this slot
    /// </summary>
    public void LoadData()
    {
        LoadData(AssetsManager.GetGameItemData(ItemId));
    }

    /// <summary>
    /// Loads a new item in this slot
    /// </summary>
    /// <param name="data">The new item</param>
    public void LoadData(GameItemData data)
    {
        data ??= new GameItemData();

        ItemId = data.GameItemId;
        spriteRenderer.sprite = data.Sprite;
        spriteRenderer.transform.localPosition = data.Position;
        spriteRenderer.transform.localScale = data.Scale;
    }

    /// <summary>
    /// Sets the status of this slot
    /// </summary>
    /// <param name="render"></param>
    /// <param name="draggable"></param>
    /// <param name="renderOrder"></param>
    /// <param name="renderColor"></param>
    public void SetStatus(bool render = false, bool draggable = false, int renderOrder = 0, Color? renderColor = null)
    {
        Render = render;
        Draggable = draggable;
        RenderColor = renderColor != null ? renderColor.Value : Color.white;
        RenderOrder = renderOrder;
    }

    /// <summary>
    /// Destroys the current slot and removes the reference in parent the layer (if has any)
    /// </summary>
    public void RemoveSlot()
    {
        transform.GetComponentInParent<ILayer>()?.RemoveSlot(this);
        DestroyImmediate(gameObject);
    }

    /// <summary>
    /// Clears this slot by loading an empty item (ItemId = 0)
    /// </summary>
    public void ClearSlot()
    {
        LoadData(null);
    }

    #endregion

    /// <summary>
    /// These methods are the logic behind Drag and Drop of the game
    /// </summary>
    #region Methods

    /// <summary>
    /// The previous render order. When picked up render on top.
    /// </summary>
    private int previousRenderOrder;

    /// <summary>
    /// The new position where the slot is dragged.
    /// </summary>
    private Vector3 offset;

    /// <summary>
    /// Gets the new position offset based on the previous frame position
    /// </summary>
    /// <returns></returns>
    private Vector3 DragPosition()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    /// <summary>
    /// When the slot (if its not empty) is selected and if the game is not paused
    /// launch an selected slot event and renders the item on top
    /// </summary>
    private void OnMouseDown()
    {
        if (ItemId != 0
            && GameEventsManager.Instance != null
            && !GameEventsManager.Instance.SlotSelected
            && !GameEventsManager.Instance.Paused)
        {
            offset = transform.position - DragPosition();
            previousRenderOrder = RenderOrder;
            RenderOrder = 1;
            GameEventsManager.Instance?.SelectSlot(this);
        }
    }

    /// <summary>
    /// When this slot IS THE selected slot
    /// from the launched event, moves the slot
    /// </summary>
    private void OnMouseDrag()
    {
        if (ItemId != 0
            && GameEventsManager.Instance != null
            && GameEventsManager.Instance.SelectSlot(this, true)
            && !GameEventsManager.Instance.Paused)
        {
            transform.position = DragPosition() + offset;
        }
    }

    /// <summary>
    /// When this slot IS THE selected slot
    /// from the launched event and is being released,
    /// checks if its on top of another empty slot then switches the slots and 
    /// launches an slots switched event, then launches a slot unselected event
    /// </summary>
    private void OnMouseUp()
    {
        if (ItemId != 0
            && GameEventsManager.Instance != null
            && GameEventsManager.Instance.SelectSlot(this, true))
        {
            RenderOrder = previousRenderOrder;

            if (!GameEventsManager.Instance.Paused)
            {
                slotCollider.enabled = false;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                slotCollider.enabled = true;

                if (hit.collider != null)
                {
                    ISlot hitSlot = hit.collider.GetComponent<ISlot>();
                    if (hitSlot?.ItemId == 0)
                    {
                        hitSlot.LoadData(AssetsManager.GetGameItemData(ItemId));
                        ClearSlot();
                        GameEventsManager.Instance?.ChangeSlots(hitSlot, this);
                    }
                }
            }

            if (this != null && GameEventsManager.Instance != null)
                GameEventsManager.Instance.UnselectSlot(this);
        }
    }
    #endregion
}