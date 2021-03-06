using System;
using System.Collections;
using System.Collections.Generic;
using ReactorScripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Serialization;
using Random = System.Random;

public class RoomBlocks : MonoBehaviour
{
    [Serializable]
    class RoomBlock
    {
        public List<Light2D> Lights;
        public bool isLight;
        public List<SpawnTrigger> Triggers;
        public void LightOn()
        {
            foreach (var light in Lights)
            {
                light.intensity = 1;
            }
            foreach (var trigger in Triggers)
            {
                trigger.canSpawn = false;
            }
            isLight = true;
        }

      public  void LightOff()
        {
            foreach (var light in Lights)
            {
                light.intensity = 0;
            }

            foreach (var trigger in Triggers)
            {
                trigger.canSpawn = true;
            }
            isLight = false;
        }
    }

    [SerializeField] private List<RoomBlock> roomBlocks;
    private ReactorScripts.States _reactorState  = ReactorScripts.States.fullHP;
    private void Start()
    {
        Reactor.OnHealthChanged += StateChanged;
    }

    void StateChanged(object sender, ReactorEventHealth reactorEventHealth)
    {
        if (_reactorState != reactorEventHealth.State)
        {
            if ((int)_reactorState > (int)reactorEventHealth.State)
            {
                for(int i =0; i<(int)_reactorState - (int)reactorEventHealth.State; i++)
                LightOn();
            }
            else
            {
                for(int i =0; i<(int)reactorEventHealth.State -(int)_reactorState; i++)
                LightOff();
            }

            _reactorState = reactorEventHealth.State;
        }
    }
    
   void LightOff()
   {
       var r = roomBlocks.FindAll(room => room.isLight);
       if(r.Count != 0)
       r[new Random().Next(0, r.Count-1)].LightOff();
   }

   void LightOn()
   {
       var r = roomBlocks.FindAll(room => room.isLight == false);
       if(r.Count != 0)
       r[new Random().Next(0, r.Count-1)].LightOn();
   }
}
