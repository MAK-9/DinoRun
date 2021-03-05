using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyObstacle : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerController.Die();
        }
    }
}
