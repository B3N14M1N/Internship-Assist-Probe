using System.Collections;
using UnityEngine;

using GameItemHolders;

public class GameEventsManager : MonoBehaviour
{
    public float ItemReturnSpeed = 150.0f;
    private static GameEventsManager instance;
    public static GameEventsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameEventsManager>();
            }
            return instance;
        }
    }


    public bool SlotSelected { get; private set; }

    private ISlot slotSelected { get; set; }

    public bool Paused { get; set; }

    public bool SelectedSlot(ISlot slot, bool check = false)
    {
        if (!check)
        {
            SlotSelected = true;
            slotSelected = slot;
            (slot as MonoBehaviour).GetComponentInChildren<Animation>().Play("Grab");
        }
        else
        {
            return slotSelected == slot;
        }
        return true;
    }

    public bool UnselectedSlot(ISlot slot)
    {

        if (slot != null && (slot as MonoBehaviour).isActiveAndEnabled)
            (slot as MonoBehaviour).StartCoroutine(ReturnBack(slot));
        SlotSelected = false;
        return true;
    }
    IEnumerator ReturnBack(ISlot slot)
    {
        if (slot != null)
        {
            var gameObject = (slot as MonoBehaviour)?.transform;
            while (slot != null && gameObject.localPosition != slot.SlotPosition)
            {
                gameObject.localPosition = Vector3.MoveTowards(gameObject.localPosition, slot.SlotPosition, ItemReturnSpeed * Time.deltaTime);
                yield return null;
            }
            (slot as MonoBehaviour).GetComponentInChildren<Animation>().Play("Drop");
        }
    }


    public void ChangedSlots(ISlot to, ISlot from)
    {
        if (to != null)
        {
            CheckContainer((to as MonoBehaviour).GetComponentInParent<IContainer>(true));
            (to as MonoBehaviour).GetComponentInChildren<Animation>().Play("Drop");
        }
        if (from != null)
            CheckContainer((from as MonoBehaviour).GetComponentInParent<IContainer>(true));
    }

    public void CheckContainer(IContainer container)
    {
        if (container != null && !container.IsEmpty)
        {
            container.ClearContainer(keepTopLayer: true, clearOnlyEmpty: true);
            container.RearangeLayers(true);
            CheckLayer(container.Layers[0]);
        }
    }

    public void CheckLayer(ILayer layer)
    {
        if (layer != null)
        {
            if (layer.IsCombinable)
            {
                CombinedSlotsEffects(layer);
                layer.ClearLayer(removeSlots: false);
                CheckContainer((layer as MonoBehaviour).GetComponentInParent<IContainer>(true));
            }
        }
    }
    public void CombinedSlotsEffects(ILayer layer)
    {
        AssetsManager.GetEffects("Combine").StartEffects(layer);
        Debug.Log("Combined layer");
    }

}
