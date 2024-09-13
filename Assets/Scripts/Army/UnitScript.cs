using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;                              //importovani random

public class UnitScript : MonoBehaviour
{
	//importovane scripty
	[Header("Importing scripts")]
	ProgresScript progresS;                                     //importuje script zakladny v levo(hrace)
	HpScript hpS;                                               //importuje script zakladny v levo(hrace)
	EnemySpawn enemyS;                                          //importuje script zakladny v pravo(enemy)


	private UnitScript SoldierArmyScript;                          //importovani scriptu, ktery bude slouzit pro vojacka, aby si nasel nepritele

    UnitScript armyScriptForOpponent;                                //import scriptu protivnika
																	 //ArmyScript armyScriptP;                                        //import scriptu spojence


	//animace
	public Animator animator;                                   //pro import animatoru

	//sound
	public AudioSource attackSound;
	public static float sfxSound = 0.35f;


	//
	[Header("Opponent layer")]
	public LayerMask opponent;                                  //layer nepratelskych jednotek typu soldier
	public LayerMask opponentBase;                              //layer nepratelske zakladny

	//zaklad pro pohyb a gravitaci
	public Rigidbody2D rb;                                      //funkce pro gravitaci
	public float movespeed;                                     //rychlost pohybu objektu

	//
	[Header("Unit type")]
	public LayerMask armyType;                                  //typ jednotky

	[Header("Unit types")]
	public LayerMask[] armyTypeLayer;
	public LayerMask allies;                                    //Layer pratelskych vojacku
	public int armyTypeNum = 0;                                 //toto definuje jaky je to typ vojaka

	//Ohledne HPbaru
	[Header("Showing HP")]
	public GameObject hpBar;

	[Header("Attributes")]
	public float currhp;
	private float unitRange;
	private float hpinprocents = 1f;
	public int lvl = 0;                                         //uchovani urovne vojacka											//kvuli fireball je public


	private int[] moveDir = { 1, -1, 0 };
	public UnitTeam team;
	private int teamInt => (int)team; 

	private bool[] checkCollision = { false, false, false };    //zda vidi nepritele, zda vidi zakladnu nepritele, zda vidi spojence
																//public bool isBase = false;

	private Vector3 distanceFromAllie;


	private bool canGiveDmg = false;                                //zda muze bojovat
	private bool foundEnemy = false;                                //zda nasel nepritele

	private float distance;
	private float previousdistance;

