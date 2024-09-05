using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelBuilderManager))]
public class LevelBuilderManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelBuilderManager t = (LevelBuilderManager)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Add Container"))
        {
            t.AddContainer();
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            t.SaveLevel();
        }
        if (GUILayout.Button("Reset"))
        {
            t.ResetLevel();
        }
        if (GUILayout.Button("Load"))
        {
            t.LoadLevel();
        }
        EditorGUILayout.EndHorizontal();

    }
}