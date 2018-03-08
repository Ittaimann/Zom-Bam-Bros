using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "enemy")]
public class EnemyScriptable : ScriptableObject {
	public int health;
	public Transform player1;
	public Transform player2;
	public int enemyNumber;
    public Vector3[,] grid;

	public void DecEnemyNumber()
	{
		if(enemyNumber!=0)
		{
			enemyNumber-=1;
		}

	}
	
	// Use this for initialization


}
