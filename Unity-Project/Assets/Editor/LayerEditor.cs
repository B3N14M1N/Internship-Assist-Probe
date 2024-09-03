using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Layer))]
public class LayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Layer t = target as Layer;
        DrawDefaultInspector();
        if (GUILayout.Button("Push Layer"))
        {
            t.PushLayer();
        }
        if (GUILayout.Button("Pull Layer"))
        {
            t.PullLayer();
        }
    }
}
