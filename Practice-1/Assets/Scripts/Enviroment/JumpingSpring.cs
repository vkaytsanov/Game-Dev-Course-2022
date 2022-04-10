using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingSpring : MonoBehaviour
{
    public float force;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Rigidbody2D rb = collider.gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
    }
}
