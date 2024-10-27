using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEditor.Experimental.GraphView.GraphView;
using Random = UnityEngine.Random;                              //importovani random

public class UnitScript : MonoBehaviour
{
    //importovane scripty (hrace)
    [Header("Importing scripts")]
	ProgresScript progresS;
    HpScript hpPlayerS;
    EvolutionPlayerScript evolutionPlayerS;

    //ziskani dat
    //public UnitStats unitData;

    //importovane scripty (nepritele)
    EnemySpawn enemyS;
	HpScript hpEnemyS;
    EvolutionEnemyScript evolutionEnemyS;

    //importovani scriptu, ktery bude slouzit pro vojacka, aby si nasel nepritele
    private UnitScript SoldierArmyScript;
    //import scriptu protivnika
    UnitScript armyScriptForOpponent;

    //pro import animatoru
    //animace
    public Animator animator;

	//sound
	public AudioSource attackSound;

	//bool isPlayingSound = false;
	//public static float sfxSound = 0.35f;


	//
	[Header("Opponent layer")]
    //layer nepratelskych jednotek typu soldier
    public LayerMask opponent;
    //layer nepratelske zakladny
    public LayerMask opponentBase;

    //zaklad pro pohyb a gravitaci
    //funkce pro gravitaci
    public Rigidbody2D rb;
    //rychlost pohybu objektu
    public float movespeed;

	//typ jednotky
	[Header("Unit type")]
	public LayerMask armyType;

	[Header("Unit types")]
	public LayerMask[] armyTypeLayer;
    //Layer pratelskych vojacku
    public LayerMask allies;
    //toto definuje jaky je to typ vojaka
    public int armyTypeNum = 0;

	[Header("Attributes")]
	public float currhp;
	private float unitRange;

    //uchovani urovne vojacka
    public int lvl = 0;


	private int[] moveDir = { 1, -1, 0 };
	public UnitTeam team;
	private int teamInt => (int)team;

    //zda vidi nepritele, zda vidi zakladnu nepritele, zda vidi spojence
    private bool[] checkCollision = { false, false, false };

	//public bool isBase = false;

	private Vector3 distanceFromAllie;

    //zda muze bojovat
    public bool canGiveDmg = false;
    //zda nasel nepritele
    public bool foundEnemy = false;

	private float distance;
	//private float previousdistance;

	public float chargeAttack = 0f;

    //public bool isDead = false;

