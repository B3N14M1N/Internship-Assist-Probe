using System.Collections.Generic;
using UnityEngine;

using GameItemHolders;

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
    public bool Initialized { get; private set; }

    private List<IContainer> containers = new List<IContainer>();
    public void Awake()
    {
        LoadLevel(0);
    }

    public void LoadLevel(int level)
    {
        Paused = false;
        Started = false;
        var levelModel = AssetsManager.GetLevelModel(level);
        if (levelModel != null)
        {
            Debug.Log($"Loaded level: {levelModel.level}");
            Level = levelModel.level;
            LevelTime = levelModel.LevelTime;
            LevelRemainingTime = levelModel.LevelTime;
            TimerName = nameof(Level) + Level;
            foreach (Container container in levelModel.containers)
            {
                AddContainer(container);
            }
            Initialized = true;
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
        Initialized = false;
        Started = false;
        Paused = false;
        GameEventsManager.Instance.Paused = Paused;
        Timer.RemoveTimer(TimerName);
        while (containers.Count > 0)
        {
            containers[0].RemoveContainer();
        }
        LoadLevel(Level);
    }

    public void StartLevel()
    {
        if (Initialized && !Started)
        {
            Debug.Log($"Level Started. Time: {LevelTime}");

            if (Started)
                EndLevel();
            Timer.StartTimer(TimerName, LevelTime);
            LevelRemainingTime = Timer.GetTimer(TimerName);
            Started = true;
        }
    }

    public void EndLevel()
    {
        if(Initialized && Started)
        {
            Debug.Log($"Level Ended. Time remaining: {Timer.GetTimer(TimerName)}");
            Timer.RemoveTimer(TimerName);
            Started = false;
            Paused = false;
            Initialized = false;
            GameEventsManager.Instance.Paused = Paused;
        }
    }

    public void PauseLevel(bool pause)
    {
        Debug.Log($"Level Paused. Time remaining: {Timer.GetTimer(TimerName)}");
        Paused = pause;
        GameEventsManager.Instance.Paused = Paused;
    }
    public void Update()
    {
        if (Initialized && !Started && GameEventsManager.Instance.SlotSelected)
        {
            StartLevel();
        }

        if (Initialized && Started && !Paused)
        {
            Timer.UpdateTimer(TimerName, Time.deltaTime);
            LevelRemainingTime = Timer.GetTimer(TimerName);

            bool empty = true;
            foreach(var container in containers)
            {
                if (!container.IsEmpty)
                    empty = false;
            }

            if (LevelRemainingTime <= 0 || empty)
            {
                EndLevel();
            }
        }
    }

}
