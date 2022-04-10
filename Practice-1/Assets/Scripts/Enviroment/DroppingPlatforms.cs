using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingPlatforms : MonoBehaviour
{
    private Rigidbody2D rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Unfreeze");
            rb.constraints = RigidbodyConstraints2D.None;
            rb.isKinematic = false;
            rb.gravityScale = 0.2f;
        }

        if (collision.gameObject.layer == 8)
        {
            //Debug.Log("Destroy");
            Destroy(this.gameObject);
        }
    }
}
