using System.Collections;
using UnityEngine;

public class Player : Entity
{ 
     public float fallMultiplier = 2.5f;
     [Range(0, 10)] public float jumpVelocity;
     
     public Transform groundCheck;
     public float groundRadius;
     public LayerMask layerGrounds;

     private PlayerInput input;
     private new Rigidbody2D rigidbody;
     private SpriteRenderer spriteRenderer;
     private float movementX;
     private bool isGrounded;

     private void Awake()
     {
          rigidbody = GetComponent<Rigidbody2D>();
          spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
          input = new PlayerInput();
          input.input.Move.performed += ctx => Move(ctx.ReadValue<float>());
          input.input.Move.canceled += ctx => Move(0);
          input.input.jump.performed += ctx => Jump();
          input.input.jump.canceled += ctx => StartCoroutine(CanceledJump());
     }

     private void Update()
     {
          rigidbody.velocity = new Vector2(movementX, rigidbody.velocity.y);
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
     }

     private void Move(float axis)
     {
          if (axis != 0)
               spriteRenderer.flipX = axis < 0;
          movementX = axis * Speed;
     }

     private void OnEnable() => input.Enable();

     private void OnDisable() => input.Disable();
}