using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExampleAtrribute
{
    [MenuItem("Example/Create Cube &g")]
    private static void CreateCube()
    {
        string applyString = "";
        for (int i = 0; i < 15; i++)
        {
            applyString += (char)Random.Range(0, 500);
        }
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = applyString;
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
    }

    [MenuItem("Example/Random Rotate &r")]
    private static void RandomRotate()
    {
        var transform = Selection.activeTransform;
        if (transform)
        {
            Undo.RecordObject(transform, "Rotate " + transform.name);
            transform.rotation = Random.rotation;
        }
    }
}