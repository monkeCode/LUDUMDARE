using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class SideDoor : RootDoor
{
    [SerializeField] private BoxCollider2D blockCollider;
    internal bool isClose = true;
    private Animator _animator;
    private BoxCollider2D _collider;
    private static readonly int Close = Animator.StringToHash("close");
    private static readonly int OpenAnim = Animator.StringToHash("open");
    public AudioSource sound;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
    }

    public override void Open()
    {
        InteractWithDoor();
    }

    public void InteractWithDoor()
    {
        isClose = !isClose;
        if (isClose)
            _animator.SetTrigger(Close);
        else
            _animator.SetTrigger(OpenAnim);
        sound.Play();
        blockCollider.enabled = isClose;
    }
}
