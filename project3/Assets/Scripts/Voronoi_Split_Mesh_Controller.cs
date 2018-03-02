using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Voronoi_Split_Mesh_Controller : MonoBehaviour {


    public GameObject LeftPivot, RightPivot, MiddleBar;
    public Camera_Follow LeftCam, RightCam;
    public GameObject Focus_Left, Focus_Right;

    public GameObject player1, player2;

    private Quaternion rot;

    public float left_offset, right_offset;

    public float approach_speed, cutoff;
    public float x_min, x_max, y_min, y_max;


    // Use this for initialization
    void Start () {
        rot = Quaternion.identity;
        LeftCam.offset_percent = 1 - (462.5f + Focus_Left.transform.localPosition.x) / 462.5f;
        RightCam.offset_percent = 1 - (462.5f - Focus_Right.transform.localPosition.x) / 462.5f;
        //rm.rectTransform.
    }

    // Update is called once per frame
    void Update () {

        if ((player2.transform.position - player1.transform.position).magnitude < 35)
        {
            //Set split screen off
            if (RightPivot.activeSelf)
            {
                player1.GetComponent<Player>().Set_Camera(gameObject);
                player2.GetComponent<Player>().Set_Camera(gameObject);
                RightPivot.SetActive(false);
                LeftPivot.SetActive(false);
                MiddleBar.SetActive(false);
            }
        }
        else
        {
            //Set split screen on
            if (!RightPivot.activeSelf)
            {
                player1.GetComponent<Player>().Set_Camera(LeftCam.gameObject);
                player2.GetComponent<Player>().Set_Camera(RightCam.gameObject);
                RightPivot.SetActive(true);
                LeftPivot.SetActive(true);
                MiddleBar.SetActive(true);
            }
        }


        //Controller for when they are together
        Vector3 point_between = (player1.transform.position + player2.transform.position) / 2;

        Vector3 diff = new Vector3(-(transform.position.x - point_between.x), -(transform.position.y - point_between.y), 0);

        //Checks to make sure screen is in bounds
        if (transform.position.x + diff.x >= x_max || transform.position.x + diff.x <= x_min)
            diff = new Vector3(0, diff.y, 0);
        if (transform.position.y + diff.y >= y_max || transform.position.y + diff.y <= y_min)
            diff = new Vector3(diff.x, 0, 0);

        //Checks to see if the camera is already withing cutoff range or if it doesn't need to move
        if (Mathf.Abs(transform.position.magnitude - point_between.magnitude) > cutoff && diff != Vector3.zero)
            transform.position += diff * approach_speed * Time.deltaTime;

        //controller for when they are not
        Vector3 line_between = Vector3.Cross(player2.transform.position - player1.transform.position, Vector3.forward);


        float angle = Vector2.Angle(line_between, Vector2.down) * (player2.transform.position.y > player1.transform.position.y ? 1 : -1);


        if (rot.eulerAngles.z != angle)
        {
            //Rotates everything
            rot.eulerAngles = new Vector3(0, 0, angle);
            LeftPivot.transform.rotation = rot;
            RightPivot.transform.rotation = rot;
            MiddleBar.transform.rotation = rot;
            LeftCam.transform.rotation = rot;
            RightCam.transform.rotation = rot;

            //Shifts the focus so the player is centered more in each side camera
            if ((rot.eulerAngles.z > 0 && rot.eulerAngles.z < 90))
            {
                Focus_Left.transform.localPosition = new Vector2(Mathf.Lerp(-125, -200, (90 - rot.eulerAngles.z) / 90f), 0);
                Focus_Right.transform.localPosition = new Vector2(Mathf.Lerp(125, 200, (90 - rot.eulerAngles.z) / 90f), 0);
            }
            else if (rot.eulerAngles.z > 90 && rot.eulerAngles.z < 180)
            {
                Focus_Left.transform.localPosition = new Vector2(Mathf.Lerp(-200, -125, (90 - (rot.eulerAngles.z - 90)) / 90f), 0);
                Focus_Right.transform.localPosition = new Vector2(Mathf.Lerp(200, 125, (90 - (rot.eulerAngles.z - 90)) / 90f), 0);
            }
            else if (rot.eulerAngles.z > 180 && rot.eulerAngles.z < 270)
            {
                Focus_Left.transform.localPosition = new Vector2(Mathf.Lerp(-125, -200, (90 - (rot.eulerAngles.z - 180)) / 90f), 0);
                Focus_Right.transform.localPosition = new Vector2(Mathf.Lerp(125, 200, (90 - (rot.eulerAngles.z - 180)) / 90f), 0);
            }
            else if (rot.eulerAngles.z > 270 && rot.eulerAngles.z < 360)
            {
                Focus_Left.transform.localPosition = new Vector2(Mathf.Lerp(-200, -125, (90 - (rot.eulerAngles.z - 270)) / 90f), 0);
                Focus_Right.transform.localPosition = new Vector2(Mathf.Lerp(200, 125, (90 - (rot.eulerAngles.z - 270)) / 90f), 0);
            }

            //Sets that focus offset to both cameras
            LeftCam.offset_percent = 1 - (462.5f + Focus_Left.transform.localPosition.x) / 462.5f;
            RightCam.offset_percent = 1 - (462.5f - Focus_Right.transform.localPosition.x) / 462.5f;
        }



        
	}
}
