using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyStats : MonoBehaviour
{
	[Header("HP")]
    public float[,] maxhp = { { 100, 60, 300 }, { 200, 160, 400 }, { 300, 260, 500 }, { 400, 360, 600 }, { 500, 460, 700 } };       //kvuli fireball je public

	[Header("Attack")]
	public int[,] dmgMin = { { 20, 30, 15 }, { 40, 60, 30 }, { 60, 90, 45 }, { 80, 120, 60 }, { 100, 150, 75 } };             //sila pro vojacky (soldier, ranger, tank)
    public int[,] dmgMax = { { 40, 60, 30 }, { 80, 120, 60 }, { 120, 180, 90 }, { 160, 240, 120 }, { 200, 300, 150 } };             //sila pro vojacky (soldier, ranger, tank)
    public float[] ranges = { 0.5f, 1.7f, 0.1f };//velikost kde muze bojovat
    public int attackDelay = 2;

    [Header("Rewards")]
    public int[,] moneykill = { { 20, 35, 135 }, { 40, 65, 265 }, { 80, 135, 535 }, { 160, 265, 1065 }, { 320, 535, 2135 } };             //peniza za zabiti nepritele (soldier, ranger, tank)
    public int[] expperkill = { 100, 125, 300 };                //zkusenosti za zabiti nepritele (soldier, ranger, tank)

	[Header("Speed")]
    public float movespeed = 1f;//rychlost pohybu objektu



    /*// Start is called before the first frame update
    void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}*/
}
