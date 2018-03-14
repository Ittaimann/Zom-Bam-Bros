using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour {

    public EnemyScriptable es;
    private Rigidbody2D rb;
    [SerializeField]
    private float knockback;
    public GameObject drop; // on spawn we'll need to set this to some random
                            // prefabof pick up objects
    dumbsprites stateSprites;

    private float health;
    private bool dead = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        health = es.health;
        
        stateSprites=es.states[Random.Range(0,5)];
        Material pallate = es.palletes[Random.Range(0, 5)];
        GetComponentInChildren<SpriteRenderer>().sprite=stateSprites.T1;
        GetComponentInChildren<SpriteRenderer>().material = pallate;


    }
	
	void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "PlayerBullet1" || c.tag == "PlayerBullet2")
        {
            BulletMovement bm = c.GetComponent<BulletMovement>();
            Take_Damage(bm.GetDamage());


            float angle = Vector2.Angle(Vector2.right, transform.position - c.transform.position);
            if (c.transform.position.y > transform.position.y)
            {
                angle = -angle;
            }
            rb.AddForce(new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle) * bm.GetDamage(), Mathf.Sin(Mathf.Deg2Rad * angle) * bm.GetDamage()) * knockback);

            if (bm.piercing-- == 0)
                bm.ReturnToPool();
        }
    }

    public void Take_Damage(float dam)
    {
        health -= dam;

        //damage sprite change        
        if(health<=2)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = stateSprites.T2;
        }
        if (health <= 0 && !dead)
        {
            es.DecEnemyNumber();
            if(drop!= null)
                Instantiate( drop, transform.position, transform.rotation );
            dead = true;
            Destroy(gameObject);
        }
    }
}
