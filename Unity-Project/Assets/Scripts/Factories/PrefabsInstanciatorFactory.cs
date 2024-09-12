using UnityEngine;

using GameItemHolders;

/// <summary>
/// This class is used for instanciating prefabs
/// </summary>
public class PrefabsInstanciatorFactory : MonoBehaviour
{

    /// <summary>
    /// Instanciates a new Slot prefab and returns the slot component
    /// </summary>
    /// <param name="slot">The slot data class</param>
    /// <param name="parent">If not null, sets the newly instanciated object parent to this</param>
    /// <param name="loadData">If true loads the data from the slot data class</param>
    /// <returns></returns>
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

    /// <summary>
    /// Instanciates a new Layer prefab and returns the layer component
    /// </summary>
    /// <param name="layer">The layer data class</param>
    /// <param name="parent">If not null, sets the newly instanciated object parent to this</param>
    /// <param name="loadData">If true loads the data from the layer data class</param>
    /// <returns></returns>
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

    /// <summary>
    /// Instanciates a new Container prefab and returns the container component
    /// </summary>
    /// <param name="container">The container data class</param>
    /// <param name="parent">If not null, sets the newly instanciated object parent to this</param>
    /// <param name="loadData">If true loads the data from the container data class</param>
    /// <returns></returns>
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
