using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomData
{
    public int roomNumber; // •”‰®”Ô†
    public bool isCleared; // ƒNƒŠƒAó‘Ô
    public bool isVisitor; // –K‚ê‚½‚©‚Ç‚¤‚©

    public RoomData(int number, bool cleared, bool visitored)
    {
        roomNumber = number;
        isCleared = cleared;
        isVisitor = visitored;
    }
}