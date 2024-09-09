using Zenject;

public class AssetsManagerInstaller : MonoInstaller
{
    public string levelPath;
    public string progressDataPath;
    public string ItemsPath;
    public string PrefabsPath;
    public string EffectsPath;
    public override void InstallBindings()
    {
        Container.Bind<AssetsManager>().AsSingle().WithArguments(progressDataPath, levelPath, ItemsPath, PrefabsPath, EffectsPath).NonLazy();
    }
}