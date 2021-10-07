using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


class Room
    {
        public List<Light2D> Lights;
        public bool isLight { get; private set; } = true;
        public List<SpawnTrigger> Triggers;
        public List<SideDoor> doors;
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
            foreach (var door in doors)
            {
                if(door.isClose == false)
                    door.InteractWithDoor();
            }
        }
    }

