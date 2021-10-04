using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cooling : MonoBehaviour
{
    public SideDoor[] NeedDoorsToClose;
    public ParticleSystem particles;
    public float cooling;
    public LayerMask layerPlayer;
    public float timeRestore = 1f;

    private float lastTimeRestore;
    private bool isPlayerStay;
    private Weapon weapon;
    private bool isParticlePlayed;

    private void Start()
    {
        weapon = FindObjectOfType<Weapon>();
    }

    private void Update()
    {
        if (isPlayerStay && Time.time - lastTimeRestore > timeRestore && 
            NeedDoorsToClose.Count(door => door.isClose) == NeedDoorsToClose.Length)
        {
            lastTimeRestore = Time.time;
            weapon.Cooling(cooling);
            if (!isParticlePlayed)
            {
                isParticlePlayed = true;
                particles.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & layerPlayer) != 0)
        {
            isPlayerStay = true;
            isParticlePlayed = false;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & layerPlayer) != 0)
        {
            isPlayerStay = false;
        }
    }
}
