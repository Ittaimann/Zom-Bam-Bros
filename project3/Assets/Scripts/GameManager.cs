using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	private bool fighting;
	public float SpawnTime;
	public GameObject enemy;

	void Awake()
	{
		fighting = false;
	}
	// Use this for initialization
	void Start () {
		StartCoroutine("enemySpawn");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator enemySpawn()
	{
		while(!fighting)
		{
            yield return new WaitForSeconds(SpawnTime);
			Transform spawn=(transform.GetChild(Random.Range(0,4)));

			Instantiate(enemy,spawn);


        }
	}
}
