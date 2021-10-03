using System.Collections;
using ReactorScripts;
using UnityEngine;
using System;
using System.Collections.Generic;

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
    // public LayerMask layerSideDoors;
    public SpawnItemInspector itemInspector;
    public static List<string> Keys;


    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform rotatePoint;

    [SerializeField] internal PlayerInput Input;
     
     private new Rigidbody2D rigidbody;
     private SpriteRenderer spriteRenderer;
     private float movementX;
     private bool isGrounded;
     private bool isShoted;

     private void Awake()
     {
          Keys = new List<string>();
          weapon = gameObject.AddComponent<Weapon>();
          weapon.weaponPos = attackPoint.transform;
          rigidbody = GetComponent<Rigidbody2D>();
          spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
          Input = new PlayerInput();
        Debug.Log("PlayerInput");
          Input.Player.Move.performed += ctx => Move(ctx.ReadValue<float>());
          Input.Player.Move.canceled += ctx => Move(0);
          Input.Player.Jump.performed += ctx => Jump();
          Input.Player.Jump.canceled += ctx => StartCoroutine(CanceledJump());
          Input.Player.Action.performed += ctx => TakeItem();
          Input.Player.Throw.performed += ctx => ThrowItem();
          Input.Player.Shot.performed += ctx => isShoted = true;
          Input.Player.Shot.canceled += ctx => isShoted = false;
          Input.Player.OpenDoor.performed += ctx => OpenDoor();
          // Input.Player.OpenDoor.performed += ctx => InteractSideDoor();
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
          if (isShoted)
               Shot();
          isGrounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, layerGrounds);
          rigidbody.velocity = new Vector2(movementX, rigidbody.velocity.y);
          var vector = GetVectorToMouse();
          var angle = Mathf.Atan2(vector.y, vector.x)  * Mathf.Rad2Deg - 90f;
          rotatePoint.rotation = Quaternion.Euler(0, 0, angle);
     }

     private void TakeItem()
     {
         var itemCollider = Physics2D.OverlapCircle (groundCheck.position, takeRadius, layerItem);
          if (itemCollider != null)
          {
            var item = itemCollider.GetComponent<Item>();
            Debug.Log("1");
            inventoryItem = item.data;
            Debug.Log("2");
            itemInspector.TakeItem(item);
            Debug.Log("3");
            Destroy(itemCollider.gameObject);
          }
     }

    private void ThrowItem()
     {
          if (inventoryItem != null) 
               Debug.Log($"Throw {inventoryItem.name} {inventoryItem.type}");
          inventoryItem.type = TypeItem.Default;
     }

    private void OpenDoor()
    {
        var doorCollider = Physics2D.OverlapCircle(groundCheck.position, takeRadius, layerDoors);
        if(doorCollider != null)
        {
            StartCoroutine(EnterDoor(doorCollider ));  
        }
    }

    
    IEnumerator EnterDoor(Collider2D doorCollider)
    {
        // var door = doorCollider.GetComponent<Door>();
        var door = doorCollider.GetComponent<RootDoor>();
        var Out = door.@out;
        door.TryOpen();
        if (Out != null)
        {
             OnDisable();
             yield return new WaitForSeconds(1);
             spriteRenderer.sortingOrder = 4;
             yield return new WaitForSeconds(1);
             transform.position = Out.transform.position;
             var otherDoor = Out.GetComponentInParent<Door>();
             otherDoor.Open();
             yield return new WaitForSeconds(1);
             spriteRenderer.sortingOrder = 6;
             yield return new WaitForSeconds(1);
             OnEnable();
        }
        else
        {
             StartCoroutine(EnterSideDoor(doorCollider));
        }
    }

    // void InteractSideDoor()
    // {
    //      var doorCollider = Physics2D.OverlapCircle(transform.position, takeRadius, layerSideDoors);
    //      if (doorCollider != null)
    //      {
    //         StartCoroutine(EnterSideDoor(doorCollider));
    //          ;
    //      }
    // }
    IEnumerator EnterSideDoor(Collider2D doorCollider)
    { 
         OnDisable();
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