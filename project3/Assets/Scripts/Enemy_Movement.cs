using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour {

    //[HideInInspector]
    //This will be set in the Game Manager
    public Transform player, player2;
    private Rigidbody2D rb;

    public EnemyScriptable enemyinfo;

    public LayerMask PlayerSearchLayers;

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
        player = enemyinfo.player1;
        player2 = enemyinfo.player2;
        //Debug.LogWarning(player+ " "+ player2);
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Transform to_target = target;

        if(!to_target)
            to_target = (player.position - transform.position).magnitude > (player2.position - transform.position).magnitude ? player2 : player;


        RaycastHit2D rh = Physics2D.Raycast(transform.position, (to_target.transform.position - transform.position).normalized, Mathf.Infinity, PlayerSearchLayers);
        if (rh.collider)
        {
            if(rh.collider.tag == "Player")
            {
                rb.velocity /= 1.05f;

                transform.GetChild(0).rotation = Quaternion.LookRotation(Vector3.forward, to_target.position - transform.position);

                if (rb.velocity.magnitude < maxVelocity)
                    rb.velocity += speed * Time.deltaTime * (Vector2)(to_target.position - transform.position).normalized;

                if ((to_target.position - transform.position).magnitude < cutoffDistance)
                {
                    rb.velocity = Vector2.zero;
                    if (can_hit)
                        StartCoroutine(Hit_Player(to_target.gameObject));
                }
            }
            else
            {
                //print("do A* of some sort maybe");
                rb.velocity = Vector2.zero;
            }

        }
        else
        {
            print("y u no see anything");
        }


    }

    IEnumerator Hit_Player(GameObject player)
    {
        //print("Direction thing: " + (player.transform.position - transform.position).normalized * hitStrength);
        can_hit = false;
        player.GetComponent<Player>().Take_Damage(damage);
        //Hits the player back on contact (doesn't really work but seems like a cool idea)
        //player.GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position).normalized * hitStrength);
        yield return new WaitForSeconds(0.5f);
        can_hit = true;
    }
}
