using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public int playerNum;
	private Rigidbody2D rb2d;
	
	public PlayerScriptable playerInfo;


    private float health;
	private float speed;

	void Awake()
	{
		rb2d = GetComponent<Rigidbody2D>();
		health=playerInfo.StartHealth;
		speed=playerInfo.StartSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		PlayerMove();
		aim();
	}


	void aim()
	{
		var child = transform.GetChild(1);
		Vector2 direction=new Vector2(Input.GetAxis("RstickVertical" + playerNum), Input.GetAxis("RstickHorizontal" + playerNum));
		direction.Normalize();
		if(direction != Vector2.zero)
			child.eulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.x,direction.y) * 180 / Mathf.PI); 
	}

	void PlayerMove()
	{
		Debug.Log("Horizontal" + playerNum);
		Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"+playerNum), Input.GetAxisRaw("Vertical"+ playerNum));
        movement.Normalize();
        rb2d.MovePosition(movement * Time.deltaTime * speed + rb2d.position);
	}
}
