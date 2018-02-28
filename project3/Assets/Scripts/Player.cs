using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public int playerNum;
	private Rigidbody2D rb2d;
	


	//TEMP
	public GameObject bullet;


	public PlayerScriptable playerInfo;


    private float health;
	private float speed;

    private bool can_take_damage = true;

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
        yield return new WaitForSeconds(0);
        if(Input.GetAxisRaw("shoot"+playerNum)!=-1)
		{
			Instantiate(bullet,transform.position,angle);
		}
	}

    public void Take_Damage(float dam)
    {
        print("got hit");
        if (!can_take_damage)
            return;

        StartCoroutine(Invulnerability_Frames());

        health -= dam;

        //Play sounds for damage
    }

    IEnumerator Invulnerability_Frames()
    {
        can_take_damage = false;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        for(int i = 0; i < 5; ++i)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.2f);
            yield return new WaitForSeconds(0.1f);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
            yield return new WaitForSeconds(0.1f);
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
        can_take_damage = true;
    }
}
