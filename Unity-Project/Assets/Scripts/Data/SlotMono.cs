using System;
using UnityEngine;

[Serializable]
public class SlotMono : MonoBehaviour
{
    #region FIELDS

    public CircleCollider2D slotCollider;
    public SpriteRenderer spriteRenderer;

    private Vector3 slotPosition;
    private GameItemData data;

    public int ItemId => data.gameItemId;
    public Vector3 SlotPosition { get { return slotPosition; } set { transform.localPosition = value; slotPosition = value; } }
    public bool Draggable { get { return slotCollider.enabled; } set { slotCollider.enabled = value; } }
    public bool Render { set { gameObject.SetActive(value); } get { return gameObject.activeSelf; } }
    public int RenderOrder { get { return spriteRenderer.sortingOrder; } set { spriteRenderer.sortingOrder = value; } }
    public Color RenderColor { set { spriteRenderer.color = value; } }
    public Slot Slot => Slot.ToModel(this);
    #endregion

    #region CREATE & LOAD & REMOVE

    public static SlotMono InitializeNew(Slot slot, Transform parent)
    {
        if (slot == null)
            return null;

        var prefab = Resources.Load("Prefabs/Slot") as GameObject;

        SlotMono newSlot = Instantiate(prefab).GetComponent<SlotMono>();
        newSlot.LoadData(slot.gameItemId);

        if(parent != null)
            newSlot.transform.parent = parent;

        return newSlot;
    }

    public void RemoveSlot()
    {
        DestroyImmediate(gameObject);
    }

    public void LoadData(int itemGameDataId)
    {
        if (itemGameDataId != 0)
        {
            // get GameItemData from manager and load the sprite;
            data = ScriptableObjectsManager.GetGameItemData(itemGameDataId);

            if (data != null)
            {
                spriteRenderer.sprite = data.sprite;
                spriteRenderer.transform.localPosition = data.position;
                spriteRenderer.transform.localScale = data.scale;
            }
        }
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

    private Vector3 offset;
    private Vector3 DragPosition()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    private void OnMouseDown()
    {
        offset = transform.position - DragPosition();
    }
    private void OnMouseDrag()
    {
        transform.position = DragPosition() + offset;
    }
    private void OnMouseUp()
    {
        transform.localPosition = slotPosition;
    }
    #endregion
}