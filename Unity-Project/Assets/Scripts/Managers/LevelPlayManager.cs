using System.Collections.Generic;
using UnityEngine;

using GameItemHolders;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameEventsManager))]
public class LevelPlayManager : MonoBehaviour, ILevelManager
{
    #region Fields

    /// <summary>
    /// The level index
    /// </summary>
    public int Level { get; private set; }

    /// <summary>
    /// The total time allocated for this level in seconds
    /// </summary>
    public float LevelTime {  get; private set; }

    /// <summary>
    /// The timer identificator
    /// </summary>
    private const string LevelTimerName = "LevelTimer";

    /// <summary>
    /// The remaining time to complete the level
    /// </summary>
    public float LevelRemainingTime => Timer.GetTimer(LevelTimerName);


    /// <summary>
    /// The total time allocated to combo in seconds
    /// </summary>
    public float MaxComboTime = 30;

    /// <summary>
    /// The combo timer identificator
    /// </summary>
    private const string ComboTimerName = "ComboTimer";

    /// <summary>
    /// The previous combo (combo is taken from game events manager)
    /// </summary>
    private int prevCombo { get; set; }

    /// <summary>
    /// The combo time decrease by level
    /// </summary>
    private const float ComboTimeMultiplierByLevel = 0.85f;

    /// <summary>
    /// The current combo max time allocated by combo value (higher combo => less time)
    /// </summary>
    private float CurrentComboMaxTime => MaxComboTime * Mathf.Pow(ComboTimeMultiplierByLevel, GameEventsManager.Instance.Combo);

    /// <summary>
    /// The remaining time until combo is reseted
    /// </summary>
    private float ComboRemainingTime => Timer.GetTimer(ComboTimerName);

    /// <summary>
    /// The combo value normalized by the max combo value ( for slider => timer:1 slider full timer:0 slider empty )
    /// </summary>
    public float ComboNormalized => ComboRemainingTime / CurrentComboMaxTime;

    /// <summary>
    /// The game progress. Used to load and save the progress
    /// </summary>
    private GameProgress progress;

    /// <summary>
    /// The current level loaded containers
    /// </summary>
    private List<IContainer> containers = new List<IContainer>();
    #endregion

    #region Methods
    /// <summary>
    /// Loads the level based on the saved progress
    /// </summary>
    public void Awake()
    {
        progress = GameProgress.LoadProgress();
        Level = progress.CurrentLevel;
        LoadLevel(Level);
    }

    /// <summary>
    /// Loads the level and sets the game events.
    /// </summary>
    /// <param name="level"></param>
    public void LoadLevel(int level)
    {
        GameEventsManager.Instance.Started = false;
        GameEventsManager.Instance.Paused = false;
        GameEventsManager.Instance.Lost = false;
        GameEventsManager.Instance.Won = false;
        GameEventsManager.Instance.Combo = prevCombo = 0;
        Timer.RemoveTimer(ComboTimerName);
        Timer.RemoveTimer(LevelTimerName);
        var levelModel = AssetsManager.GetLevelModel(level);
        if (levelModel != null)
        {
            Debug.Log($"Loaded Level: {levelModel.Level}");
            Level = levelModel.Level;
            LevelTime = levelModel.LevelTime;
            Timer.StartTimer(LevelTimerName, levelModel.LevelTime);
            foreach (Container container in levelModel.Containers)
            {
                AddContainer(container);
            }
            GameEventsManager.Instance.Initialized = true;
        }
    }

    /// <summary>
    /// Adds a container to the level. used for loading the level
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    public IContainer AddContainer(Container container)
    {
        IContainer newContainer = PrefabsInstanciatorFactory.InitializeNew(container, transform);
        containers.Add(newContainer);
        return newContainer;
    }

    /// <summary>
    /// Removes the selected container
    /// </summary>
    /// <param name="container"></param>
    public void RemoveContainer(IContainer container)
    {
        containers.Remove(container);
    }

    /// <summary>
    /// Clears the level completly and reloads the level
    /// </summary>
    public void ResetLevel()
    {
        Timer.RemoveTimer(LevelTimerName);
        while (containers.Count > 0)
        {
            containers[0].RemoveContainer();
        }
        LoadLevel(Level);
    }

