using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayerMask;
    public float speed = 100f;
    private float horizontalMove=0f;
    public float jumpStrength = 10f;
    private Rigidbody2D rb;
    private CircleCollider2D collider2D;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        collider2D = gameObject.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        
        //jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            float verticalMove = jumpStrength;
            rb.velocity = Vector2.up * verticalMove;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        rb.velocity = new Vector2(speed * Time.fixedDeltaTime, rb.velocity.y);
    }

    bool IsGrounded()
    {
        float extraHeightText = 0.01f;
        RaycastHit2D raycastHit =  Physics2D.Raycast(collider2D.bounds.center, Vector2.down,
            collider2D.bounds.extents.y + extraHeightText,
            groundLayerMask);

        /*
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else 
            rayColor = Color.red;
        
        Debug.Log(raycastHit);
        Debug.DrawRay(collider2D.bounds.center,Vector2.down * 
                                                  (collider2D.bounds.extents.y + extraHeightText),rayColor);
        */
        return raycastHit.collider != null;
    }

    public void Die()
    {
        Debug.Log("Game Over!");
        //Time.timeScale = 0f;
    }
}
