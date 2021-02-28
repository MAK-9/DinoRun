using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloudController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float cloudSpeed;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        cloudSpeed = Random.Range(0f, 2f);
        
        rb.velocity = new Vector2(-cloudSpeed, 0);
    }
}
