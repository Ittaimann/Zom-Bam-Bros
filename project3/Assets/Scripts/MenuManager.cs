﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public GameObject pauseMenu;
    public GameObject controlsMenu;

    private bool isPaused;


	// Use this for initialization
	void Start () {
        isPaused = false;
        // UnloadAll();
        // Having this off should allow this to be use for all scenes
        // Must ensure all menus active = false on load
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Pause") && !isPaused)
        {
            Pause();
        } else
        if (Input.GetButtonDown("Pause") && isPaused)
        {
            UnPause();
        }
	}


    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        string scene = SceneManager.GetActiveScene().name;
        LoadLevel(scene);
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        LoadMenu(pauseMenu);
    }

    public void UnPause()
    {
        isPaused = false;
        Time.timeScale = 1;
        UnloadAll();
    }

    public void UnloadMenu(GameObject menu)
    {
        menu.SetActive(false);
    }

    public void LoadMenu(GameObject menu)
    {
        menu.SetActive(true);
    }

    private void UnloadAll()
    {
        UnloadMenu(pauseMenu);
        UnloadMenu(controlsMenu);
    }
}