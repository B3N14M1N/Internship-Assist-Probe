
using System;
using UnityEngine;

[Serializable]
public class Slot
{
    public int gameItemId;

    public static Slot Empty => new Slot() { gameItemId = 0 };
    public static Slot ToModel(SlotMono slotMono)
        => new Slot() 
        {
            gameItemId = slotMono.ItemId
        };
}
