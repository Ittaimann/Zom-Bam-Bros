using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour {

    //[HideInInspector]
    //This will be set in the Game Manager
    public Transform player, player2;
    private Rigidbody2D rb;

    [HideInInspector]
    public Transform target;

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
        Transform to_target = target;
        if(!to_target)
            to_target = (player.position - transform.position).magnitude > (player2.position - transform.position).magnitude ? player2 : player;
        rb.velocity /= 1.05f;

        if (rb.velocity.magnitude < maxVelocity)
            rb.velocity += speed * Time.deltaTime * (Vector2) (to_target.position - transform.position).normalized;

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
