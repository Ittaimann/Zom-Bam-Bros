using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poolScript : MonoBehaviour {
	public int BulletCount;
	public GameObject bullet;
	public PoolApi pool;

	
	// Use this for initialization
	void Start () 
	{
		pool.SetPool(gameObject);
		for(int i=0; i<BulletCount; i++)
		{
			var child= Instantiate(bullet,transform.position,transform.rotation,transform);
			child.SetActive(false);
            child.GetComponent<BulletMovement>().CancelInvoke();
		}		
	}
	
	// Update is called once per frame
	
}
