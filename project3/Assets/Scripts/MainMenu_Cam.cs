﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Cam : MonoBehaviour {

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(25 + (45 * Mathf.Cos(Time.time/3)), 0, -10);
	}
}
