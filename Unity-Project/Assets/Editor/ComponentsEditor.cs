using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SlotMono))]
public class SlotMonoEditor : Editor
{
    public override void OnInspectorGUI()
    {

        SlotMono t = (SlotMono)target;
        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);
        EditorGUILayout.Space(5);

        t.ItemId = EditorGUILayout.IntField("Item Id: ", t.ItemId);
        EditorGUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        if (Application.isEditor && !Application.isPlaying )
            GUI.enabled = false;
        if (GUILayout.Button("Load Data"))
        {
            t.LoadData(t.ItemId);
        }
        GUI.enabled = true;

        if (GUILayout.Button("Delete Slot"))
        {
            t.RemoveSlot();
        }
        EditorGUILayout.EndHorizontal();
    }
}
[CustomEditor(typeof(LayerMono))]
public class LayerMonoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LayerMono t = (LayerMono)target;

        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);

        t.layerOrder = EditorGUILayout.IntField("Layer Order: ", t.layerOrder);
        t.status = (LayerStatus)EditorGUILayout.EnumPopup("Layer Status: ", t.status);
        EditorGUILayout.Space(5);

        EditorGUIUtility.labelWidth = 45.0f;
        EditorGUILayout.BeginHorizontal();
        t.Slot1 = EditorGUILayout.ObjectField("Slot 1:", t.Slot1, typeof(SlotMono),true) as SlotMono;
        if (GUILayout.Button("Add Slot"))
        {
            if(t.Slot1 == null)
            {
                t.Slot1 = SlotMono.InitializeNew(new Slot(), t.transform, false);
                t.Slot1.name = "Slot 1";
            }

        }
        if (GUILayout.Button("Remove Slot"))
        {
            if (t.Slot1 != null)
            {
                t.Slot1.RemoveSlot();
            }
        }
        if (GUILayout.Button("Detach Slot"))
        {
            if (t.Slot1 != null)
            {
                t.Slot1.transform.parent = null;
                t.Slot1 = null;
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        t.Slot2 = EditorGUILayout.ObjectField("Slot 2:", t.Slot2, typeof(SlotMono), true) as SlotMono;
        if (GUILayout.Button("Add Slot"))
        {
            if (t.Slot2 == null)
            {
                t.Slot2 = SlotMono.InitializeNew(new Slot(), t.transform, false);
                t.Slot2.name = "Slot 2";
            }

        }
        if (GUILayout.Button("Remove Slot"))
        {
            if (t.Slot2 != null)
            {
                t.Slot2.RemoveSlot();
            }
        }
        if (GUILayout.Button("Detach Slot"))
        {
            if (t.Slot2 != null)
            {
                t.Slot2.transform.parent = null;
                t.Slot2 = null;
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        t.Slot3 = EditorGUILayout.ObjectField("Slot 1:", t.Slot3, typeof(SlotMono), true) as SlotMono;
        if (GUILayout.Button("Add Slot"))
        {
            if (t.Slot3 == null)
            {
                t.Slot3 = SlotMono.InitializeNew(new Slot(), t.transform, false);
                t.Slot3.name = "Slot 3";
            }

        }
        if (GUILayout.Button("Remove Slot"))
        {
            if (t.Slot3 != null)
            {
                t.Slot3.RemoveSlot();
            }
        }
        if (GUILayout.Button("Detach Slot"))
        {
            if (t.Slot3 != null)
            {
                t.Slot3.transform.parent = null;
                t.Slot3 = null;
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Push Layer"))
        {
            t.PushLayer();
        }
        if (GUILayout.Button("Pull Layer"))
        {
            t.PullLayer();
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Remove Layer"))
        {
            t.RemoveLayer();
        }
    }
}

[CustomEditor(typeof(ContainerMono))]
public class ContainerMonoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ContainerMono t = (ContainerMono)target;
        DrawDefaultInspector();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Layer"))
        {
            t.AddLayer();
        }
        if (GUILayout.Button("Rearange Layers"))
        {
            t.RearangeLayers();
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Remove Container"))
        {
            t.RemoveContainer();
        }
    }
}
