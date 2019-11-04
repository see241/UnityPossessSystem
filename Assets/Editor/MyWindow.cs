using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MyWindow : EditorWindow
{
    private Bounds b;

    [MenuItem("Window/MyWindow/Open MyWindow")]
    private static void Open()
    {
        GetWindow<MyWindow>("MyWindow ");
    }

    private void OnGUI()
    {
        b = EditorGUILayout.BoundsField("", b);
    }
}