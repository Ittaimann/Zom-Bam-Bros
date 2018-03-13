using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class dumbsprites
{
	public Sprite T1;
	public Sprite T2;


	public Sprite getT1()
	{return T1;}


    public Sprite getT2()
    { return T2; }
}



[CreateAssetMenu(fileName = "Enemy", menuName = "enemy")]
public class EnemyScriptable : ScriptableObject {
	public int health;
	public Transform player1;
	public Transform player2;
	public int enemyNumber;
	public List<Material> palletes;
	public List <dumbsprites> states;

	public void DecEnemyNumber()
	{
		if(enemyNumber!=0)
		{
			enemyNumber-=1;
		}

	}
	
	// Use this for initialization


}
