using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SideDoor : MonoBehaviour
{
    [SerializeField] private BoxCollider2D blockCollider;
 private bool isClose = true;
 private Animator _animator;
 private BoxCollider2D _collider;
 private static readonly int Close = Animator.StringToHash("close");
 private static readonly int Open = Animator.StringToHash("open");
   public AudioSource sound;

    void Start()
    {
      _animator = GetComponent<Animator>();
      _collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame

    public void InteractWithDoor()
   {
       isClose = !isClose;
       if(isClose)
            _animator.SetTrigger(Close);
       else 
           _animator.SetTrigger(Open);
        sound.Play();
       blockCollider.enabled = isClose;
   }
   
}
