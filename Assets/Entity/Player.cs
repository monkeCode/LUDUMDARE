using System.Collections;
using UnityEngine;

public class Player : Entity
{ 
     public float fallMultiplier = 2.5f;
     [Range(0, 10)] public float jumpVelocity;
     
     public Transform groundCheck;
     public float groundRadius;
     public LayerMask layerGrounds;
     public Camera mainCamera;
     
     internal PlayerInput Input;
     
     private new Rigidbody2D rigidbody;
     private SpriteRenderer spriteRenderer;
     private float movementX;
     private bool isGrounded;

     private void Awake()
     {
          rigidbody = GetComponent<Rigidbody2D>();
          spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
          Input = new PlayerInput();
          Input.Player.Move.performed += ctx => Move(ctx.ReadValue<float>());
          Input.Player.Move.canceled += ctx => Move(0);
          Input.Player.Jump.performed += ctx => Jump();
          Input.Player.Jump.canceled += ctx => StartCoroutine(CanceledJump());
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

     private void Move(float axis)
     {
          if (axis != 0)
               spriteRenderer.flipX = axis < 0;
          movementX = axis * Speed;
     }
     
     public Vector3 GetVectorToMouse() => 
          mainCamera.ScreenToWorldPoint(Input.Mouse.Move.ReadValue<Vector2>()) - transform.position;
     
     public float GetAngleToMouse()
     {
          var vector = mainCamera.ScreenToWorldPoint(Input.Mouse.Move.ReadValue<Vector2>()) - transform.position;
          var angle = Mathf.Atan2(vector.y, vector.x);
          return angle * Mathf.Rad2Deg - 90f;
     }

     private void OnEnable() => Input.Enable();

     private void OnDisable() => Input.Disable();
}