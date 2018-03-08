using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	private bool fighting;
	public float SpawnTime;

    [Header("Enemies")]
	public GameObject enemy;
    public int num_enemies;

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

    [Header("Pathfinding")]
    public Vector2[] positionsOfObstacles;
    public int xSize, ySize;

	// Use this for initialization
	void Start () {
        fighting = false;
        enemies.player1 = player1.loc;
        enemies.player2 = player2.loc;
        player1.isalive = true;
        player2.isalive = true;
        player1.fighting = false;
        player2.fighting = false;
        enemies.enemyNumber = num_enemies;
        CreateGrid(xSize, ySize);
		StartCoroutine("enemySpawn");

	}


    private void CreateGrid(int sizex, int sizey)
    {
        enemies.grid = new Vector3[sizey, sizex];
        for (int y = 0; y < sizey; ++y)
        {
            for (int x = 0; x < sizex; ++x)
            {
                enemies.grid[y, x] = new Vector3(x, y, 0);
            }
        }
    }

	
	// Update is called once per frame
	void Update () {
        if (!player1.isalive && !player1_death_panel.activeSelf)
            player1_death_panel.SetActive(true);
        if (!player2.isalive && !player2_death_panel.activeSelf)
            player2_death_panel.SetActive(true);
		
		// Might want to break this into a seperate function
	
	}

	IEnumerator enemySpawn()
	{
        int count = 0;
		while(enemies.enemyNumber > 0)
		{
            yield return new WaitForSeconds(Random.Range(SpawnTime - 0.5f, SpawnTime + 0.5f));
			Transform spawn=(transform.GetChild(Random.Range(0,4)));

            if(count != num_enemies)
            {
                count++;
                GameObject e = Instantiate(enemy, spawn);
                e.GetComponent<Enemy_Movement>().speed += Random.Range(-10f, 30f);
                int rand = Random.Range(0, 100);

                if(rand > 40 && rand < 60)
                    e.GetComponent<Enemy_Health>().drop = healthDrop;
                else if(rand <= 60 && rand < 75)
                    e.GetComponent<Enemy_Health>().drop = speedDrop;
                else if(rand <= 75 && rand < 85)
                    e.GetComponent<Enemy_Health>().drop = shootspeedDrop;
            }

        }
        player1.fighting = true;
        player2.fighting = true;
	}
}
