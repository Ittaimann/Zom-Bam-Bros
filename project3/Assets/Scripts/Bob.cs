using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour {

    private GameObject sprite;

	// Use this for initialization
	void Start () {
        sprite = transform.GetChild(0).gameObject;
        StartCoroutine(UpandDown());
	}

    private IEnumerator UpandDown()
    {
        float timer = 0;
        while(true)
        {
            timer += Time.deltaTime * 3;
            sprite.transform.localPosition = new Vector3(0, Mathf.Cos(timer)/ 2, 0);
            yield return new WaitForSeconds(0.01f);
            //while(sprite.transform.localPosition.y < 1)
            //{
            //    sprite.transform.Translate(0, 0.02f, 0);
            //    yield return new WaitForSeconds(0.01f);
            //}
            //while(sprite.transform.localPosition.y > 0)
            //{
            //    sprite.transform.Translate(0, -0.02f, 0);
            //    yield return new WaitForSeconds(0.01f);
            //}
        }
    }
	
	
}
