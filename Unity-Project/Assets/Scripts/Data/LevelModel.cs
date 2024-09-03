using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelModel
{
    [SerializeField]
    public int level;
    [SerializeField]
    public float LevelTime;
    [SerializeField]
    public List<Container> containers;
}