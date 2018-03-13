using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour {

    //[HideInInspector]
    //This will be set in the Game Manager
    public Transform player, player2;
    private Rigidbody2D rb;

    public EnemyScriptable enemyinfo;

    [Header("Pathfinding")]
    public LayerMask PlayerSearchLayers;
    public Grid_Creator gc;
    private List<Node> path_to_player;
    private bool resetPath = false;
    private bool settingPath = false;
    private Vector2 lastKnown = new Vector2(-100, 100);

    [HideInInspector]
    public Transform target;
    private Transform to_target;

    [Header("Variables")]
    public float speed;
    public float maxVelocity;
    public float cutoffDistance;
    public float hitStrength;
    public float damage;

    private bool can_hit = true;

    [Header("Sounds")]
    public AudioClip[] randomSounds;
    public AudioClip[] attackSounds;
    private AudioSource audioPlayer;
    public float maxHearableDistance;

	// Use this for initialization
	void Start () {
        path_to_player = new List<Node>();
        player = enemyinfo.player1;
        player2 = enemyinfo.player2;
        //Debug.LogWarning(player+ " "+ player2);
        rb = GetComponent<Rigidbody2D>();
        audioPlayer = GetComponent<AudioSource>();
        StartCoroutine(PlayRandomSounds());
	}
	
	// Update is called once per frame
	void Update ()
    {
        to_target = (player.position - transform.position).magnitude > (player2.position - transform.position).magnitude ? player2 : player;


        RaycastHit2D rh = Physics2D.Raycast(transform.position, (to_target.transform.position - transform.position).normalized, Mathf.Infinity, PlayerSearchLayers);
        if (rh.collider)
        {
            if (rh.collider.tag == "Player" || ((Vector2)to_target.position - lastKnown).magnitude < 5)
            {
                print("saw player " + transform.position.x + " " + transform.position.y);
                lastKnown = rh.collider.gameObject.transform.position;
                resetPath = true;
                settingPath = false;
                //If can see player walk right towards them
                rb.velocity /= 1.05f;

                transform.GetChild(0).rotation = Quaternion.LookRotation(Vector3.forward, to_target.position - transform.position);

                if (rb.velocity.magnitude < maxVelocity)
                    rb.velocity += speed * Time.deltaTime * (Vector2)(to_target.position - transform.position).normalized;

            }
            else
            {
                //If can't see player then A* towards them

                //This is to make it reset the path if it started following the player but may be too many calls
                if ((path_to_player.Count == 0 || resetPath) && !settingPath)
                {
                    resetPath = false;
                    path_to_player.Clear();
                    path_to_player = gc.Get_Path(transform, to_target);
                    print("getting path");
                    settingPath = true;
                }
                else if (path_to_player.Count > 0)
                {
                    settingPath = false;
                    if (((Vector2)transform.position - path_to_player[path_to_player.Count - 1].position()).magnitude <= 0.25f)
                    {
                        //If he got to the Node then go to next one
                        path_to_player.RemoveAt(path_to_player.Count - 1);
                    }

                    Vector2 nodePos = path_to_player[path_to_player.Count - 1].position();

                    //Travel to the first Node on the list
                    rb.velocity /= 1.05f;
                    transform.GetChild(0).rotation = Quaternion.LookRotation(Vector3.forward, (Vector3)nodePos - transform.position);

                    if (rb.velocity.magnitude < maxVelocity)
                        rb.velocity += speed * Time.deltaTime * (nodePos - (Vector2)transform.position).normalized;
                }
            }

        }
    }

    void OnTriggerStay2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero;
            if (can_hit)
                StartCoroutine(Hit_Player(c.gameObject));

        }
    }

    IEnumerator Hit_Player(GameObject player)
    {
        //print("Direction thing: " + (player.transform.position - transform.position).normalized * hitStrength);
        can_hit = false;
        audioPlayer.volume = 1;
        audioPlayer.pitch = Random.Range(0.9f, 1.1f);
        audioPlayer.clip = attackSounds[Random.Range(0, attackSounds.Length)];
        audioPlayer.Play();
        player.GetComponent<Player>().Take_Damage(damage);
        //Hits the player back on contact (doesn't really work but seems like a cool idea)
        //player.GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position).normalized * hitStrength);
        yield return new WaitForSeconds(0.5f);
        can_hit = true;
    }

    IEnumerator PlayRandomSounds()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 7f));

            audioPlayer.volume = ((maxHearableDistance - (transform.position - to_target.position).magnitude) / maxHearableDistance);

            audioPlayer.pitch = Random.Range(0.9f, 1.1f);
            audioPlayer.clip = randomSounds[Random.Range(0, randomSounds.Length)];
            audioPlayer.Play();
        }
    }
}
