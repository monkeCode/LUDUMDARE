using System.Collections;
using ReactorScripts;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Experimental.Rendering.Universal;

public class Player : Entity
{
    public float fallMultiplier = 2.5f;
    [Range(0, 10)] public float jumpVelocity;
    public ItemData inventoryItem = new ItemData();
    public SpriteRenderer weaponSprite;

    public Transform groundCheck;
    public float groundRadius;
    public LayerMask layerGrounds;
    public float takeRadius;
    public LayerMask layerItem;
    public Camera mainCamera;
    public Weapon weapon;
    public LayerMask layerDoors;
    public LayerMask layerReactor;
    // public LayerMask layerSideDoors;
    public SpawnItemInspector itemInspector;
    public static List<string> Keys;
    public LayerMask layerKeys;
    public Light2D LeftLight;
    public Light2D RightLight;
    public AudioSource sound;
    public AudioClip FlamethrowerSound;
    public AudioClip LaserSound;
    public AudioClip GunSound;
    private bool isDelay;

    public float damageInterval;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform rotatePoint;

    [SerializeField] internal PlayerInput Input;
     
     private new Rigidbody2D rigidbody;
     private SpriteRenderer spriteRenderer;
     private float movementX;
     private bool isGrounded;
     private bool isShoted;
     private bool _canTakeDamage = true;

     public event EventHandler<ItemData> itemChanged;  

     private void Awake()
     {
          Keys = new List<string>();
          weapon = gameObject.GetComponent<Weapon>();
          weapon.weaponPos = attackPoint.transform;
          rigidbody = GetComponent<Rigidbody2D>();
          spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
          Input = new PlayerInput();
          Input.Player.Move.performed += ctx => Move(ctx.ReadValue<float>());
          Input.Player.Move.canceled += ctx => Move(0);
          Input.Player.Jump.performed += ctx => Jump();
          Input.Player.Jump.canceled += ctx => StartCoroutine(CanceledJump());
          Input.Player.Action.performed += ctx => TakeItem();
          Input.Player.Action.performed += ctx => ReactorRepair();
          Input.Player.Throw.performed += ctx => ThrowItem();
          Input.Player.Shot.performed += ctx => isShoted = true;
          Input.Player.Shot.canceled += ctx => isShoted = false;
          Input.Player.OpenDoor.performed += ctx => OpenDoor();
          Input.Player.Action.performed += ctx => TakeKey();
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
          var isRight = -180 < angle && angle < 0;
        LeftLight.enabled = !isRight;
        RightLight.enabled = isRight;
        rotatePoint.rotation = Quaternion.Euler(0, 0, angle);
          weaponSprite.flipY = !isRight;
          spriteRenderer.flipX = !isRight;

     }

     private void TakeItem()
     { 
          var itemCollider = Physics2D.OverlapCircle (groundCheck.position, takeRadius, layerItem);
          if (itemCollider != null)
          {
            var item = itemCollider.GetComponent<Item>();
            inventoryItem = item.data;
            itemChanged?.Invoke(this, inventoryItem);
            itemInspector.TakeItem(item);
            Destroy(itemCollider.gameObject);
          }
     }

    private void ThrowItem()
     {
          if (inventoryItem != null) 
               Debug.Log($"Throw {inventoryItem.name} {inventoryItem.type}");
          inventoryItem.type = TypeItem.Default;
          itemChanged?.Invoke(this, inventoryItem);
     }

    private void TakeKey()
    {
         var keyCollider = Physics2D.OverlapCircle(groundCheck.position, takeRadius, layerKeys);
         if (keyCollider != null)
         {
              Keys.Add(keyCollider.GetComponent<Key>().name);
              Destroy(keyCollider.gameObject);
         }
    }

    private void ReactorRepair()
    {
         var reactor = Physics2D.OverlapCircle (transform.position, takeRadius, layerReactor);
         if (reactor != null)
         {
              var reactorScript = reactor.GetComponent<Reactor>();
              reactorScript.GetRepair(inventoryItem);
              if (!reactorScript.isRequested) 
                   ThrowItem();
         }
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
        var doorIsOpening = door.TryOpen();
        if (doorIsOpening)
        {
             if (Out != null)
             {
                  _canTakeDamage = false;
                  OnDisable();
                  yield return new WaitForSeconds(1);
                  spriteRenderer.sortingOrder = 4;
                  yield return new WaitForSeconds(1);
                  transform.position = Out.transform.position;
                  var otherDoor = Out.GetComponentInParent<Door>();
                  otherDoor.Open();
                  yield return new WaitForSeconds(1);
                  spriteRenderer.sortingOrder = 8;
                  yield return new WaitForSeconds(1);
                  OnEnable();
                  _canTakeDamage = true;
             }
             else
             {
                  StartCoroutine(EnterSideDoor(doorCollider));
             }
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
         _canTakeDamage = false;
         OnDisable(); 
         yield return new WaitForSeconds(1); 
         OnEnable();
         _canTakeDamage = true;
    }
    private void Move(float axis)
     {
          if (axis != 0) {
               
          }
          movementX = axis * Speed;
     }
     
     public Vector3 GetVectorToMouse() => 
          mainCamera.ScreenToWorldPoint(Input.Mouse.Move.ReadValue<Vector2>()) - transform.position;

     private void Shot()
     {
            var vector = GetVectorToMouse();
            if (!weapon.onCooldown)
        {
            if (inventoryItem.type == TypeItem.Default)
                sound.PlayOneShot(GunSound);
            if (inventoryItem.type == TypeItem.Flamethrower && !isDelay)
            {
                isDelay = true;
                StartCoroutine(playSound(FlamethrowerSound));
            }
            if (inventoryItem.type == TypeItem.Laser && !isDelay)
            {
                isDelay = true;
                StartCoroutine(playSound(LaserSound));
            }
                
        }
            
        weapon.Shoot(vector, inventoryItem);
     }

    IEnumerator playSound(AudioClip soundclip)
    {
        yield return new WaitForSeconds(0.3f);
        sound.PlayOneShot(soundclip);
        isDelay = false;
    }

     public void OnEnable() => Input.Enable();

     public void OnDisable() => Input.Disable();

     public override void TakeDamage(int damage)
     {
          if (_canTakeDamage)
          {
               base.TakeDamage(damage);
               _canTakeDamage = false;
               StartCoroutine(WaitInterDamage());
          }
     }

     IEnumerator WaitInterDamage()
     {
          spriteRenderer.color = Color.red;
          yield return new WaitForSeconds(damageInterval/3);
          spriteRenderer.color = Color.white;
          yield return new WaitForSeconds(damageInterval/3);
          spriteRenderer.color = Color.red;
          yield return new WaitForSeconds(damageInterval/3);
          spriteRenderer.color = Color.white;
          _canTakeDamage = true;
     }
     protected override void Die()
     {
          ShipController ShipControllerScript = GameObject.Find("ShipController").GetComponent<ShipController>();
          ShipControllerScript.DeadMenu.SetActive(true);
          OnDisable();
          Time.timeScale = 0;
     }
}