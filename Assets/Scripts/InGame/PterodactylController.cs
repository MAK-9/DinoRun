using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PterodactylController : MonoBehaviour
{
    private PlayerController playerController;
    private Rigidbody2D rb;

    private void Awake()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerController.Die(true);
            Die(other);
        }
    }

    void Die(Collider2D other)
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        if (other.transform.position.y > this.transform.position.y)
        {
            rb.velocity = new Vector2(10f, -15f);
        }
        else
        {
            rb.velocity = new Vector2(10f, 15f);
        }
        StartCoroutine(Destroy(1f));
    }

    IEnumerator Destroy(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
