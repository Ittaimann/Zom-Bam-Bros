using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour {

    public GameObject to_follow;
    public float approach_speed, cutoff;
    public float x_min, x_max, y_min, y_max;

    // Update is called once per frame
    void Update()
    {
        Vector3 diff = new Vector3(-(transform.position.x - to_follow.transform.position.x), -(transform.position.y - to_follow.transform.position.y), 0);

        if (transform.position.x + diff.x >= x_max || transform.position.x + diff.x <= x_min)
            diff = new Vector3(0, diff.y, 0);
        if (transform.position.y + diff.y >= y_max || transform.position.y + diff.y <= y_min)
            diff = new Vector3(diff.x, 0, 0);

        if (Mathf.Abs(transform.position.magnitude - to_follow.transform.position.magnitude) > cutoff && diff != Vector3.zero)
            transform.position += diff * approach_speed * Time.deltaTime;
    }
}
