using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayerMask;
    public float speed = 100f;
    private float dashDuration = 0.5f;
    private float dashStrength = 300f;
    private float horizontalMove=0f;
    public float jumpStrength = 10f;

    private float dashCooldown = 2f;

    private bool dead = false;
    private bool dashReady = true;
    
    private Rigidbody2D rb;
    private new CircleCollider2D collider2D;
    private BoxCollider2D boxCollider2D;
    private Animator animator;

    enum AbilityType
    {
        DASH
    };
    
    
    // Start is called before the first frame update
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        collider2D = gameObject.GetComponent<CircleCollider2D>();
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        
        //jump
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) && IsGrounded() && !dead)
        {
            Jump();
        }
        //dash
        if (Input.GetKeyDown(KeyCode.RightArrow) && dashReady)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void FixedUpdate()
    {
        if(!dead)
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

    void Jump()
    {
        float verticalMove = jumpStrength;
        rb.velocity = Vector2.up * verticalMove;
    }

    IEnumerator Dash()
    {
        speed += dashStrength;
        dashReady = false;
        
        animator.SetBool("Dash",true);
        SwapColliders();
        yield return new WaitForSeconds(dashDuration);
        animator.SetBool("Dash",false);
        SwapColliders();
        
        
        speed -= dashStrength;
        StartCoroutine(AbilityCooldown(AbilityType.DASH));
    }

    IEnumerator AbilityCooldown(AbilityType type = AbilityType.DASH)
    {
        switch (type)
        {
            case AbilityType.DASH:
                yield return new WaitForSeconds(dashCooldown);
                dashReady = true;
                break;
        }
    }

    void SwapColliders()
    {
        collider2D.enabled = !collider2D.enabled;
        boxCollider2D.enabled = !boxCollider2D.enabled;
    }

    public void Die()
    {
        if (!dead)
        {
            Debug.Log("Game Over!");
            //Time.timeScale = 0f;

            dead = true;
            animator.SetTrigger("Hurt");
            Jump();
            animator.SetTrigger("Dead");
        }
        
    }
}