    // Start is called before the first frame update
    private void Awake()
    {
        //toto najde zakladnu hrace pomoci tagu ktery ma
        GameObject script1 = GameObject.FindWithTag("baseP");
        progresS = script1.GetComponent<ProgresScript>();
        hpPlayerS = script1.GetComponent<HpScript>();
        evolutionPlayerS = script1.GetComponent<EvolutionPlayerScript>();

        //toto najde zakladnu nepritele pomoci tagu ktery ma
        GameObject script2 = GameObject.FindWithTag("baseE");
        enemyS = script2.GetComponent<EnemySpawn>();
        hpEnemyS = script2.GetComponent<HpScript>();
        evolutionEnemyS = script2.GetComponent<EvolutionEnemyScript>();


    }
    void Start()
	{
		//Sound ziskani hodnot z menu
		attackSound.volume = ButtonsMenu.volumeSFX;

		if (team == UnitTeam.Left)
		{
            //urceni levelu, ktery bude mit jednotky hrace
            lvl = evolutionPlayerS.level;
		}
		else
		{
            //urceni levelu, ktery bude mit jednotky nepritele
            lvl = evolutionEnemyS.level;
		}

		int index;
		for (index = 0; index < armyTypeLayer.Length; index++)
		{
			if(armyType == armyTypeLayer[index])
            {
                break;
            }
        }

        armyTypeNum = index;
        currhp = UnityConfiguration.maxhp[index] * (lvl+1);
        canGiveDmg = true;
        //kdyz layer je roven range, tak dostane vetsi pole dostrelu, jinak melee
        if (index == 1)
        {
            //canGiveDmgR = true;
            //unitRange = ranges[1];
            unitRange = UnityConfiguration.ranges[1];

        }
        else
        {
            //canGiveDmgM = true;
            //unitRange = ranges[0];
            unitRange = UnityConfiguration.ranges[0];
        }
		//nastaveni animace, tim se nastavi vzhled aktualniho levelu a tridy dane jednotky
        animator.SetInteger("Class", index + 1);
        animator.SetInteger("Level", lvl);

    }
	// Update is called once per frame
	void Update()
	{
		DetectEnemy();
		CheckForCollision();
		Move();
		CheckDead();

		//IDK PROC BY TU TO MELO BYT (canGiveDmg), ALE KDYZ TU TO NENI TAK SE STANE LOOP U ZAKLADNY
		/*if (canGiveDmg && !LogScript.isGameOver)
		{
			Attack();
		}*/
		if (foundEnemy || checkCollision[1])
		{
			ChargeAttack();
            //animator.SetBool("ScriptFound", true);
        }
		else
		{
			chargeAttack = 0f;
            animator.SetBool("ScriptFound", false);
        }

		Animation();
	}
	public void DetectEnemy()
	{
		//Nalezne nepritele a dosadi script za konkretni objekt
		Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(transform.position, unitRange, opponent);
		if (detectedObjects.Length > 0)
		{
			distance = 5f;
			for (int i = 0; i < detectedObjects.Length; i++)
			{
                /*Debug.Log(Mathf.Abs(transform.position.x) + gameObject.name + i);
                Debug.Log(Mathf.Abs(detectedObjects[i].transform.position.x) + gameObject.name + i);*/
                if (Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(detectedObjects[i].transform.position.x)) < distance)
				{
					distance = Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(detectedObjects[i].transform.position.x));
					SoldierArmyScript = detectedObjects[i].GetComponent<UnitScript>();
					armyScriptForOpponent = SoldierArmyScript;
				}
                /*if (Mathf.Abs(detectedObjects[i].transform.position.x) < distance)
                {
                    distance = Mathf.Abs(detectedObjects[i].transform.position.x);
                    SoldierArmyScript = detectedObjects[i].GetComponent<UnitScript>();
                    armyScriptForOpponent = SoldierArmyScript;
                }*/
            }

