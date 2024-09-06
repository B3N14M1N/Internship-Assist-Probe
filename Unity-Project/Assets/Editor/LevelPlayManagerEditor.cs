using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelPlayManager))]
public class LevelPlayManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelPlayManager t = (LevelPlayManager)target;
        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);
        EditorGUILayout.Space(5);

        GUI.enabled = false;
        EditorGUIUtility.labelWidth = 100.0f;
        EditorGUILayout.IntField("Level: ", t.Level);
        EditorGUILayout.FloatField("Starting Time", t.LevelTime);
        if(EditorGUILayout.Toggle("Initialized: ", t.Initialized))
        {
            EditorGUILayout.Toggle("Started: ", t.Started);
            EditorGUILayout.Toggle("Paused: ", t.Paused);
        }
        GUI.enabled = true;
        EditorGUIUtility.labelWidth = 0;

        EditorGUILayout.Space(5);
        if (Application.isEditor && !Application.isPlaying)
            GUI.enabled = false;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Start Game"))
        {
            t.StartLevel();
        }
        if (GUILayout.Button("Pause Game"))
        {
            t.PauseLevel(!t.Paused);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("End Game"))
        {
            t.EndLevel();
        }
        if (GUILayout.Button("RestartGame"))
        {
            t.ResetLevel();
        }
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;
    }
}