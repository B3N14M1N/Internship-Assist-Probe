using Zenject;

public class LevelManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        var levelPlay = FindObjectOfType<LevelPlayManager>();
        var levelBuilder = FindObjectOfType<LevelBuilderManager>();
        Container.Bind<LevelPlayManager>().FromInstance(levelPlay).AsSingle();
        Container.Bind<LevelBuilderManager>().FromInstance(levelBuilder).AsSingle();
    }
}