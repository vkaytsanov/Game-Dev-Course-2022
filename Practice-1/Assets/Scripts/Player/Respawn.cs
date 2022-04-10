using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Vector3 spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //respawns the player to starting location
        if (collision.gameObject.CompareTag("Map End"))
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            transform.position = spawnPoint;

        }
    }

}
