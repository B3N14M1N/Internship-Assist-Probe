using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(GameEventsManager))]
public class LevelDataDisplayManager : MonoBehaviour
{
    #region Fields

    private float previousTime;
    [Inject]
    private LevelPlayManager levelManager;

    public TMP_Text TMP_Level;
    public TMP_Text TMP_Timer;
    public TMP_Text TMP_Combo;
    public TMP_Text TMP_Stars;
    public Slider comboSloder;
    public GameObject menu;
    public TMP_Text TMP_Menu;
    public Color pauseTitleColor;
    public Color lostTitleColor;
    public Color winTitleColor;

    public GameObject gameUnpauseButton;
    public GameObject gameNextLevelButton;
    #endregion

    #region Methods

    public void Awake()
    {
        Debug.Log(levelManager == null ? "Null" : "Injected");
    }

    public void Update()
    {
        var time = levelManager.LevelRemainingTime;
        TMP_Level.text = "Level: " + (levelManager.Level + 1);
        string minutes = "" + (int)(time / 60);
        int seconds = (int)(time % 60);

        TMP_Timer.text = $"Time: {minutes}:{(seconds < 10 ? "0" + seconds : seconds)}";
        if((time <= 30) && (int)previousTime != (int)time)
        {
            TMP_Timer.GetComponent<Animation>().Play("Pulse Animation");
        }
        TMP_Timer.color = (time <= 30) ? lostTitleColor : pauseTitleColor;

        TMP_Combo.text = GameEventsManager.Instance.Combo > 0 ? $"Combo x{GameEventsManager.Instance.Combo}" : "";
        comboSloder.value = levelManager.ComboNormalized;

        TMP_Stars.text = "" + GameEventsManager.Instance.Stars;
        if (GameEventsManager.Instance.Paused)
        {
            TMP_Menu.text = "Game Paused";
            TMP_Menu.color = pauseTitleColor;
            gameUnpauseButton.SetActive(true);
            gameNextLevelButton.SetActive(false);
        }
        if (GameEventsManager.Instance.Won)
        {
            menu.SetActive(true);
            TMP_Menu.text = "Level Completed";
            TMP_Menu.color = winTitleColor;
            gameUnpauseButton.SetActive(false);
            gameNextLevelButton.SetActive(true);
        }
        if (GameEventsManager.Instance.Lost)
        {
            menu.SetActive(true);
            TMP_Menu.text = "Game Lost";
            TMP_Menu.color = lostTitleColor;
            gameUnpauseButton.SetActive(false);
            gameNextLevelButton.SetActive(false);
        }
        previousTime = levelManager.LevelRemainingTime;
    }
    #endregion
}
