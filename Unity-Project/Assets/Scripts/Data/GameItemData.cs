using System;
using UnityEngine;

[Serializable]
public class GameItemData
{
    [SerializeField]
    public int gameItemId;
    [SerializeField]
    public Sprite sprite;
    [SerializeField]
    public Vector3 position;
    [SerializeField]
    public Vector3 scale;
}
