using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ScriptableObjectsManagerInstaller : MonoInstaller
{
    public List<GameItemScriptableObject> gameItemScriptableObjects;
    public override void InstallBindings()
    {
        Container.Bind<List<GameItemScriptableObject>>().FromInstance(gameItemScriptableObjects).AsSingle();
    }
}
