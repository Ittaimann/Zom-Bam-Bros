using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarTesting : MonoBehaviour {

    public Vector3[][] grid;
    public EnemyScriptable es;

    //Use vector3's z value as the f value (cause its an easy typedef)

    void LateStart()
    {
        Get_Path();
    }

    private void Get_Path()
    {
        print(es.grid[0,0]);
    }
}
