using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Manager : MonoBehaviour {

    public int num_locations;

    [Header("Guns")]
    public GameObject Sniper;
    public GameObject Shotgun;
    public GameObject Rifle;

    private Transform[] locations;

	// Use this for initialization
	void Start () {
        locations = new Transform[num_locations];
		for(int i = 0; i < num_locations; ++i)
        {
            locations[i] = transform.GetChild(i);
        }
        SpawnGuns();
	}

    private void SpawnGuns()
    {


        List<Transform> used_locations = new List<Transform>(locations);



        int cur_location = Random.Range(0, used_locations.Count);

        Instantiate(Sniper, used_locations[cur_location].position, Quaternion.identity);
        used_locations.RemoveAt(cur_location);


        cur_location = Random.Range(0, used_locations.Count);

        Instantiate(Sniper, used_locations[cur_location].position, Quaternion.identity);
        used_locations.RemoveAt(cur_location);


        cur_location = Random.Range(0, used_locations.Count);

        Instantiate(Shotgun, used_locations[cur_location].position, Quaternion.identity);
        used_locations.RemoveAt(cur_location);


        cur_location = Random.Range(0, used_locations.Count);

        Instantiate(Shotgun, used_locations[cur_location].position, Quaternion.identity);
        used_locations.RemoveAt(cur_location);


        cur_location = Random.Range(0, used_locations.Count);

        Instantiate(Rifle, used_locations[cur_location].position, Quaternion.identity);
        used_locations.RemoveAt(cur_location);


        cur_location = Random.Range(0, used_locations.Count);

        Instantiate(Rifle, used_locations[cur_location].position, Quaternion.identity);
        used_locations.RemoveAt(cur_location);


        cur_location = Random.Range(0, used_locations.Count);
    }

}
