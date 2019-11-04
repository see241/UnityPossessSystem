using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    [SerializeField, Range(0, 100)]
    private int hp;

    [SerializeField]
    private Character character;
}

[System.Serializable]
public class Character
{
    [SerializeField]
    private string name;

    [SerializeField]
    private int hp;
}