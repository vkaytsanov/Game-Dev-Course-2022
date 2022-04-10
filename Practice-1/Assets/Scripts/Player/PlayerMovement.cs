using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 40f;

    private Rigidbody2D rigidBody;
    private float horizontalInput;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        ResolveLookDirection();

        //Translate movement
        /*
        Vector3 movement = Time.deltaTime * new Vector3(horizontalInput * moveSpeed,0,0);
        Debug.Log(movement);
        transform.Translate(movement);
        */

        //change position
        /*
        Vector3 movement = Time.deltaTime * new Vector3(horizontalInput * moveSpeed, 0, 0);
        transform.position = transform.position + movement;
        */
    }

   

    void FixedUpdate ()
    {
        // Move our character
        // Change velocity
        /*
        if (horizontalInput != 0)
        {
            Vector2 target = rigidBody.velocity;
            target.x = horizontalInput * moveSpeed;
            rigidBody.velocity = (target);
        }
        */
        
        if(horizontalInput != 0)
        {
            
            Vector2 force = new Vector2(horizontalInput * moveSpeed,0);
            if (rigidBody.velocity.x < 10)
            {
                rigidBody.AddForce(force);
            }
        }
        
        /*
        if(horizontalInput != 0)
        {
            Vector2 nextPosition = rigidBody.position + Time.fixedDeltaTime * new Vector2(horizontalInput * moveSpeed , rigidBody.velocity.y);
            rigidBody.MovePosition(nextPosition);
        }
        */
    }

    void ResolveLookDirection()
    {
        if (Abs(rigidBody.velocity.x) > 0.01f) 
        {
            transform.localScale = new Vector3(Sign(rigidBody.velocity.x), 1, 1);
        }
    }

}
