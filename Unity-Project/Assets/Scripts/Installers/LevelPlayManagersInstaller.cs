using Zenject;

public class LevelPlayManagersInstaller : MonoInstaller
{
    public LevelPlayManager levelPlayManager;
    public override void InstallBindings()
    {
        Container.Bind<LevelPlayManager>().FromInstance(levelPlayManager).AsSingle();
    }
}