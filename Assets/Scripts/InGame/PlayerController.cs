using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class PlayerController : MonoBehaviour
{
    //private Dash dash;
    public DashMeter dashMeter;
    [SerializeField] private LayerMask groundLayerMask;
    public float speed = 300f;
    private float dashDuration = 0.5f;
    private float dashStrength = 300f;
    private float horizontalMove=0f;
    public float jumpStrength = 10f;
    
    // dash text ready
    public TMP_Text dashReadyText;

    private float dashCooldown = 2f;

    public bool dead = false;
    private bool dashReady = true;
    private bool immune = false;
    
    private Rigidbody2D rb;
    private new CircleCollider2D collider2D;
    private BoxCollider2D boxCollider2D;
    private Animator animator;

    public GameController gameController;

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
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space)) && IsGrounded() && !dead)
        {
            Jump();
        }
        //dash
        if (Input.GetKey(KeyCode.RightArrow))
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("GameScene");
        }
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        HandleTouchInput();
    }

    private void FixedUpdate()
    {
        if(!dead)
            Move();
    }

    void HandleTouchInput()
    {
        foreach (Touch touch in Input.touches) {
            if((touch.position.x <= Screen.width / 2) && (touch.phase == TouchPhase.Stationary) && IsGrounded() && !dead) {
                Jump();
            }
  
            if((touch.position.x > Screen.width / 2)&& (touch.position.y < Screen.height * 3/4) && (touch.phase == TouchPhase.Began) && !dead)
            {
                StartCoroutine(Dash());
            }
        }
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
        RaycastHit2D raycastHit2 = Physics2D.Raycast(new Vector2(collider2D.bounds.center.x + collider2D.radius,collider2D.bounds.center.y), Vector2.down,
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
        
        if (raycastHit2.collider != null)
        {
            rayColor = Color.green;
        }
        else 
            rayColor = Color.red;
        
        Debug.Log(raycastHit2);
        Debug.DrawRay(new Vector2(collider2D.bounds.center.x + collider2D.bounds.extents.x,collider2D.bounds.center.y),Vector2.down * 
                                                (collider2D.bounds.extents.y + extraHeightText),rayColor);
        */
        
        //return raycastHit.collider != null;
        return (raycastHit.collider != null || raycastHit2.collider != null);
    }

    void Jump()
    {
        float verticalMove = jumpStrength;
        rb.velocity = Vector2.up * verticalMove;
    }

    IEnumerator Dash()
    {
        if (!dashMeter.TryToDash())
        {
            Debug.Log("dash quit");
            yield break;
        }
        
        speed += dashStrength;
        dashReady = false;
        immune = true;
        
        animator.SetBool("Dash",true);
        SwapColliders();
        yield return new WaitForSeconds(dashDuration);
        animator.SetBool("Dash",false);
        SwapColliders();

        immune = false;
        speed -= dashStrength;
        //StartCoroutine(AbilityCooldown(AbilityType.DASH));
    }

    void SwitchDashText(bool isReady)
    {
        if (isReady)
        {
            dashReadyText.color=Color.green;
        }
        else dashReadyText.color = Color.red;
    }

    //legacy function
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

    public void IncreaseSpeed(float amt)
    {
        speed += amt;
    }
    public void Die(bool avoidable = false)
    {
        if (!dead && !immune ||
            !dead && !avoidable)
        {
            //Debug.Log("Game Over!");
            //Time.timeScale = 0f;

            dead = true;
            animator.SetTrigger("Hurt");
            Jump();
            animator.SetTrigger("Dead");
            
            // tell the game controller to toggle gameOverPanel
            gameController.ToggleGameOverPanel();
            gameController.UpdateHighScore();
            gameController.SaveGame();
        }
        
    }
}
