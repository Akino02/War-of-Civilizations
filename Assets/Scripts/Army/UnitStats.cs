using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Unit Type", menuName = "New Unit Type")]
public class UnitStats : ScriptableObject
{
    [Range(60f, 300f)]
    public float maxHp;

    public float currHp;

    [Range(20, 30)]
    public int minDmg;

    [Range(30, 60)]
    public int maxDmg;

    [Range(0.1f, 1.7f)]
    public float range;

    [Range(2, 3)]
    public int attackDelay;

    [Range(15, 150)]
    public int rewardMoney;

    [Range(100, 300)]
    public int rewardXP;

    [Range(15, 150)]
    public int cost;

    public bool isPlayer;

    //public WarriorClass warriorClass;
    public LayerMask unitClass;

    /*//Unit HP
    public float[,] maxhp = { { 100, 60, 300 }, { 200, 160, 400 }, { 300, 260, 500 }, { 400, 360, 600 }, { 500, 460, 700 } };

    //Attack
    public int[,] dmgMin = { { 20, 30, 15 }, { 40, 60, 30 }, { 60, 90, 45 }, { 80, 120, 60 }, { 100, 150, 75 } };             //power of unit (soldier, ranger, tank)
    public int[,] dmgMax = { { 40, 60, 30 }, { 80, 120, 60 }, { 120, 180, 90 }, { 160, 240, 120 }, { 200, 300, 150 } };
    public float[] ranges = { 0.5f, 1.7f, 0.1f };
    public int attackDelay = 2;
    public static int[,] moneykill = { { 15, 25, 150 }, { 30, 50, 300 }, { 60, 100, 600 }, { 120, 200, 1200 }, { 240, 400, 2400 } };             //peniza za zabiti nepritele (soldier, ranger, tank)
    public static int[] expperkill = { 100, 125, 300 };                //zkusenosti za zabiti nepritele (soldier, ranger, tank)
    public static int[,] moneyperunit = { { 15, 25, 150 }, { 30, 50, 300 }, { 60, 100, 600 }, { 120, 200, 1200 }, { 240, 400, 2400 } };      //vícerozmìrné pole pro cenu jednotek

    //Speed
    public float movespeed = 1f;*/

    /*public UnitStats(float maxHp, int minDmg, int maxDmg, float range, int attackDelay, int rewardMoney, int rewardXP, int cost, bool isPlayer, WarriorClass warriorClass)
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

        public UnitStats createSoldier() => new UnitStats(100 * level, 20 * level, 40 * level, 0.5f, 2, 15 * level, 100, 15, isPlayer, WarriorClass.Soldier);
        public UnitStats createRanger() => new UnitStats(100 * level, 20 * level, 40 * level, 0.5f, 2, 15 * level, 100, 15, isPlayer, WarriorClass.Soldier);
        public UnitStats createTank() => new UnitStats(100 * level, 20 * level, 40 * level, 0.5f, 2, 15 * level, 100, 15, isPlayer, WarriorClass.Soldier);
    }*/

}