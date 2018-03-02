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
            Take_Damage(c.GetComponent<BulletMovement>().GetDamage());
            c.GetComponent<BulletMovement>().ReturnToPool();
        }
    }

    public void Take_Damage(float dam)
    {
        health -= dam;
        if (health <= 0)
        {
            print("dead");
            
            if(drop!= null)
                Instantiate( drop, transform.position, transform.rotation );
            
            Destroy(gameObject);
        }
    }
}
