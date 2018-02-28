using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BulletpoolAPI", menuName = "pool")]

public class PoolApi : ScriptableObject {

	private GameObject pool;


	public void SetPool(GameObject requestToPool)
	{
		if(pool==null)
		{
			pool=requestToPool;
		}
	}

	public GameObject RequestBullet(string tag)
	{
		if(pool.transform.childCount!=0)
		{
			var bullet = pool.transform.GetChild(0);
            bullet.parent = null;
            bullet.gameObject.SetActive(true);
			bullet.tag=tag;
            return bullet.gameObject;
		}
		return null;
		
	}
	
	public void returnBullet(GameObject ReturningBullet)
	{
		ReturningBullet.transform.parent=pool.transform;
	}

}
