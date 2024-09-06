using System.Collections.Generic;
using UnityEngine;

public class LevelPlayManager : MonoBehaviour, ILevelManager
{
    public int Level { get; private set; }
    public float LevelTime {  get; private set; }
    public float LevelRemainingTime { get; private set; }
    public int Stars { get; private set; }
    public int Combo { get; private set; }
    public string TimerName {  get; private set; }
    public bool Paused { get; private set; }
    public bool Started { get; private set; }

    private List<IContainer> containers = new List<IContainer>();

    public void Awake()
    {
        LoadLevel(0);
        Paused = false;
        Started = false;
    }

    public void LoadLevel(int level)
    {
        var levelModel = AssetsManager.GetLevelModel(level);
        if (levelModel != null)
        {
            Debug.Log($"Loaded level: {levelModel.level}");
            Level = levelModel.level;
            LevelTime = levelModel.LevelTime;
            TimerName = nameof(Level) + Level;
            foreach (Container container in levelModel.containers)
            {
                AddContainer(container);
            }
        }
    }
    public IContainer AddContainer(Container container)
    {
        IContainer newContainer = PrefabsInstanciatorFactory.InitializeNew(container, transform);
        containers.Add(newContainer);
        return newContainer;
    }
    public void RemoveContainer(ContainerMono container)
    {
        containers.Remove(container);
    }
    public void ResetLevel()
    {
        while (containers.Count > 0)
        {
            containers[0].RemoveContainer();
        }
    }

    public void StartGame()
    {
        Debug.Log($"Game Started. Level Time: {LevelTime}");

        if (Started)
            EndGame();
        Timer.StartTimer(TimerName, LevelTime);
        LevelRemainingTime = Timer.GetTimer(TimerName);
        Started = true;
    }

    public void EndGame()
    {
        Debug.Log($"Game Ended. Level Time remaining: {Timer.GetTimer(TimerName)}");

        ResetLevel();
        Timer.RemoveTimer(TimerName);
        Started = false;
        Paused = false;
    }

    public void PauseGame(bool pause)
    {
        Debug.Log($"Game Paused. Level Time remaining: {Timer.GetTimer(TimerName)}");

        Paused = pause;
    }
    public void Update()
    {
        if (Started && !Paused)
        {
            Timer.UpdateTimer(TimerName, Time.deltaTime);
            LevelRemainingTime = Timer.GetTimer(TimerName);
            if (LevelRemainingTime == 0)
            {
                EndGame();
            }
        }
    }

}
