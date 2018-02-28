using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour {

    public EnemyScriptable es;

    private float health;

	// Use this for initialization
	void Start () {
        health = es.health;
	}
	
	void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "PlayerBullet1" || c.tag == "PlayerBullet2")
            Take_Damage(c.GetComponent<BulletMovement>().GetDamage());
    }

    public void Take_Damage(float dam)
    {
        health -= dam;
        if (health <= 0)
        {
            print("dead");
            Destroy(gameObject);
        }
    }
}
