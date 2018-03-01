using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private Rigidbody2D rb2d;
	


	public PoolApi pool;


	public PlayerScriptable playerInfo;


    private float health;
	private float speed;

    private GameObject following_camera;

    private bool can_take_damage = true;
	private bool shooting=false;

	void Awake()
	{
        playerInfo.loc=transform;
		rb2d = GetComponent<Rigidbody2D>();
		health=playerInfo.StartHealth;
		speed=playerInfo.StartSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		PlayerMove();
		aim(); // SHOOT IS CALLED HERE
    }

    public void Set_Camera(GameObject c)
    {
        following_camera = c;
    }


	void aim()
	{
		var child = transform.GetChild(1);
		Vector2 direction=new Vector2(Input.GetAxis("RstickVertical" + playerInfo.playerNum), Input.GetAxis("RstickHorizontal" + playerInfo.playerNum));
		direction.Normalize();
		if(direction != Vector2.zero)
			child.eulerAngles = new Vector3( 0, 0, Mathf.Atan2(direction.x,direction.y) * Mathf.Rad2Deg);
        if(!shooting && Input.GetAxisRaw("shoot" + playerInfo.playerNum) >= .5)
	    	StartCoroutine(shoot(child.rotation));
	}

	void PlayerMove()
	{
		Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"+playerInfo.playerNum), Input.GetAxisRaw("Vertical"+ playerInfo.playerNum));
        movement.Normalize();
        rb2d.MovePosition(movement * Time.deltaTime * speed + rb2d.position);
	}

	IEnumerator shoot(Quaternion angle)
	{

        shooting=true;


        
        StartCoroutine(ScreenShake(1));
        transform.GetChild(2).rotation = angle;
        transform.GetChild(2).GetComponent<ParticleSystem>().Emit(40);
        var bullet = pool.RequestBullet("PlayerBullet" + playerInfo.playerNum);
        if(bullet!=null)
        {
		    bullet.transform.position=transform.position;
		    bullet.transform.rotation=angle;
        }
        yield return new WaitForSeconds(1);

        shooting = false;
	}

    public void Take_Damage(float dam)
    {
        StartCoroutine(ScreenShake(5));
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

    IEnumerator ScreenShake(float mult)
    {
        for (int i = 0; i < 5; ++i)
        {
            following_camera.transform.position += new Vector3(Random.Range(-.1f * mult, 0.1f * mult), Random.Range(-.1f * mult, .1f * mult));
            yield return new WaitForSeconds(0.05f);
        }
    }
}
