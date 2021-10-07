using System;
using System.Collections;
using System.Collections.Generic;
using ReactorScripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using Random = System.Random;

public class RoomBlocks : MonoBehaviour
{
    [SerializeField]
    private List<Room> rooms;
    public int order;

    void LightOff()
    {
        var r = rooms.FindAll(room => room.isLight);
        if(r.Count != 0)
            r[new Random().Next(0, r.Count-1)].LightOff();
    }

    void LightOn()
    {
        var r = rooms.FindAll(room => room.isLight == false);
        if(r.Count != 0)
            r[new Random().Next(0, r.Count-1)].LightOn();
    }


}
