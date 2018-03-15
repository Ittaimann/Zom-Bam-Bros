using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour {

    public GameObject startMenu;
    public GameObject controlsMenu;

    public UnityEngine.EventSystems.EventSystem startSystem;
    public UnityEngine.EventSystems.EventSystem controlsSystem;

    // Use this for initialization
    void Start () {
        UnloadSystem(controlsSystem);
        LoadSystem(startSystem);

        UnloadMenu(controlsMenu);
        LoadMenu(startMenu);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void UnloadMenu(GameObject menu)
    {
        menu.SetActive(false);
    }

    public void LoadMenu(GameObject menu)
    {
        menu.SetActive(true);
    }


    public void LoadSystem(UnityEngine.EventSystems.EventSystem system)
    {
        system.gameObject.SetActive(true);
    }

    public void UnloadSystem(UnityEngine.EventSystems.EventSystem system)
    {
        system.gameObject.SetActive(false);
    }

    public void StartToControls()
    {
        UnloadSystem(startSystem);
        UnloadMenu(startMenu);

        LoadSystem(controlsSystem);
        LoadMenu(controlsMenu);
    }

    public void ControlsToStart()
    {
        UnloadSystem(controlsSystem);
        UnloadMenu(controlsMenu);

        LoadSystem(startSystem);
        LoadMenu(startMenu);
    }
}
