using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public float SpawnTime;
    public Text zombieCount;


    [Header("Enemies")]
	public GameObject enemy;
    public int num_enemies;
    public Grid_Creator gc;

	public EnemyScriptable enemies;

    [Header("Player")]
	public PlayerScriptable player1;
	public PlayerScriptable player2;

    public GameObject player1_death_panel;
    public GameObject player2_death_panel;

    [Header("Drops")]
    public GameObject healthDrop;
    public GameObject speedDrop;
    public GameObject shootspeedDrop;



	// Use this for initialization
	void Start () {
        enemies.player1 = player1.loc;
        enemies.player2 = player2.loc;
        player1.isalive = true;
        player2.isalive = true;
        player1.fighting = false;
        player2.fighting = false;
        enemies.enemyNumber = num_enemies;
		StartCoroutine("enemySpawn");

	}

	
	// Update is called once per frame
	void Update () {
        zombieCount.text = "Zombies Left: " + enemies.enemyNumber;

        if (!player1.isalive && !player1_death_panel.activeSelf)
            player1_death_panel.SetActive(true);
        if (!player2.isalive && !player2_death_panel.activeSelf)
            player2_death_panel.SetActive(true);
		
		// Might want to break this into a seperate function
	
	}

	IEnumerator enemySpawn()
	{
        float offset = 0.5f;
		while(true)
		{
            yield return new WaitForSeconds(Random.Range(SpawnTime + offset, SpawnTime + offset));
			Transform spawn=(transform.GetChild(Random.Range(0,4)));

            GameObject e = Instantiate(enemy, spawn);
            e.GetComponent<Enemy_Movement>().maxVelocity += Random.Range(-1f, 1f);
            e.GetComponent<Enemy_Movement>().gc = gc;
            int rand = Random.Range(0, 100);

            if(rand > 30 && rand < 35)
                e.GetComponent<Enemy_Health>().drop = healthDrop;
            else if(rand >= 40 && rand < 44)
                e.GetComponent<Enemy_Health>().drop = speedDrop;
            else if(rand >= 50 && rand < 53)
                e.GetComponent<Enemy_Health>().drop = shootspeedDrop;

            if(canFight())
            {
                player1.fighting = true;
                player2.fighting = true;
                offset = 0.2f;
            }
        }
    }

    private bool canFight()
    {
        return enemies.enemyNumber == 0;
    }
}