    /// <summary>
    /// Loads the next level (used in the next level button click action in the won popup menu)
    /// </summary>
    public void LoadNextLevel()
    {
        progress = GameProgress.LoadProgress();
        Level = progress.CurrentLevel;
        ResetLevel();
    }

    /// <summary>
    /// Starts the level and the timer. if already started resets the level
    /// </summary>
    public void StartLevel()
    {
        if (GameEventsManager.Instance.Initialized && !GameEventsManager.Instance.Started)
        {
            Debug.Log($"Level Started. Time: {LevelTime}");

            if (GameEventsManager.Instance.Started)
                EndLevel();
            GameEventsManager.Instance.Started = true;
        }
    }

    /// <summary>
    /// Resets all progress, starting from level 0
    /// </summary>
    public void ResetAllProgress()
    {
        Level = 0;
        progress.CurrentLevel = 0;
        progress.SaveProgress();
        ResetLevel();
    }

    /// <summary>
    /// Ends level and stops the timers.
    /// </summary>
    public void EndLevel()
    {
        if(GameEventsManager.Instance.Initialized && GameEventsManager.Instance.Started)
        {
            Debug.Log($"Level Ended. Time remaining: {Timer.GetTimer(LevelTimerName)}");
            Timer.RemoveTimer(LevelTimerName);
            Timer.RemoveTimer(ComboTimerName);
            GameEventsManager.Instance.Started = false;
            GameEventsManager.Instance.Initialized = false;
            GameEventsManager.Instance.Paused = false;
            progress.SaveProgress();
        }
    }

    /// <summary>
    /// Pauses the game. used by pause buton click action
    /// </summary>
    /// <param name="pause"></param>
    public void PauseLevel(bool pause)
    {
        Debug.Log($"Level Paused. Time remaining: {Timer.GetTimer(LevelTimerName)}");
        GameEventsManager.Instance.Paused = pause;
    }

    /// <summary>
    /// Returns to main menu
    /// </summary>
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// The level loop
    /// </summary>
    public void Update()
    {
        /// <summary>
        /// Don't start the game automatically. Wait for the first slot selected event then start
        /// </summary>
        if (GameEventsManager.Instance.Initialized && !GameEventsManager.Instance.Started && GameEventsManager.Instance.SlotSelected)
        {
            StartLevel();
        }

        /// <summary>
        /// When the game is running
        /// </summary>
        if (GameEventsManager.Instance.Initialized && GameEventsManager.Instance.Started && !GameEventsManager.Instance.Paused)
        {

            /// <summary>
            /// Updates the combo timer
            /// </summary>
            if (prevCombo != GameEventsManager.Instance.Combo)
            {
                prevCombo = GameEventsManager.Instance.Combo;
                Timer.StartTimer(ComboTimerName, CurrentComboMaxTime + Time.deltaTime);
            }
            if (prevCombo != 0)
            {
                Timer.UpdateTimer(ComboTimerName, Time.deltaTime);
                if(ComboRemainingTime <= 0)
                {
                    GameEventsManager.Instance.Combo = prevCombo = 0;
                    Timer.RemoveTimer(ComboTimerName);
                }
            }

            /// <summary>
            /// Updates the level timer
            /// </summary>
            Timer.UpdateTimer(LevelTimerName, Time.deltaTime);


            /// <summary>
            /// Check the containers if all are empty
            /// </summary>
            bool empty = true;
            foreach(var container in containers)
            {
                if (!container.IsEmpty)
                    empty = false;
            }

            /// <summary>
            /// If ran out of time end the game - LOST
            /// </summary>
            if (LevelRemainingTime <= 0)
            {
                GameEventsManager.Instance.Lost = true;
                GameEventsManager.Instance.Won = false;
                EndLevel();
            }

            /// <summary>
            /// If containers are empty then end the game - WON
            /// </summary>
            if (empty)
            {
                GameEventsManager.Instance.Lost = false;
                GameEventsManager.Instance.Won = true;
                progress.CurrentLevel = AssetsManager.GetNextLevel(Level);
                EndLevel();
            }
        }
    }
    #endregion
}
