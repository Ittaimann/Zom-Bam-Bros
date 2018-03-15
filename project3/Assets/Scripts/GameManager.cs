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
    public float spawnDisableDist;

    [Header("Player")]
	public PlayerScriptable player1;
	public PlayerScriptable player2;

    public GameObject player1_death_panel;
    public GameObject player2_death_panel;

    [Header("Drops")]
    public GameObject healthDrop;
    public GameObject speedDrop;
    public GameObject shootspeedDrop;

    [Header("Music")]
    private AudioSource levelMusic;
    public AudioClip pvpMusic;
    public AudioSource alarm;

    //Menu Manager for game over menu
    private MenuManager menus;



	// Use this for initialization
	void Start () {
        menus = FindObjectOfType<MenuManager>();
        player1_death_panel.SetActive(false);
        player2_death_panel.SetActive(false);
        enemies.player1 = player1.loc;
        enemies.player2 = player2.loc;
        player1.isalive = true;
        player2.isalive = true;
        player1.fighting = false;
        player2.fighting = false;
        enemies.enemyNumber = num_enemies;
        levelMusic = GetComponent<AudioSource>();
		StartCoroutine("enemySpawn");

	}

	
	// Update is called once per frame
	void Update () {
        if (!player1.fighting)
            zombieCount.text = "Zombies Left: " + enemies.enemyNumber;


        if (!player1.isalive && !player1_death_panel.activeSelf && !player2_death_panel.activeSelf)
        {
            player1_death_panel.SetActive(true);
            menus.GameOver();
        }
        if (!player2.isalive && !player2_death_panel.activeSelf && !player1_death_panel.activeSelf)
        {
            player2_death_panel.SetActive(true);
            menus.GameOver();
        }
		
		// Might want to break this into a seperate function
	
	}

	IEnumerator enemySpawn()
	{
		while(true)
		{
            yield return new WaitForSeconds(Random.Range(SpawnTime - SpawnTime/4, SpawnTime + SpawnTime/4));
            Transform spawn=(transform.GetChild(Random.Range(0,transform.childCount)));

            while((player1.loc.position - spawn.position).magnitude < spawnDisableDist || (player2.loc.position - spawn.position).magnitude < spawnDisableDist)
            { 
                spawn = (transform.GetChild(Random.Range(0, transform.childCount)));
            }

            GameObject e = Instantiate(enemy, spawn.position, Quaternion.identity);
            e.GetComponent<Enemy_Movement>().maxVelocity += Random.Range(-1.5f, 1.5f);
            e.GetComponent<Enemy_Movement>().gc = gc;
            int rand = Random.Range(0, 100);

            if(rand > 25 && rand < 35)
                e.GetComponent<Enemy_Health>().drop = healthDrop;
            else if(rand >= 40 && rand < 45)
                e.GetComponent<Enemy_Health>().drop = speedDrop;
            else if(rand >= 50 && rand < 55)
                e.GetComponent<Enemy_Health>().drop = shootspeedDrop;

            if(canFight() && !player1.fighting)
            {
                player1.fighting = true;
                player2.fighting = true;
                zombieCount.text = "FIGHT";
                zombieCount.color = Color.red;
                zombieCount.gameObject.transform.localScale = new Vector3(2, 2, 2);
                StartCoroutine(Music_Fade());
                StartCoroutine(Flash(zombieCount.gameObject));
            }
        }
    }

    private bool canFight()
    {
        return enemies.enemyNumber == 0;
    }

    private IEnumerator Music_Fade()
    {
        for(int i = 0; i < 10; ++i)
        {
            levelMusic.volume -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        alarm.Play();
        yield return new WaitForSeconds(0.8f);
        levelMusic.clip = pvpMusic;
        levelMusic.Play();
        levelMusic.volume = .9f;
    }

    private IEnumerator Flash(GameObject g)
    {
        int count = 0;
        while(count < 5)
        {
            g.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            g.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            count++;
        }
    }
}
