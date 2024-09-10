using Zenject;

public class AssetsManagerInstaller : MonoInstaller
{
    public string levelPath;
    public string ItemsPath;
    public string PrefabsPath;
    public string EffectsPath;
    public override void InstallBindings()
    {
        Container.Bind<AssetsManager>().AsSingle().WithArguments(levelPath, ItemsPath, PrefabsPath, EffectsPath).NonLazy();
    }
}