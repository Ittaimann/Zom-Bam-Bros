using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Cam : MonoBehaviour {

    private bool hasControllers = false;
    public GameObject controllerCheck;

    void Start()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        StartCoroutine(Flash());
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(25 + (45 * Mathf.Sin(Time.time/4)), 0, -10);
        int count = 0;
        foreach (string s in Input.GetJoystickNames())
        {
            if (s == "Wireless Controller")
                count++;
        }
        if (count == 2)
            hasControllers = true;
        else
            hasControllers = false;

    }

    IEnumerator Flash()
    {
        while(true)
        {
            if(hasControllers)
            {
                controllerCheck.SetActive(false);
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                controllerCheck.SetActive(true);
                yield return new WaitForSeconds(0.3f);
                controllerCheck.SetActive(false);
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}
