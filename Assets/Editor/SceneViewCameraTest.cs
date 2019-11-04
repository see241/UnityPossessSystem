using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SceneViewCameraTest : EditorWindow
{
    [MenuItem("Window/SceneViewCameraTest")]
    private static void Init()
    {
        // Get existing open window or if none, make a new one:
        SceneViewCameraTest window = (SceneViewCameraTest)EditorWindow.GetWindow(typeof(SceneViewCameraTest));
    }

    private void OnGUI()
    {
        EditorGUILayout.TextField("SceneViewCameraAngle", "" + SceneView.lastActiveSceneView.rotation);
    }
}