using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraManager))]
public class CameraManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CameraManager manager = (CameraManager)target;

        GUILayout.Space(10);
        GUILayout.Label("Debug Tools", EditorStyles.boldLabel);

        if (GUILayout.Button("Enable All Camera Holders"))
        {
            manager.EnableAllCameraHolders();
        }

        if (GUILayout.Button("Disable All Camera Holders"))
        {
            manager.DisableAllCameraHolders();
        }
    }
}
