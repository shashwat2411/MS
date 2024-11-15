using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    [SerializeField]
    private RoomCore[] rooms;

    public RoomCore[] GetRooms() { return rooms; }
}
