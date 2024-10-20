using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityConfiguration
{
    //Unit HP
    public static float[,] maxhp = { { 100, 60, 300 }, { 200, 160, 400 }, { 300, 260, 500 }, { 400, 360, 600 }, { 500, 460, 700 } };

    //Attack
    public static int[,] dmgMin = { { 20, 30, 15 }, { 40, 60, 30 }, { 60, 90, 45 }, { 80, 120, 60 }, { 100, 150, 75 } };             //power of unit (soldier, ranger, tank)
    public static int[,] dmgMax = { { 40, 60, 30 }, { 80, 120, 60 }, { 120, 180, 90 }, { 160, 240, 120 }, { 200, 300, 150 } };
    public static float[] ranges = { 0.5f, 1.7f, 0.1f };
    public static int attackDelay = 2;

    //Speed
    public static float movespeed = 1f;

    [Header("DeadZone")]
    public static int deadZone = -10;


    [Header("HP Base")]
    public static float maxHPBase = 1000;

	[Header("Next LevelUp")]
    public static int nextlevelup = 4000;                              //pokud dosahne tolika zkusenosti tak se evolvuje

    [Header("Rewards")]
    //public static int[,] moneykill = { { 20, 35, 135 }, { 40, 65, 265 }, { 80, 135, 535 }, { 160, 265, 1065 }, { 320, 535, 2135 } };             //peniza za zabiti nepritele (soldier, ranger, tank)
    public static int[,] moneykill = { { 15, 25, 150 }, { 30, 50, 300 }, { 60, 100, 600 }, { 120, 200, 1200 }, { 240, 400, 2400 } };             //peniza za zabiti nepritele (soldier, ranger, tank)
    public static int[] expperkill = { 100, 125, 300 };                //zkusenosti za zabiti nepritele (soldier, ranger, tank)

    [Header("MoneyPerUnit")]
    //public static int[,] moneyperunit = { { 15, 25, 100 }, { 30, 50, 200 }, { 60, 100, 400 }, { 120, 200, 800 }, { 240, 400, 1600 } };      //vícerozmìrné pole pro cenu jednotek
    public static int[,] moneyperunit = { { 15, 25, 150 }, { 30, 50, 300 }, { 60, 100, 600 }, { 120, 200, 1200 }, { 240, 400, 2400 } };      //vícerozmìrné pole pro cenu jednotek

	//[Header("ProductionBar")]
	[Header("CameraType")]
    public static MoveType cameraMoveType = MoveType.Keyboard | MoveType.Mouse;
    //public static bool[,] cameraTypeImage = { {true, true}, {true, false}, {false, true} };		//format keyboard, mouse



    //udelat script konstatni na text 
    public static string[] trainingTextShow = { "You aren't crafting", "Training Soldier...", "Training Ranger...", "Training Tank..." };

    public static string[] possibleText = { "You don't have enough money", "You have a full queue", "You Won", "You Lost" };
}

[Flags]
public enum MoveType
{
    None = 0,
    Keyboard = 1,
    Mouse = 2,
}

class Warrior
{
    public float maxHp;
    public float currHp;
    public int minDmg;
    public int maxDmg;
    public float range;
    public int attackDelay;
    public int rewardMoney;
    public int rewardXP;
    public int cost;
    public bool isPlayer;
    public WarriorClass warriorClass;

    public Warrior(float maxHp, int minDmg, int maxDmg, float range, int attackDelay, int rewardMoney, int rewardXP, int cost, bool isPlayer, WarriorClass warriorClass)
    {
        this.maxHp = maxHp;
        this.currHp = maxHp;
        this.minDmg = minDmg;
        this.maxDmg = maxDmg;
        this.range = range;
        this.attackDelay = attackDelay;
        this.rewardMoney = rewardMoney;
        this.rewardXP = rewardXP;
        this.cost = cost;
        this.isPlayer = isPlayer;
        this.warriorClass = warriorClass;
    }

    public class Factory
    {
        public int level;
        public bool isPlayer;

        public Factory(int level, bool isPlayer = true)
        {
            this.level = level;
            this.isPlayer = isPlayer;
        }

        public Warrior createSoldier() => new Warrior(100*level, 20 * level, 40 * level, 0.5f, 2, 15 * level, 100, 15, isPlayer, WarriorClass.Soldier);
        public Warrior createRanger() => new Warrior(100*level, 20 * level, 40 * level, 0.5f, 2, 15 * level, 100, 15, isPlayer, WarriorClass.Soldier);
        public Warrior createTank() => new Warrior(100*level, 20 * level, 40 * level, 0.5f, 2, 15 * level, 100, 15, isPlayer, WarriorClass.Soldier);
    }
}
public enum WarriorClass
{
    Soldier = 0,
    Ranger = 1,
    Tank = 2
}