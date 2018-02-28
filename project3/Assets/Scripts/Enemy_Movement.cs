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
    public float cutoffDistance;
    public float hitStrength;
    public float damage;

    private bool can_hit = true;


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

        if ((to_target.position - transform.position).magnitude < cutoffDistance)
        {
            rb.velocity = Vector2.zero;
            if (can_hit)
                StartCoroutine(Hit_Player(to_target.gameObject));
        }
    }

    IEnumerator Hit_Player(GameObject player)
    {
        print("hit");
        //print("Direction thing: " + (player.transform.position - transform.position).normalized * hitStrength);
        can_hit = false;
        //Hits the player back on contact (doesn't really work but seems like a cool idea)
        //player.GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position).normalized * hitStrength);
        player.GetComponent<Player>().Take_Damage(damage);
        yield return new WaitForSeconds(0.5f);
        can_hit = true;
    }
}
