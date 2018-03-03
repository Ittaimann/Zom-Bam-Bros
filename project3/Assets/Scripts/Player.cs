using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // private variables 
	private Rigidbody2D rb2d;
    private float health;
    private float speed;
    private bool can_take_damage = true;
    private bool shooting = false;
    //should get this from the gun
    private float shoot_wait = 1;

    // public scriptables
	public PoolApi pool;
	public PlayerScriptable playerInfo;

    //Max variables
    [SerializeField]
    private float speed_max, health_max, shoot_speed_max;


    //follow cam
    private GameObject following_camera;

  

	void Awake()
	{
        playerInfo.loc=transform;
		health=playerInfo.StartHealth;
		speed=playerInfo.StartSpeed;

        rb2d = GetComponent<Rigidbody2D>();

    }
	

	void Update () 
    {
        if( playerInfo.isalive )
        {
            PlayerMove();
            aim(); // SHOOT IS CALLED HERE
        }
    }


    public void Set_Camera( GameObject c )
    {
        following_camera = c;
    }


	void aim()
	{
		var child = transform.GetChild(1);

		Vector2 direction=new Vector2( Input.GetAxis( "RstickVertical" + playerInfo.playerNum ),
                                     Input.GetAxis( "RstickHorizontal" + playerInfo.playerNum ) );
		direction.Normalize();
		
        if(direction != Vector2.zero)
			child.eulerAngles = new Vector3( 0, 0, Mathf.Atan2(direction.x,direction.y) * Mathf.Rad2Deg);
        
        if(!shooting && Input.GetAxisRaw("shoot" + playerInfo.playerNum) >= .5)
	    	StartCoroutine(shoot(child.rotation));
	}

	void PlayerMove()
	{
		Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"+playerInfo.playerNum), 
                                        Input.GetAxisRaw("Vertical"+ playerInfo.playerNum));
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

        yield return new WaitForSeconds(shoot_wait);

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
        {
            playerInfo.isalive = false;
            gameObject.SetActive(false);
        }

        //Play sounds for damage
    }


  
    void OnTriggerEnter2D(Collider2D other)
    {
        print(other.name);

        if(other.tag == "SpeedUp")
        {
            Destroy(other.gameObject);
            if(speed < speed_max)
                speed += 5;
        }
        else if(other.tag == "HealthUp")
        {
            Destroy(other.gameObject);
            if (health < health_max)
                health += 1;
        }
        else if(other.tag == "ShootSpeedUp")
        {
            Destroy(other.gameObject);
            if (shoot_wait > shoot_speed_max)
                shoot_wait -= 0.1f;
        }
        else if(other.tag == "Pistol")
        {
            //Press button to pick up 
        }
        else if (playerInfo.fighting && other.tag != "PlayerBullet" + playerInfo.playerNum)
        {
            float damage = other.GetComponent<BulletMovement>().damage;
            Take_Damage(damage);
        }


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