	// Start is called before the first frame update
	void Start()
	{
		//Sound                         //to na sound udìlat better
		attackSound.volume = ButtonsMenu.volumeSFX;
		sfxSound = attackSound.volume;


		/*GameObject script1 = GameObject.FindWithTag("baseP");   //toto najde zakladnu hrace pomoci tagu ktery ma
		progresS = script1.GetComponent<ProgresScript>();
		hpS = script1.GetComponent<HpScript>();
		//
		GameObject script2 = GameObject.FindWithTag("baseE");   //toto najde zakladnu nepritele pomoci tagu ktery ma
		enemyS = script2.GetComponent<EnemySpawn>();*/

		if (team == UnitTeam.Left)
		{
			lvl = ProgresScript.level;                               //zde se urci jaky level bude mit pro hrace
		}
		else
		{
			lvl = EnemySpawn.level;                                 //zde se urci jaky level bude mit pro nepritele
		}

		for (int layer = 0; layer < armyTypeLayer.Length; layer++)
		{
			if (armyType == armyTypeLayer[layer])
			{
				armyTypeNum = layer;
				currhp = UnityConfiguration.maxhp[lvl, layer];
				canGiveDmg = true;
				if (layer == 1)
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
			}
			animator.SetInteger("Class", armyTypeNum + 1);
			animator.SetInteger("Level", lvl);
		}
	}
	// Update is called once per frame
	void Update()
	{
		DetectEnemy();
		CheckForCollision();
		Move();
		CheckDead();
		UpdateHP();

		//IDK PROC BY TU TO MELO BYT (canGiveDmg), ALE KDYZ TU TO NENI TAK SE STANE LOOP U ZAKLADNY
		if (canGiveDmg && !LogScript.isGameOver)
		{
			Attack();
		}

		Animation();
	}
	public void DetectEnemy()                                                                                                               //ZATIM OK
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
				if (Mathf.Abs(transform.position.x) - Mathf.Abs(detectedObjects[i].transform.position.x) < distance)
				{
					distance = Mathf.Abs(transform.position.x) - Mathf.Abs(detectedObjects[i].transform.position.x);
					SoldierArmyScript = detectedObjects[i].GetComponent<UnitScript>();
					armyScriptForOpponent = SoldierArmyScript;
				}
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
	private void CheckForCollision()                                                                                                            //ZATIM NENI OK NEVIM ZDA BUDE FUNGOVAT BEZ NOTFULLSIZE
	{
		//float notFullsize = 0.30f;                              //slouzi pro mensi odstup od objektu, aby kontroloval odstup mezi spojenci


		float borderObject = GetComponent<SpriteRenderer>().bounds.size.x / 2;
		distanceFromAllie = new Vector3(transform.position.x + (UnityConfiguration.ranges[2] * 1.25f) * moveDir[teamInt] + borderObject * moveDir[teamInt], transform.position.y, transform.position.y);

		checkCollision[0] = Physics2D.OverlapCircle(transform.position, unitRange, opponent);               //zda vidi enemy tak stoji
		checkCollision[1] = Physics2D.OverlapCircle(transform.position, unitRange, opponentBase);           //zda vidi nepratelskou zakladnu
		checkCollision[2] = Physics2D.OverlapCircle(distanceFromAllie, 0.09f, allies);           //zda vidi spojence tak se zastavi (je urceno pro vsechny)

	}
	private void Move()                                                                                                                     //ZATIM NENI OK NEVIM ZDA BUDOU FUNGOVAT ANIMACE
	{
		//pokud vojacek narazi na jakoukoliv kolizi tak se zastavi (enemy, enemy base, allies)
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
		if (currhp <= 0)
		{
			//Pokud se jedna o nepritele
			if (team != 0)
			{
				Reward();
			}
			Destroy(gameObject);
			return;
		}

		//Pokud jednotka se propadne
		if (transform.position.y < -20)
		{
			Destroy(gameObject);
			return;
		}
	}
	private void UpdateHP()
	{
		hpinprocents = ((100 * currhp) / UnityConfiguration.maxhp[lvl, armyTypeNum]) / 100;
		hpBar.transform.localScale = new Vector2(hpinprocents, hpBar.transform.localScale.y);
	}
	private void Attack()                           //SPATNE FUNGUJE DMG DO ZAKLADNY (JE TO V LOOP)
	{
		if (canGiveDmg && checkCollision[0] || checkCollision[1])
		{
			canGiveDmg = false;
			StartCoroutine(Attacking());
			//Debug.Log("AHOJ");
		}
	}
	private IEnumerator Attacking()
	{
		canGiveDmg = false;
		//Debug.Log("can attack");
		animator.SetBool("ScriptFound", true);
		yield return new WaitForSeconds(UnityConfiguration.attackDelay);
		//Debug.Log("Attacking");
		int randomDmg = Random.Range(UnityConfiguration.dmgMin[lvl, armyTypeNum], UnityConfiguration.dmgMax[lvl, armyTypeNum]);
		if (checkCollision[0] && foundEnemy == true)
		{
			armyScriptForOpponent.currhp -= randomDmg;
			attackSound.Play();
		}
		else if (checkCollision[1])
		{
			if (team == 0)
			{
				Debug.Log("Hit base to E");
                EnemySpawn.currHPBase -= randomDmg;
				attackSound.Play();
			}
			else
			{
				Debug.Log("Hit base to P");
				HpScript.currHPBase -= randomDmg;
				attackSound.Play();
			}
		}
		animator.SetBool("ScriptFound", false);
		canGiveDmg = true;
	}


	private void Reward()
	{
		ProgresScript.money += UnityConfiguration.moneykill[lvl, armyTypeNum];
		ProgresScript.experience += UnityConfiguration.expperkill[armyTypeNum];
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