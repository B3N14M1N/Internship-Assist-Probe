using UnityEngine;

using GameItemHolders;

public class PrefabsInstanciatorFactory : MonoBehaviour
{
    public static ISlot InitializeNew(Slot slot = null, Transform parent = null,  bool loadData = true)
    {
        var prefab = AssetsManager.GetPrefab("Slot");
        if (prefab == null)
            return null;

        ISlot newSlot = Instantiate(prefab).GetComponent<ISlot>();
        if (parent != null)
        {
            (newSlot as MonoBehaviour).transform.parent = parent;
            (newSlot as MonoBehaviour).transform.position = parent.position;
        }

        if (loadData)
        {
            newSlot.Slot = slot;
        }

        return newSlot;
    }
    public static ILayer InitializeNew(Layer layer = null, Transform parent = null, bool loadData = true)
    {
        var prefab = AssetsManager.GetPrefab("Layer");
        if (prefab == null)
            return null;

        var newLayer = Instantiate(prefab).GetComponent<ILayer>();
        if (parent != null)
        {
            (newLayer as MonoBehaviour).transform.parent = parent;
            (newLayer as MonoBehaviour).transform.position = parent.position;
        }

        if (loadData)
        {
            newLayer.Layer = layer;
        }
        return newLayer;
    }

    public static IContainer InitializeNew(Container container = null, Transform parent = null, bool loadData = true)
    {
        var prefab = AssetsManager.GetPrefab("Container");
        if (prefab == null)
            return null;

        var newContainer = Instantiate(prefab).GetComponent<IContainer>();
        if (parent != null)
        {
            (newContainer as MonoBehaviour).transform.parent = parent;
            (newContainer as MonoBehaviour).transform.localPosition = container.Position;
        }

        if (loadData)
        {
            newContainer.Container = container;
        }
        return newContainer;
    }
}
