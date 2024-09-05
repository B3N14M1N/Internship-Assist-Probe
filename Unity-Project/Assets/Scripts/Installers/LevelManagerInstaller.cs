using Zenject;

public class LevelManagerInstaller : MonoInstaller
{
    public string levelPath;
    public string progressDataPath;
    public string ItemsPath;
    public string PrefabsPath;
    public override void InstallBindings()
    {
        Container.Bind<LevelManager>().AsSingle().WithArguments(progressDataPath, levelPath, ItemsPath, PrefabsPath).NonLazy();
    }
}