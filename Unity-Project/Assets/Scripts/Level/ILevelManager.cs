public interface ILevelManager
{
    void LoadLevel(int level = 0);
    IContainer AddContainer(Container container = null);
    void RemoveContainer(ContainerMono container);
}
