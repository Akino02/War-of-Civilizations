using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityConfiguration
{
	[Header("HP Unit")]
    public static float[,] maxhp = { { 100, 60, 300 }, { 200, 160, 400 }, { 300, 260, 500 }, { 400, 360, 600 }, { 500, 460, 700 } };       //kvuli fireball je public

	[Header("Attack")]
	public static int[,] dmgMin = { { 20, 30, 15 }, { 40, 60, 30 }, { 60, 90, 45 }, { 80, 120, 60 }, { 100, 150, 75 } };             //sila pro vojacky (soldier, ranger, tank)
    public static int[,] dmgMax = { { 40, 60, 30 }, { 80, 120, 60 }, { 120, 180, 90 }, { 160, 240, 120 }, { 200, 300, 150 } };             //sila pro vojacky (soldier, ranger, tank)
    public static float[] ranges = { 0.5f, 1.7f, 0.1f };//velikost kde muze bojovat
    public static int attackDelay = 2;

    [Header("Rewards")]
    public static int[,] moneykill = { { 20, 35, 135 }, { 40, 65, 265 }, { 80, 135, 535 }, { 160, 265, 1065 }, { 320, 535, 2135 } };             //peniza za zabiti nepritele (soldier, ranger, tank)
    public static int[] expperkill = { 100, 125, 300 };                //zkusenosti za zabiti nepritele (soldier, ranger, tank)

	[Header("Speed")]
    public static float movespeed = 1f;//rychlost pohybu objektu

	[Header("DeadZone")]
    public static int deadZone = -10;


    [Header("HP Base")]
    public static float[] maxHPBase = { 1000, 2000, 3000, 4000, 5000 };

	[Header("Next LevelUp")]
    public static int nextlevelup = 4000;                              //pokud dosahne tolika zkusenosti tak se evolvuje

	[Header("MoneyPerUnit")]
    public static int[,] moneyperunit = { { 15, 25, 100 }, { 30, 50, 200 }, { 60, 100, 400 }, { 120, 200, 800 }, { 240, 400, 1600 } };      //vícerozmìrné pole pro cenu jednotek

	//[Header("ProductionBar")]
	[Header("CameraType")]
    public static int cameraMoveType = 0;
	public static bool[,] cameraTypeImage = { {true, true}, {true, false}, {false, true} };		//format keyboard, mouse


    /*// Start is called before the first frame update
    void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}*/
}
