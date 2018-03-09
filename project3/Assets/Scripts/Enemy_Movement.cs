﻿using System.Collections;
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
    private Transform to_target;

    [Header("Variables")]
    public float speed;
    public float maxVelocity;
    public float cutoffDistance;
    public float hitStrength;
    public float damage;

    private bool can_hit = true;

    [Header("Sounds")]
    public AudioClip[] randomSounds;
    public AudioClip[] attackSounds;
    private AudioSource audioPlayer;
    public float maxHearableDistance;

	// Use this for initialization
	void Start () {
        player = enemyinfo.player1;
        player2 = enemyinfo.player2;
        //Debug.LogWarning(player+ " "+ player2);
        rb = GetComponent<Rigidbody2D>();
        audioPlayer = GetComponent<AudioSource>();
        StartCoroutine(PlayRandomSounds());
	}
	
	// Update is called once per frame
	void Update ()
    {
        to_target = (player.position - transform.position).magnitude > (player2.position - transform.position).magnitude ? player2 : player;


        //RaycastHit2D rh = Physics2D.Raycast(transform.position, (to_target.transform.position - transform.position).normalized, Mathf.Infinity, PlayerSearchLayers);
        //if (rh.collider)
        //{
            //if(rh.collider.tag == "Player")
            //{
        rb.velocity /= 1.05f;

        transform.GetChild(0).rotation = Quaternion.LookRotation(Vector3.forward, to_target.position - transform.position);

        if (rb.velocity.magnitude < maxVelocity)
            rb.velocity += speed * Time.deltaTime * (Vector2)(to_target.position - transform.position).normalized;

            //}
            //else
            //{
            //    //print("do A* of some sort maybe");
            //    rb.velocity = Vector2.zero;
            //}

        //}
        //else
        //{
        //    print("y u no see anything");
        //}


    }

    void OnTriggerStay2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;
            if (can_hit)
                StartCoroutine(Hit_Player(c.gameObject));

        }
    }

    IEnumerator Hit_Player(GameObject player)
    {
        //print("Direction thing: " + (player.transform.position - transform.position).normalized * hitStrength);
        can_hit = false;
        audioPlayer.volume = 1;
        audioPlayer.pitch = Random.Range(0.9f, 1.1f);
        audioPlayer.clip = attackSounds[Random.Range(0, attackSounds.Length)];
        audioPlayer.Play();
        player.GetComponent<Player>().Take_Damage(damage);
        //Hits the player back on contact (doesn't really work but seems like a cool idea)
        //player.GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position).normalized * hitStrength);
        yield return new WaitForSeconds(0.5f);
        can_hit = true;
    }

    IEnumerator PlayRandomSounds()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 7f));

            audioPlayer.volume = ((maxHearableDistance - (transform.position - to_target.position).magnitude) / maxHearableDistance);

            audioPlayer.pitch = Random.Range(0.9f, 1.1f);
            audioPlayer.clip = randomSounds[Random.Range(0, randomSounds.Length)];
            audioPlayer.Play();
        }
    }
}
