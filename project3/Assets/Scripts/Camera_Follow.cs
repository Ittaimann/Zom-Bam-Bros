using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour {

    public GameObject to_follow;
    public float approach_speed, cutoff;
    public float x_min, x_max, y_min, y_max;
    public float offset_percent, edge_of_cam_bounds;
    public int flipped_Cam;

    void Start()
    {
        to_follow.GetComponent<Player>().Set_Camera(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        //Allows for offsets for screen split and others
        Vector3 new_pos = transform.position + (flipped_Cam * new Vector3((edge_of_cam_bounds - (offset_percent * edge_of_cam_bounds)) * Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.z),
                                                                          (edge_of_cam_bounds - (offset_percent * edge_of_cam_bounds)) * Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.z)));

        //The difference between the current target position and the new one
        Vector3 diff = new Vector3(-(new_pos.x - to_follow.transform.position.x), -(new_pos.y - to_follow.transform.position.y), 0);

        //This is an extra thing that makes it not look laggy when you turn it
        //if (offset_percent != last_offset)
        //{
        //    transform.position += diff;
        //    last_offset = offset_percent;
        //}
        //else
        //{
            //Checks to make sure screen is in bounds
            if (new_pos.x + diff.x >= x_max || new_pos.x + diff.x <= x_min)
                diff = new Vector3(0, diff.y, 0);
            if (new_pos.y + diff.y >= y_max || new_pos.y + diff.y <= y_min)
                diff = new Vector3(diff.x, 0, 0);

            //Checks to see if the camera is already withing cutoff range or if it doesn't need to move
            if (Mathf.Abs(new_pos.magnitude - to_follow.transform.position.magnitude) > cutoff && diff != Vector3.zero)
                transform.position += diff * approach_speed * Time.deltaTime;
        //}
    }
}
