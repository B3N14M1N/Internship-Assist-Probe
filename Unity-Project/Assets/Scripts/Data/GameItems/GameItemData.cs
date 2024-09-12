using System;
using UnityEngine;

// this could've been a scriptableobject
[Serializable]
public class GameItemData
{
    public int GameItemId;
    public Sprite Sprite;
    public Vector3 Position;
    public Vector3 Scale;
}
