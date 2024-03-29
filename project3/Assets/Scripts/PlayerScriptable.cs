﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Player", menuName = "Player data")]

public class PlayerScriptable : ScriptableObject{
	public float StartHealth;
	public float StartSpeed;
    public int playerNum;
	public Transform loc;
    public bool isalive;
	public bool fighting;

    public AudioClip healthSound;
    public AudioClip shootspeedSound;
    public AudioClip speedSound;
    public AudioClip equipSound;
	


}
