using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ScriptableObjectsManagerInstaller : MonoInstaller
{
    public string ItemsPath;
    public override void InstallBindings()
    {
        List<GameItemScriptableObject> gameItemScriptableObjects = Resources.LoadAll<GameItemScriptableObject>(ItemsPath).ToList();
        Debug.Log(gameItemScriptableObjects.Count);
        Container.Bind<List<GameItemScriptableObject>>().FromInstance(gameItemScriptableObjects).AsSingle();
    }
}
