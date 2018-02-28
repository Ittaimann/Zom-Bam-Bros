using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour {

    //[HideInInspector]
    //This will be set in the Game Manager
    public Transform player;
    private Rigidbody2D rb;

    [Header("Variables")]
    public float speed;
    public float maxVelocity;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        rb.velocity /= 1.05f;

        if (rb.velocity.magnitude < maxVelocity)
            rb.velocity += speed * Time.deltaTime * (Vector2) (player.transform.position - transform.position).normalized;

        //if (player.transform.position.x > transform.position.x && rb.velocity.magnitude < maxVelocity)
        //    rb.velocity += speed * Time.deltaTime * Vector2.right;
        //else if (player.transform.position.x < transform.position.x && rb.velocity.magnitude < maxVelocity)
        //    rb.velocity -= speed * Time.deltaTime * Vector2.right;

        //if (player.transform.position.y > transform.position.y && rb.velocity.magnitude < maxVelocity)
        //    rb.velocity += speed * Time.deltaTime * Vector2.up;
        //else if (player.transform.position.y < transform.position.y && rb.velocity.magnitude < maxVelocity)
        //    rb.velocity -= speed * Time.deltaTime * Vector2.up;
	}
}
