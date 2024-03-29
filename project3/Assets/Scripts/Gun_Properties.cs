﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Properties : MonoBehaviour {
    public string gunName;
    public int bullets;
    public float spreadDegrees;
    public float bulletSpeed;
    public float reload;
    public float damage;
    public AudioClip shootSound;
    public int piercing;

    public Vector2 laserLocation;
}
