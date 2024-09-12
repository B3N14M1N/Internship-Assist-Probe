using UnityEngine;

using GameItemHolders;

[CreateAssetMenu(fileName = "EffectsSO", menuName ="Scriptable Objects/Effects", order = 1)]
public class EffectsScriptableObject : ScriptableObject
{
    public string Identificator;
    public Material ParticleMaterial;
    public GameObject ParticlesPrefab;

}
