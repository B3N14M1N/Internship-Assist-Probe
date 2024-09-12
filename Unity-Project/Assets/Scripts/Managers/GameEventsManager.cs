using System.Collections;
using UnityEngine;

using GameItemHolders;

/// <summary>
/// This component manages the game events
/// </summary>
public class GameEventsManager : MonoBehaviour
{
    #region Fields

    /// <summary>
    /// This float is the item return speed to the original position in layer after it was dragged away
    /// </summary>
    public float ItemReturnSpeed = 150.0f;

    /// <summary>
    /// Singleton
    /// </summary>
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

    /// <summary>
    /// If the game has been loaded and initialized
    /// </summary>
    public bool Initialized { get; set; }

    /// <summary>
    /// If the game has started
    /// </summary>
    public bool Started { get; set; }

    /// <summary>
    /// If the game has been paused
    /// </summary>
    public bool Paused { get; set; }

    /// <summary>
    /// If the level has been completed
    /// </summary>
    public bool Won { get; set; }

    /// <summary>
    /// If the level has been lost
    /// </summary>
    public bool Lost { get; set; }

    /// <summary>
    /// The number of stars collected  -------not used
    /// </summary>
    public int Stars { get; set; }

    /// <summary>
    /// The combo for combining items
    /// </summary>
    public int Combo { get; set; }

    /// <summary>
    /// If there is a slot selected
    /// </summary>
    public bool SlotSelected { get; set; }

    /// <summary>
    /// The current selected slot
    /// </summary>
    private ISlot Slot { get; set; }

    #endregion

    #region Methods

    /// <summary>
    /// Event for selected slot or a method to check if the slot is the selected one.
    /// Plays an animation on selection.
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="check"></param>
    /// <returns></returns>
    public bool SelectSlot(ISlot slot, bool check = false)
    {
        if (!check)
        {
            SlotSelected = true;
            Slot = slot;
            (slot as MonoBehaviour).GetComponentInChildren<Animation>().Play("Grab");
        }
        else
        {
            return Slot == slot;
        }
        return true;
    }

    /// <summary>
    /// Event for unselecting a slot
    /// Starts a coroutine for returning the item to its original position.
    /// </summary>
    /// <param name="slot"></param>
    public void UnselectSlot(ISlot slot)
    {

        if (slot != null && (slot as MonoBehaviour).isActiveAndEnabled)
            (slot as MonoBehaviour).StartCoroutine(ReturnBack(slot));
        SlotSelected = false;
    }

    /// <summary>
    /// A coroutine for returning the slot to its original position.
    /// When the slot returned, plays an animation
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
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

    /// <summary>
    /// An event for slots changed.
    /// Checks both slots parent containers for other game events (pull layer in fornt| combine layer | etc.)
    /// </summary>
    /// <param name="to"></param>
    /// <param name="from"></param>
    public void ChangeSlots(ISlot to, ISlot from)
    {
        if (to != null)
        {
            CheckContainer((to as MonoBehaviour).GetComponentInParent<IContainer>(true));
            (to as MonoBehaviour).GetComponentInChildren<Animation>().Play("Drop");
        }
        if (from != null)
            CheckContainer((from as MonoBehaviour).GetComponentInParent<IContainer>(true));
    }

    /// <summary>
    /// Check if the container exists and clears the layers (empty and keeps only the top one)
    /// Rearanges the layers and checks the top layer for other events.
    /// </summary>
    /// <param name="container"></param>
    public void CheckContainer(IContainer container)
    {
        if (container != null && !container.IsEmpty)
        {
            container.ClearContainer(keepTopLayer: true, clearOnlyEmpty: true);
            container.RearrangeLayers(true);
            CheckLayer(container.Layers[0]);
        }
    }

    /// <summary>
    /// Checks if the layer exists and if it is ready to combine.
    /// If so Launches an effect, adds a combo and clears the layer then lauches a container check event
    /// </summary>
    /// <param name="layer"></param>
    public void CheckLayer(ILayer layer)
    {
        if (layer != null)
        {
            if (layer.IsCombinable)
            {
                Combo += 1;
                CombinedSlotsEffects(layer);
                layer.ClearLayer(removeSlots: false);
                CheckContainer((layer as MonoBehaviour).GetComponentInParent<IContainer>(true));
            }
        }
    }

    /// <summary>
    /// Plays a combine effect for the layer
    /// </summary>
    /// <param name="layer"></param>
    public void CombinedSlotsEffects(ILayer layer)
    {
        StartCombineEffects(layer, AssetsManager.GetEffects("Combine"));
        Debug.Log("Combined layer");
    }

    /// <summary>
    /// Spawns a particle with the same sprite for 
    /// each slot in the layer and plays the particle effect.
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="so"></param>
    public void StartCombineEffects(ILayer layer, EffectsScriptableObject so)
    {
        var particlesObject = Instantiate(so.ParticlesPrefab);
        particlesObject.transform.position = (layer as MonoBehaviour).transform.position;
        ParticleSystem particleSystem = particlesObject.GetComponent<ParticleSystem>();
        GameItemData item = AssetsManager.GetGameItemData(layer.Slots[0].ItemId);
        var mainAccess = particleSystem.main;
        mainAccess.maxParticles = layer.MaxSlots;

        Material material = new Material(so.ParticleMaterial);
        material.SetTexture("_MainTex", item.Sprite.texture);
        mainAccess.startSize3D = true;
        mainAccess.startSizeX = item.Scale.x * (item.Sprite.texture.width / item.Sprite.pixelsPerUnit);
        mainAccess.startSizeY = item.Scale.y * (item.Sprite.texture.height / item.Sprite.pixelsPerUnit);

        particlesObject.GetComponent<ParticleSystemRenderer>().sharedMaterial = material;

        particleSystem.Play();
        // Get particles and set positions to match the Items on layer
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[layer.MaxSlots];
        int numParticlesAlive = particleSystem.GetParticles(particles, layer.MaxSlots);

        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].position = layer.Slots[i].SlotPosition;
        }

        particleSystem.SetParticles(particles, particles.Length);
    }
    #endregion
}
