using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarTesting : MonoBehaviour {

    public EnemyScriptable es;
    private Vector3[,] grid;
    public Transform Player;

    //Use vector3's z value as the f value (cause its an easy typedef)

    void Update()
    {

        grid = new Vector3[es.grid.GetLength(0), es.grid.GetLength(1)];
        grid = es.grid;
        Get_Path();

    }

    private void Get_Path()
    {

    }
}
