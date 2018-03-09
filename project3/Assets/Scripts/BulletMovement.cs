using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour {

	public float speed;
    public float damage;
    public int piercing = 0;

	private Rigidbody2D rb2d;

	public PoolApi pool;

    public GameObject bulletExplosion;


	void Awake()
	{
        rb2d = GetComponent<Rigidbody2D>();
	}
	

	void OnEnable()
	{
	   Invoke("ReturnToPool",2);
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
        Instantiate(bulletExplosion, transform.position, Quaternion.identity);
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
