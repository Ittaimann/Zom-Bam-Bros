using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour {

    public EnemyScriptable es;
    public GameObject drop; // on spawn we'll need to set this to some random
                            // prefabof pick up objects


    private float health;

	// Use this for initialization
	void Start () {
        health = es.health;
	}
	
	void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "PlayerBullet1" || c.tag == "PlayerBullet2")
        {
            BulletMovement bm = c.GetComponent<BulletMovement>();
            Take_Damage(bm.GetDamage());
            if (bm.piercing-- == 0)
                bm.ReturnToPool();
        }
    }

    public void Take_Damage(float dam)
    {
        health -= dam;
        if (health <= 0)
        {
            es.DecEnemyNumber();
            if(drop!= null)
                Instantiate( drop, transform.position, transform.rotation );
           
            Destroy(gameObject);
        }
    }
}
