using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour {

	public float speed;
    public float damage;

	private Rigidbody2D rb2d;

	public PoolApi pool;


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
        rb2d.MovePosition(new Vector2(transform.right.x, transform.right.y) * Time.deltaTime*speed + rb2d.position);
	}
	
	public void ReturnToPool()
	{
        CancelInvoke();
		gameObject.SetActive(false);
		pool.returnBullet(gameObject);
	}

    public float GetDamage()
    {
        return damage;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Walls"))
            ReturnToPool();
    }

}
