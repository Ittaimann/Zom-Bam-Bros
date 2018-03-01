﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	private bool fighting;
	public float SpawnTime;
	public GameObject enemy;

	public EnemyScriptable enemies;
	public PlayerScriptable player1;
	public PlayerScriptable player2;

    public GameObject player1_death_panel;
    public GameObject player2_death_panel;

	void Awake()
	{
		fighting = false;
		enemies.player1= player1.loc;
        enemies.player2 = player2.loc;
        player1.isalive = true;
        player2.isalive = true;
	}
	// Use this for initialization
	void Start () {
		StartCoroutine("enemySpawn");

	}
	
	// Update is called once per frame
	void Update () {
        if (!player1.isalive && !player1_death_panel.activeSelf)
            player1_death_panel.SetActive(true);
        if (!player2.isalive && !player2_death_panel.activeSelf)
            player2_death_panel.SetActive(true);
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
