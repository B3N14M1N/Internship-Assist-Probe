using UnityEngine;
using Zenject;

public class LevelManagerInstaller : MonoInstaller
{
    public string levelPath;
    public string progressDataPath;
    public override void InstallBindings()
    {
        Container.Bind<LevelManager>().AsSingle().WithArguments(levelPath,progressDataPath).NonLazy();
    }
}