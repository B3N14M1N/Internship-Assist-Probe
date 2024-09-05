using TMPro;
using UnityEngine;
using Zenject;

public class LevelDataDisplayManager : MonoBehaviour
{
    [Inject]
    private LevelPlayManager levelManager;

    public TMP_Text TMP_Level;
    public TMP_Text TMP_Timer;
    public TMP_Text TMP_Combo;
    public TMP_Text TMP_Stars;

    public void Awake()
    {
        Debug.Log(levelManager == null ? "Null" : "Injected");
    }

    public void Update()
    {
        TMP_Level.text = "Level: " + levelManager.Level;
        TMP_Timer.text = $"Time: {(int)(levelManager.LevelRemainingTime / 60)}:{(int)(levelManager.LevelRemainingTime % 60)}";
        TMP_Combo.text = levelManager.Combo > 0 ? $"Combo x{levelManager.Combo}" : "";
        TMP_Stars.text = "" + levelManager.Stars;
    }
}