            //Debug.Log(armyScriptForOpponent.currhp);
            foundEnemy = true;
			//animator.SetBool("ScriptFound", true);
			return;
		}
		foundEnemy = false;
        //animator.SetBool("ScriptFound", false);
        return;
	}
	private void CheckForCollision()
	{
		//float notFullsize = 0.30f;                              //slouzi pro mensi odstup od objektu, aby kontroloval odstup mezi spojenci


		float borderObject = GetComponent<SpriteRenderer>().bounds.size.x / 2;
		distanceFromAllie = new Vector3(transform.position.x + (UnityConfiguration.ranges[2] * 1.25f) * moveDir[teamInt] + borderObject * moveDir[teamInt], transform.position.y, transform.position.y);

		checkCollision[0] = Physics2D.OverlapCircle(transform.position, unitRange, opponent);               //zda vidi enemy tak stoji
		checkCollision[1] = Physics2D.OverlapCircle(transform.position, unitRange, opponentBase);           //zda vidi nepratelskou zakladnu
		checkCollision[2] = Physics2D.OverlapCircle(distanceFromAllie, 0.09f, allies);           //zda vidi spojence tak se zastavi (je urceno pro vsechny)

	}
	private void Move()
	{
		//pokud vojacek narazi na jakoukoliv kolizi tak se zastavi (enemy, enemy base, allies)
		/*if (isDead)
		{
			movespeed = 0;
		}*/
		if (checkCollision[0] || checkCollision[1] || checkCollision[2])
		{
			rb.velocity = new Vector2((movespeed * moveDir[2]), rb.velocity.y);     //nebude se hybat pokud je poblic kolize

			animator.SetFloat("Speed", 0);                                          //dosazeni za promennou speed, ktera urcuje animace
		}
		//pokud nebude zadna kolize tak bude chodit
		else
		{
			rb.velocity = new Vector2((movespeed * moveDir[teamInt]), rb.velocity.y);

			animator.SetFloat("Speed", rb.velocity.x * moveDir[teamInt]);               //dosazeni za promennou speed, ktera urcuje animace
		}
	}
	private void CheckDead()
	{
		//Pokud jednotka nebude mit zivoty
		if (currhp <= 0 || transform.position.y < UnityConfiguration.deadZone)
		{
			//Pokud se jedna o nepritele
			if (team != 0 && transform.position.y > UnityConfiguration.deadZone)
			{
                //isDead = true;
                Reward();
			}
			attackSound.Stop();
			if (!attackSound.isPlaying)
			{
                Destroy(gameObject);
            }
            /*gameObject.layer = LayerMask.NameToLayer("Dead");
            if (transform.position.y < deadZone)
            {
                Destroy(gameObject);
            }*/
            return;
		}
	}

	private void ChargeAttack()
	{
        animator.SetBool("ScriptFound", true);
        chargeAttack = Mathf.Lerp(chargeAttack, chargeAttack+1f, Time.deltaTime / UnityConfiguration.attackDelay);
		if(chargeAttack >= 1)		//pri dead animaci dat do podminky isDead
		{
			chargeAttack = 0;
            animator.SetBool("ScriptFound", true);
            int randomDmg = Random.Range(UnityConfiguration.dmgMin[armyTypeNum] * (lvl+1), UnityConfiguration.dmgMax[armyTypeNum] * (lvl + 1));
			//attackSound.Stop();
			//attackSound.Play();
			PlaySFX();
            if (checkCollision[0] && foundEnemy == true)
            {
                armyScriptForOpponent.currhp -= randomDmg;
                //attackSound.Play();
            }
            else if (checkCollision[1])
            {
                if (team == 0)
                {
                    //Debug.Log($"Hit base to E: {hpEnemyS.currHPBase}");
                    hpEnemyS.currHPBase -= randomDmg;
                    //attackSound.Play();
                }
                else
                {
                    //Debug.Log($"Hit base to P: {hpPlayerS.currHPBase}");
                    hpPlayerS.currHPBase -= randomDmg;
                    //attackSound.Play();
                }
            }
            animator.SetBool("ScriptFound", false);
            //attackSound.Stop();
            //Debug.Log("Dohrala se animece a uderil");
        }
	}

	private void PlaySFX()
	{
		if (gameObject != null || attackSound != null)
		{
            attackSound.Play();
        }
        //AudioSource.PlayClipAtPoint(attackSound.clip, transform.position);
    }


    private void Reward()
	{
        progresS.money += UnityConfiguration.moneykill[armyTypeNum] * (lvl+1);
        evolutionPlayerS.experience += UnityConfiguration.expperkill[armyTypeNum];
	}

	private void Animation()
	{
		if (checkCollision[0] || checkCollision[1])
		{
			animator.SetBool("EnemyNear", true);
			return;
		}
		animator.SetBool("EnemyNear", false);
		return;
	}


	private void OnDrawGizmosSelected()     //vykreslí kruh okolo jednotky
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(distanceFromAllie, UnityConfiguration.ranges[2]);                    //odstup od spojencu
		/*Gizmos.DrawWireSphere(transform.position, ranges[1]);                   //stop pro rangera
		Gizmos.DrawWireSphere(transform.position, ranges[0]);                   //stop pro melee*/

		Gizmos.DrawWireSphere(transform.position, unitRange);
	}
}

public enum UnitTeam { 
	Left , Right
}