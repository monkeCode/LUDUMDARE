using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{ 
     public float fallMultiplier = 2.5f;
     public float lowJumpMultiplier = 2f;
     private PlayerInput _input = new PlayerInput();
     private Rigidbody2D rb;
     [Range(0,10)]
     public float jumpVelocity;
     void Start()
     {
          rb = GetComponent<Rigidbody2D>();
          _input.Enable();
          _input.input.Move.performed
     }
     
     void Move(int dir)
     {
          rb.velocity = new Vector2(Time.deltaTime * Speed * dir, rb.velocity.y);
          Debug.Log(dir);
     }
     void Update()
     {
          /*if (Input.GetButtonDown("Jump"))
          {

               rb.velocity = Vector2.up * jumpVelocity;
          }
          if(rb.velocity.y<0)
          {
               rb.velocity+=Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
          }
          else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
          {
               rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
          } */
          Move(_input.input.Move.ReadValue<int>());
     }
    
}
