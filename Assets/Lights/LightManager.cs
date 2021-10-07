using System.Collections;
using System.Collections.Generic;
using ReactorScripts;
using UnityEngine;
using Random = System.Random;

public class LightManager : MonoBehaviour
{
    private ReactorScripts.States _reactorState  = ReactorScripts.States.fullHP;
    private int _maxHpReactor;
    [SerializeField] private List<RoomBlocks> roomBlocksList;
    private void Start()
    {
        Reactor.OnHealthChanged += StateChanged;
        _maxHpReactor = Reactor.reactor.maxHealth;
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
    
    
}
