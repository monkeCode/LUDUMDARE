using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Door : RootDoor
{
    // public GameObject Out;
    public AudioSource sound;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public override void Open()
    {
        State = States.open;
        sound.Play();
    }

    public void Close()
    {
        State = States.close;
    }

    public void Idle()
    {
        State = States.idle;
    }

    public States State
    {
        get
        {
            return (States)animator.GetInteger("state");
        }
        set
        {
            animator.SetInteger("state", (int)value);
        }
    }
}

public enum States
{
    idle,
    open, 
    close
}
