using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Action PickUp;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pickup"))
        {
            Debug.Log("Picked up" + other.name);
            Destroy(other.gameObject);
        }
    }
}
