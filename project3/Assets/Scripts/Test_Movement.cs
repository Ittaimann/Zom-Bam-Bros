using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Movement : MonoBehaviour {

    public KeyCode[] keys;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(keys[0]))
            transform.Translate(Vector2.right);
        else if (Input.GetKey(keys[1]))
            transform.Translate(-Vector2.right);
        else if (Input.GetKey(keys[2]))
            transform.Translate(Vector2.up);
        else if (Input.GetKey(keys[3]))
            transform.Translate(-Vector2.up);
	}
}
