using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	private bool fighting;
	public float SpawnTime;
	public GameObject enemy;

	public EnemyScriptable enemies;
	public PlayerScriptable player1;
	public PlayerScriptable player2;

	void Awake()
	{
		fighting = false;
		enemies.player1= player1.loc;
        enemies.player2 = player2.loc;
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
