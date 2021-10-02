using System.Collections;
using ReactorScripts;
using UnityEngine;
using System;

public class Player : Entity
{ 
     public float fallMultiplier = 2.5f;
     [Range(0, 10)] public float jumpVelocity; 
     public ItemData inventoryItem = new ItemData();
     
     public Transform groundCheck;
     public float groundRadius;
     public LayerMask layerGrounds;
     public float takeRadius;
     public LayerMask layerItem;
     public Camera mainCamera;
     public Weapon weapon;
    public LayerMask layerDoors;

     internal PlayerInput Input;
     
     private new Rigidbody2D rigidbody;
     private SpriteRenderer spriteRenderer;
     private float movementX;
     private bool isGrounded;

     private void Awake()
     {
          weapon = gameObject.AddComponent<Weapon>();
          rigidbody = GetComponent<Rigidbody2D>();
          spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
          Input = new PlayerInput();
          Input.Player.Move.performed += ctx => Move(ctx.ReadValue<float>());
          Input.Player.Move.canceled += ctx => Move(0);
          Input.Player.Jump.performed += ctx => Jump();
          Input.Player.Jump.canceled += ctx => StartCoroutine(CanceledJump());
          Input.Player.Action.performed += ctx => TakeItem();
          Input.Player.Throw.performed += ctx => ThrowItem();
          Input.Player.Shot.performed += ctx => Shot();
          Input.Player.OpenDoor.performed += ctx => OpenDoor();
    }
     
     private void Jump()
     {
          if (isGrounded)
               rigidbody.velocity = new Vector2(movementX, jumpVelocity);
     }
     
     private IEnumerator CanceledJump()
     {
          while (rigidbody.velocity.y > 0)
          {
               rigidbody.velocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
               yield return null;
          }
     }

     private void FixedUpdate()
     {
          isGrounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, layerGrounds);
          rigidbody.velocity = new Vector2(movementX, rigidbody.velocity.y);
     }

     private void TakeItem()
     {
          var item = Physics2D.OverlapCircle (groundCheck.position, takeRadius, layerItem);
          if (item != null)
          {
               inventoryItem = item.GetComponent<Item>().data;
               Destroy(item.gameObject);
          }
     }

     private void ThrowItem()
     {
          if (inventoryItem != null) 
               Debug.Log($"Throw {inventoryItem.name} {inventoryItem.type}");
          inventoryItem.type = TypeItem.Item0;
     }

    private void OpenDoor()
    {
        var doorCollider = Physics2D.OverlapCircle(groundCheck.position, takeRadius, layerDoors);
        if(doorCollider!= null)
        {
            StartCoroutine(EnterDoor(doorCollider ));  
        }
    }

    IEnumerator EnterDoor(Collider2D doorCollider)
    {
        var door = doorCollider.GetComponent<Door>();
        var Out = door.Out;
        door.State = States.open;
        OnDisable();
        yield return new WaitForSeconds(1);
        spriteRenderer.sortingOrder = 4;
        yield return new WaitForSeconds(1);
        transform.position = Out.transform.position;
        var otherDoor = Out.GetComponentInParent<Door>();
        otherDoor.State = States.open;
        yield return new WaitForSeconds(1);
        spriteRenderer.sortingOrder = 6;
        yield return new WaitForSeconds(1);
        OnEnable();
    }

     private void Move(float axis)
     {
          if (axis != 0)
               spriteRenderer.flipX = axis < 0;
          movementX = axis * Speed;
     }
     
     public Vector3 GetVectorToMouse() => 
          mainCamera.ScreenToWorldPoint(Input.Mouse.Move.ReadValue<Vector2>()) - transform.position;

     private void Shot()
     {
          var vector = GetVectorToMouse();
          weapon.Shoot(vector, inventoryItem);
     }

     private void OnEnable() => Input.Enable();

     private void OnDisable() => Input.Disable();
}