using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour {

	public float speed;

	private Rigidbody2D rb2d;

	[SerializeField,HideInInspector]
	private PoolApi pool;


	void Awake()
	{
        rb2d = GetComponent<Rigidbody2D>();
	}
	

	void OnEnable()
	{
	   Invoke("ReturnToPool",5);
	}

	void Update()
	{
		move();
	}


	void move()
	{
        rb2d.MovePosition(new Vector2(transform.right.x, transform.right.y) * Time.deltaTime + rb2d.position);
	}
	
	void ReturnToPool()
	{
		gameObject.SetActive(false);
		pool.returnBullet(gameObject);
	}

}
