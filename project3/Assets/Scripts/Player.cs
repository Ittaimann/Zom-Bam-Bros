﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public int playerNum;
	private Rigidbody2D rb2d;
	


	public PoolApi pool;


	public PlayerScriptable playerInfo;


    private float health;
	private float speed;

    private bool can_take_damage = true;
	private bool shooting=false;

	void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		health=playerInfo.StartHealth;
		speed=playerInfo.StartSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		PlayerMove();
		aim(); // SHOOT IS CALLED HERE

    }


	void aim()
	{
		var child = transform.GetChild(1);
		Vector2 direction=new Vector2(Input.GetAxis("RstickVertical" + playerNum), Input.GetAxis("RstickHorizontal" + playerNum));
		direction.Normalize();
		if(direction != Vector2.zero)
			child.eulerAngles = new Vector3( 0, 0, Mathf.Atan2(direction.x,direction.y) * Mathf.Rad2Deg);
        if(!shooting)
	    	StartCoroutine(shoot(child.rotation));
	}

	void PlayerMove()
	{
		Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"+playerNum), Input.GetAxisRaw("Vertical"+ playerNum));
        movement.Normalize();
        rb2d.MovePosition(movement * Time.deltaTime * speed + rb2d.position);
	}

	IEnumerator shoot(Quaternion angle)
	{

        shooting=true;

        if(Input.GetAxisRaw("shoot"+playerNum)>=0)
		{
			
            var bullet = pool.RequestBullet("PlayerBullet" + playerNum);
            if(bullet!=null)
            {
		    	bullet.transform.position=transform.position;
		    	bullet.transform.rotation=angle;
            }
        }
        yield return new WaitForSeconds(0);

        shooting = false;
	}

    public void Take_Damage(float dam)
    {
        print("got hit");
        if (!can_take_damage)
            return;

        StartCoroutine(Invulnerability_Frames());

        health -= dam;

        if (health <= 0)
            print(name + " died");

        //Play sounds for damage
    }

    IEnumerator Invulnerability_Frames()
    {
        can_take_damage = false;
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        for(int i = 0; i < 7; ++i)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.2f);
            yield return new WaitForSeconds(0.05f);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
            yield return new WaitForSeconds(0.05f);
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
        can_take_damage = true;
    }
}
