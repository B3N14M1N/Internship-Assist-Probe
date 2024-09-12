using UnityEditor;
using UnityEngine;
using static LevelBuilderManager;

[CustomEditor(typeof(LevelBuilderManager))]
public class LevelBuilderManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelBuilderManager t = (LevelBuilderManager)target;

        t.Level = EditorGUILayout.IntField("Level: ", t.Level);
        t.LevelTime = EditorGUILayout.FloatField("Level Solve Time: ", t.LevelTime);
        EditorGUILayout.Space(5);
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
            t.LoadLevel(t.Level);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(5);

        t.Difficulty = (LevelDifficultyMode)EditorGUILayout.EnumPopup("Level Difficulty:", t.Difficulty);
        var newitems = EditorGUILayout.IntField("Number of items: ", t.NumberOfItems);
        t.NumberOfItems = newitems > 0 ? newitems : t.NumberOfItems;
        EditorGUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Populate Containers"))
        {
            t.PopulateContainers();
        }
        if (GUILayout.Button("Clear Containers"))
        {
            t.ClearAllItems();
        }
        EditorGUILayout.EndHorizontal();


    }
}