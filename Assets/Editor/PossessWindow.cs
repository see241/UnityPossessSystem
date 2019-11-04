using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

internal struct UndoData
{
    public Vector3 pos;
    public Quaternion rot;

    public UndoData(Vector3 p, Quaternion r)
    {
        pos = p;
        rot = r;
    }
}

public class PossessWindow : EditorWindow
{
    public static GameObject possessTarget;

    private bool isPossess;
    private GameObject applyPossessTarget;

    private Camera sceneviewCamera;

    private static PossessWindow possessWindow;

    private Vector3 returnPos;
    private Quaternion returnRot;

    private bool canUndo;

    private float range;

    private static bool freezePos;

    private static bool freezeRot;

    private static Dictionary<string, Stack<UndoData>> UndoDataDictionary = new Dictionary<string, Stack<UndoData>>();
    private static Dictionary<string, Stack<UndoData>> RedoDataDictionary = new Dictionary<string, Stack<UndoData>>();

    [MenuItem("Window/Possess Manager/Open Possess Manager &p")]
    private static void Open()
    {
        GetWindow<PossessWindow>("PossessWindow");
        UndoDataDictionary = new Dictionary<string, Stack<UndoData>>();
        RedoDataDictionary = new Dictionary<string, Stack<UndoData>>();
    }

    private void OnGUI()
    {
        possessTarget = EditorGUILayout.ObjectField(possessTarget, typeof(GameObject), true) as GameObject;
        range = EditorGUILayout.Slider("Range", range, 0.0f, 10.0f);
        GUI.enabled = possessTarget;
        if (GUILayout.Button(!isPossess ? "Possess" : "UnPossess"))
        {
            isPossess = !isPossess;
            if (isPossess)
            {
                if (possessTarget != null)
                {
                    if (UndoDataDictionary.ContainsKey(possessTarget.GetInstanceID() + "Transform"))
                    {
                        UndoDataDictionary[possessTarget.GetInstanceID() + "Transform"].Push(new UndoData(possessTarget.transform.position, possessTarget.transform.rotation));
                        Debug.Log("Cached Position : " + possessTarget.transform.position);
                    }
                    else
                    {
                        Stack<UndoData> undoDatas = new Stack<UndoData>();
                        undoDatas.Push(new UndoData(possessTarget.transform.position, possessTarget.transform.rotation));
                        UndoDataDictionary.Add(possessTarget.GetInstanceID() + "Transform", undoDatas);
                        Debug.Log("Cached Position : " + possessTarget.transform.position);
                    }
                }
                sceneviewCamera = SceneView.lastActiveSceneView.camera;
            }
            else
            {
            }
        }
        if (possessTarget != null)
            GUI.enabled = UndoDataDictionary.ContainsKey(possessTarget.GetInstanceID() + "Transform") && UndoDataDictionary[possessTarget.GetInstanceID() + "Transform"].Count > 0 && !isPossess;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Possess " + (possessTarget == null ? "Object" : possessTarget.name) + " Undo"))
        {
            isPossess = false;
            UndoData ud = UndoDataDictionary[possessTarget.GetInstanceID() + "Transform"].Pop();
            UndoData rd = new UndoData(possessTarget.transform.position, possessTarget.transform.rotation);
            possessTarget.transform.position = ud.pos;
            possessTarget.transform.rotation = ud.rot;
            if (RedoDataDictionary.ContainsKey(possessTarget.GetInstanceID() + "Transform"))
            {
                RedoDataDictionary[possessTarget.GetInstanceID() + "Transform"].Push(rd);
            }
            else
            {
                Stack<UndoData> redoDatas = new Stack<UndoData>();
                redoDatas.Push(rd);
                RedoDataDictionary.Add(possessTarget.GetInstanceID() + "Transform", redoDatas);
            }
        }
        if (possessTarget != null)
            GUI.enabled = RedoDataDictionary.ContainsKey(possessTarget.GetInstanceID() + "Transform") && RedoDataDictionary[possessTarget.GetInstanceID() + "Transform"].Count > 0 && !isPossess;
        if (GUILayout.Button("Possess " + (possessTarget == null ? "Object" : possessTarget.name) + " Redo"))
        {
            isPossess = false;
            UndoData rd = RedoDataDictionary[possessTarget.GetInstanceID() + "Transform"].Pop();
            UndoData ud = new UndoData(possessTarget.transform.position, possessTarget.transform.rotation);
            possessTarget.transform.position = rd.pos;
            possessTarget.transform.rotation = rd.rot;
            if (UndoDataDictionary.ContainsKey(possessTarget.GetInstanceID() + "Transform"))
            {
                UndoDataDictionary[possessTarget.GetInstanceID() + "Transform"].Push(ud);
            }
            else
            {
                Stack<UndoData> undoDatas = new Stack<UndoData>();
                undoDatas.Push(ud);
                UndoDataDictionary.Add(possessTarget.GetInstanceID() + "Transform", undoDatas);
            }
        }
        GUILayout.EndHorizontal();
        GUI.enabled = true;
        GUILayout.BeginHorizontal();
        freezePos = GUILayout.Toggle(freezePos, "Freeze Position");
        freezeRot = GUILayout.Toggle(freezeRot, "Freeze Rotation");
        GUILayout.EndHorizontal();
    }

    private void OnSelectionChange()
    {
        isPossess = false;
        possessTarget = Selection.activeGameObject;
        freezePos = false;
        freezeRot = false;
        Repaint();
    }

    private void Update()
    {
        if (isPossess)
        {
            if (possessTarget != null)
            {
                if (!freezePos)
                    possessTarget.transform.position = sceneviewCamera.transform.position + (sceneviewCamera.transform.forward * range);
                if (!freezeRot)
                    possessTarget.transform.rotation = sceneviewCamera.transform.rotation;
            }
            Repaint();
        }
    }

    [MenuItem("Window/Possess Manager/Freeze Rotation #r")]
    private static void FreezeRotation()
    {
        freezeRot = !freezeRot;
    }

    [MenuItem("Window/Possess Manager/Freeze Position #e")]
    private static void FreezePosition()
    {
        freezePos = !freezePos;
    }

    private void OnDisable()
    {
        Debug.Log("disable");
    }

    private void OnEnable()
    {
        Debug.Log("enable");
    }
}