using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityConfiguration
{
    //Unit HP
    public static float[] maxhp = { 100, 60, 300 };

    //Attack
    //sila jednotky (soldier, ranger, tank)
    public static int[] dmgMin = { 20, 30, 15 };
    public static int[] dmgMax = { 40, 60, 30 };
    public static float[] ranges = { 0.5f, 1.7f, 0.1f };
    public static int attackDelay = 2;

    //Speed
    public static float movespeed = 1f;

    [Header("DeadZone")]
    public static int deadZone = -10;


    [Header("HP Base")]
    public static float maxHPBase = 1000;

	[Header("Next LevelUp")]
    //pokud dosahne tolika zkusenosti tak se evolvuje
    public static int nextlevelup = 4000;

    [Header("Rewards")]
    //peniza za zabiti nepritele (soldier, ranger, tank)
    public static int[] moneykill = { 15, 25, 150 };
    //zkusenosti za zabiti nepritele (soldier, ranger, tank)
    public static int[] expperkill = { 100, 125, 300 };

    [Header("MoneyPerUnit")]
    //cena jednotek
    public static int[] moneyperunit = { 15, 25, 150 };

    [Header("MoneyForTurret")]
    //cena za vez
    public static int moneyForTurret = 750;

    //vlastnosti veze
    public static float bulletDamage = 15f;
    public static float bulletSpeed = 2f;
    public static float turretRange = 2f;
    public static float bulletDistance = 25f;

    public static float fireRate = 2f;

    [Header("CameraType")]
    public static MoveType cameraMoveType = MoveType.Keyboard | MoveType.Mouse;

    //udelat script konstatni na text 
    public static string[] trainingTextShow = { "You aren't crafting", "Training Soldier...", "Training Ranger...", "Training Tank..." };

    public static string[] possibleText = { "You don't have enough money", "You have a full queue", "You Won", "You Lost", "You can't evolve yet", "You have reached maximum level", "You must wait until next attack"};

    public static string[] buttonLabelTurret = { "Buy turret", "Sell turret"};

    public static string[] buttonLabelChangeActionBoard = { "Turret", "Units" };
}

//[Flags]
public enum MoveType
{
    None = 0,
    Keyboard = 1,
    Mouse = 2,
}

/*class Warrior
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
}*/