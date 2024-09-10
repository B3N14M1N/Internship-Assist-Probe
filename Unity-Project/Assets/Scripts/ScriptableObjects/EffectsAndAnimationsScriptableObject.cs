using UnityEngine;

using GameItemHolders;

[CreateAssetMenu(fileName = "EffectsAndAnimationsSO", menuName ="Scriptable Objects/Effects And Animations", order = 1)]
public class EffectsAndAnimationsScriptableObject : ScriptableObject
{
    public string Identificator;
    public Material ParticleMaterial;
    public Animation StartAnimation;
    public GameObject ParticlesPrefab;

    public void StartEffects(ILayer layer)
    {
        var particlesObject = Instantiate(ParticlesPrefab);
        particlesObject.transform.position = (layer as MonoBehaviour).transform.position;
        ParticleSystem particleSystem = particlesObject.GetComponent<ParticleSystem>();
        GameItemData item = AssetsManager.GetGameItemData(layer.Slots[0].ItemId);
        var mainAccess = particleSystem.main;
        mainAccess.maxParticles = layer.MaxSlots;

        Material material = new Material(ParticleMaterial);
        material.SetTexture("_MainTex", item.sprite.texture);
        mainAccess.startSize3D = true;
        mainAccess.startSizeX = item.scale.x * (item.sprite.texture.width / item.sprite.pixelsPerUnit);
        mainAccess.startSizeY = item.scale.y * (item.sprite.texture.height / item.sprite.pixelsPerUnit);

        particlesObject.GetComponent<ParticleSystemRenderer>().sharedMaterial = material;

        particleSystem.Play();
        // Get particles and set positions to match the Items on layer
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[layer.MaxSlots];
        int numParticlesAlive = particleSystem.GetParticles(particles, layer.MaxSlots);
        //Debug.Log(particles.Length);
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].position = layer.Slots[i].SlotPosition;
            //Debug.Log(particles[i].position);
        }

        particleSystem.SetParticles(particles, particles.Length);
    }


}
