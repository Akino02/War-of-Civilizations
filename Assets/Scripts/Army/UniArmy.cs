using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UniArmy : MonoBehaviour
{
	//importovane scripty
	ProgresScript progresS;										//importuje script zakladny v levo(hrace)
	HpScript hpS;												//importuje script zakladny v levo(hrace)
	EnemySpawn enemyS;											//importuje script zakladny v pravo(enemy)

	UniArmy armyScriptE;										//import scriptu protivnika
	UniArmy armyScriptP;										//import scriptu spojence

	private UniArmy SoldierArmyScript;							//importovani scriptu, ktery bude slouzit pro vojacka, aby si nasel nepritele

	//animace
	public Animator animator;									//pro import animatoru

	//
	public LayerMask opponent;									//layer nepratelskych jednotek typu soldier
	public LayerMask opponentBase;								//layer nepratelske zakladny
	public float[] ranges = { 0.5f, 1.45f, 0.1f};				//velikost kde muze bojovat
	public LayerMask armyType;                                  //typ jednotky

	//zaklad pro pohyb a gravitaci
	public Rigidbody2D rb;                                      //funkce pro gravitaci
	public float movespeed;                                     //rychlost pohybu objektu

	//vsechny typy jednotek
	public LayerMask soldier;									//Layer pro vojacka typu SOLDIER
	public LayerMask ranger;									//Layer pro vojacka typu RANGER
	public LayerMask tank;										//Layer pro vojacka typu TANK
	public LayerMask allies;									//Layer pratelskych vojacku
	public int armyTypeNum = 0;									//toto definuje jaky je to typ vojaka

	//Ohledne HPbaru
	public GameObject hpBar;

	private float[,] maxhp = { { 100, 60, 300 }, { 150, 90, 450 }, { 225, 135, 675 }, { 350, 200, 1000 }, { 400, 300, 1500 } };
	public float currhp = 100;
	private float hpinprocents = 1f;
	public int lvl = 0;											//uchovani urovne vojacka


	private int[] moveDir = { 1, -1, 0};
	public int dir;

	public bool[] checkCollision = { false, false , false, false, false};	//zda soldier vidi, zda ranger vidi, zda spojenci se vidi, zda vidi zakladnu(melee), zda vidi zakladnu(ranger)
	public Vector3 distanceFromAllie;


	//Ohledne utoku
	private int[,] dmg = { { 40, 60, 30 }, { 60, 90, 50 }, { 90, 135, 70 }, { 135, 90, 115 }, { 150, 200, 120 } };             //sila pro vojacky (soldier, ranger, tank)

	public bool canGiveDmgM = false;							//Muze bojovat melee
	public bool canGiveDmgR = false;							//Muze bojovat na dalku

	//Odmeny za to ze zemre
	public int[,] moneykill = { { 30, 50, 150 }, { 60, 100, 300 }, { 120, 200, 600 }, { 240, 400, 1200 }, { 480, 800, 2400 } };             //peniza za zabiti nepritele (soldier, ranger, tank)
	public int[] expperkill = { 100, 125, 300 };				//zkusenosti za zabiti nepritele (soldier, ranger, tank)
	//

	// Start is called before the first frame update
	void Start()
	{
		GameObject script1 = GameObject.FindWithTag("baseP");	//toto najde zakladnu hrace pomoci tagu ktery ma
		progresS = script1.GetComponent<ProgresScript>();
		hpS = script1.GetComponent<HpScript>();
		//
		GameObject script2 = GameObject.FindWithTag("baseE");	//toto najde zakladnu nepritele pomoci tagu ktery ma
		enemyS = script2.GetComponent<EnemySpawn>();

		if(dir == 0)
		{
			lvl = progresS.level;                               //zde se urci jaky level bude mit pro hrace
		}
		else
		{
			lvl = enemyS.level;                                 //zde se urci jaky level bude mit pro nepritele
			transform.Rotate(0f, 180f, 0f);
		}

		if (armyType == soldier)								//nastaveni co je to za druh vojacka
		{
			armyTypeNum = 1;
			//maxhp = hptypes[lvl,armyTypeNum - 1];
			currhp = maxhp[lvl, armyTypeNum - 1];
			canGiveDmgM = true;
		}
		else if (armyType == ranger)
		{
			armyTypeNum = 2;
			//maxhp = hptypes[lvl,armyTypeNum - 1];
			currhp = maxhp[lvl, armyTypeNum - 1];
			canGiveDmgR = true;
		}
		else if (armyType == tank)
		{
			armyTypeNum = 3;
			//maxhp = hptypes[lvl,armyTypeNum - 1];
			currhp = maxhp[lvl, armyTypeNum - 1];
			canGiveDmgM = true;
		}
		animator.SetInteger("Class", armyTypeNum);
		animator.SetInteger("Level", lvl);
	}
	// Update is called once per frame
	void Update()
	{
		CheckForEnemy();	//tato funkce zajistuje kontrolovani kolizi v okoli
		hpinprocents = ((100 * currhp) / maxhp[lvl, armyTypeNum - 1]) / 100;
		Move();      //tato funkce zajistuje pohyb
		//DamageBase();											//ubirani zivotu primo zakladne
		if (checkCollision[0] && canGiveDmgM == true && armyTypeNum != 2)     //je tam if, aby to poznaval hned
		{
			FindMyEnemy();
		}
		else if (checkCollision[1] && canGiveDmgR == true && armyTypeNum == 2)		//je tam if, aby to poznaval hned
		{
			FindMyEnemy();
		}
		else if (checkCollision[3] && canGiveDmgM == true && armyTypeNum != 2)
		{
			StartCoroutine(DmgDealCoolDownMeleeBase());
		}
		else if (checkCollision[4] && canGiveDmgR == true && armyTypeNum == 2)
		{
			StartCoroutine(DmgDealCoolDownRangerBase());
		}
		if (currhp <= 0)
		{
			if(dir != 0)										//tato podminka bude davat penize a zkusenosti pri tom kdyz zemre enemy
			{
				Reward();
			}
			Destroy(gameObject);
		}
		hpBar.transform.localScale = new Vector2(hpinprocents, hpBar.transform.localScale.y);
	}
	void FindMyEnemy()											//tato funkce hleda nepritele a k nemu priradi script a jeste se zavola dalsi funkce na utok
	{
		armyScriptP = null;
		armyScriptE = null;

		GameObject[] allEnemies = (dir == 1) ? GameObject.FindGameObjectsWithTag("Player") : GameObject.FindGameObjectsWithTag("Enemy");

		foreach (GameObject obj in allEnemies.Reverse())        //tady se otoci porazi aby bral toho prvniho enemy vzdy
		{
			SoldierArmyScript = obj.GetComponent<UniArmy>();
			if (obj.layer == 10 && dir == 1)
			{
				armyScriptP = SoldierArmyScript;                //dosazeni scriptu za objekt
				//Debug.Log("Allie attacked");
			}
			else if (obj.layer == 13 && dir == 0)
			{
				armyScriptE = SoldierArmyScript;                //dosazeni scriptu za objekt
				//Debug.Log("Enemy attacked");
			}
		}
		if (armyTypeNum == 1 || armyTypeNum == 3)
		{
			StartCoroutine(DmgdealcooldownMelee());
		}
		else if (armyTypeNum == 2)
		{
			StartCoroutine(DmgdealcooldownRange());
		}
		// Debug.Log("Can give ");
	}
	void CheckForEnemy()
	{
		//float notFullsize = 0.30f;                              //slouzi pro mensi odstup od objektu, aby kontroloval odstup mezi spojenci
		float borderObject = GetComponent<SpriteRenderer>().bounds.size.x / 2;
		distanceFromAllie = new Vector3(transform.position.x + (ranges[2] * 1.25f) * moveDir[dir] +borderObject * moveDir[dir], transform.position.y, transform.position.y);
		checkCollision[0] = Physics2D.OverlapCircle(transform.position, ranges[0], opponent);           //zda vidi enemy tak stoji (urceno pro melee class)
		checkCollision[3] = Physics2D.OverlapCircle(transform.position, ranges[0], opponentBase);           //zda vidi nepratelskou zakladnu (urceno pro melee class(range))
		if(armyTypeNum == 2)			//pokud je to Ranger tak ten se zastavi dale, protoze muze zabijet z dalky
		{
			checkCollision[1] = Physics2D.OverlapCircle(transform.position, ranges[1], opponent);           //zda vidi enemy tak stoji (urceno pro ranger class)
			checkCollision[4] = Physics2D.OverlapCircle(transform.position, ranges[1], opponentBase);           //zda vidi nepratelskou zakladnu (urceno pro ranger class(range))
		}
		checkCollision[2] = Physics2D.OverlapCircle(distanceFromAllie, 0.09f, allies);           //zda vidi spojence tak se zastavi (je urceno pro vsechny)
	}
	void Move()
	{
		if (checkCollision[0] || checkCollision[1] || checkCollision[2] || checkCollision[3] || checkCollision[4])            //pokud vojacek narazi na jakoukoliv kolizi tak se zastavi (base, armyP, armyE)
		{
			rb.velocity = new Vector2((movespeed * moveDir[2]), rb.velocity.y);		//nebude se hybat pokud je poblic kolize
			animator.SetFloat("Speed", 0);                                          //dosazeni za promennou speed, ktera urcuje animace
        }
		else																		//pokud nebude zadna kolize tak bude chodit
		{
			rb.velocity = new Vector2((movespeed * moveDir[dir]), rb.velocity.y);
			animator.SetFloat("Speed", rb.velocity.x * moveDir[dir]);				//dosazeni za promennou speed, ktera urcuje animace
		}
		Animation();
	}
	void Animation()
	{
		if (checkCollision[0] || checkCollision[3] || checkCollision[1] || checkCollision[4])
		{
			animator.SetBool("EnemyNear", true);
		}
		else
		{
			animator.SetBool("EnemyNear", false);
		}
	}
	void Reward()
	{
		progresS.money += moneykill[lvl, armyTypeNum-1];
		progresS.experience += expperkill[armyTypeNum-1];
		Debug.Log(moneykill[lvl, armyTypeNum - 1]);
		Debug.Log(expperkill[armyTypeNum - 1]);
	}
	IEnumerator DmgdealcooldownMelee()							//tato funkce slouzi pro utok pro soldier a tank
	{
		canGiveDmgM = false;
		yield return new WaitForSeconds(3);
		if (checkCollision[0])
		{
			if (armyTypeNum == 1)
			{
				/*if(opponent != null)
				{*/
				if (dir == 0)
				{
					armyScriptE.currhp -= dmg[lvl, armyTypeNum - 1];
				}
				else
				{
					armyScriptP.currhp -= dmg[lvl, armyTypeNum - 1];
				}
				//SoldierPscript.currhp -= dmg[armyTypeNum - 1];
				//}
				/*else if (opponentBase != null)
				{
					BaseScriptE.currHPBase -= dmg[armyTypeNum - 1];
				}*/
			}
			else if (armyTypeNum == 3)
			{
				if (dir == 0)
				{
					armyScriptE.currhp -= dmg[lvl, armyTypeNum - 1];
				}
				else
				{
					armyScriptP.currhp -= dmg[lvl, armyTypeNum - 1];
				}
				//SoldierPscript.currhp -= dmg[armyTypeNum - 1];
				//BaseScriptE.currHPBase -= dmg[armyTypeNum - 1];
			}
			/*Debug.Log("Player " + SoldierEscript.currhp);
			Debug.Log("Player " + BaseScriptE.currHPBase);*/
			canGiveDmgM = true;
		}
	}
	IEnumerator DmgdealcooldownRange()							//tato funkce slouzi pro utok pro ranger
	{
		canGiveDmgR = false;
		yield return new WaitForSecondsRealtime(2);
		if (checkCollision[1])
		{
			if (dir == 0)
			{
				armyScriptE.currhp -= dmg[lvl, armyTypeNum - 1];
			}
			else
			{
				armyScriptP.currhp -= dmg[lvl, armyTypeNum - 1];
			}
		}
		canGiveDmgR = true;
	}
	IEnumerator DmgDealCoolDownMeleeBase()						//potencionalni problem************************
	{
		canGiveDmgM = false;
		yield return new WaitForSeconds(3);
		if (checkCollision[3])
		{
			if (armyTypeNum == 1)
			{
				if (dir == 0)
				{
					enemyS.currHPBase -= dmg[lvl, armyTypeNum - 1];
				}
				else
				{
					hpS.currHPBase -= dmg[lvl, armyTypeNum - 1];
				}
			}
			else if (armyTypeNum == 3)
			{
				if (dir == 0)
				{
					enemyS.currHPBase -= dmg[lvl, armyTypeNum - 1];
				}
				else
				{
					hpS.currHPBase -= dmg[lvl, armyTypeNum - 1];
				}
			}
		}
		canGiveDmgM = true;
	}

	IEnumerator DmgDealCoolDownRangerBase()                     //potencionalni problem************************
	{
		canGiveDmgR = false;
		yield return new WaitForSecondsRealtime(2);
		if (checkCollision[4])
		{
			if (dir == 0)
			{
				enemyS.currHPBase -= dmg[lvl, armyTypeNum - 1];
			}
			else
			{
				hpS.currHPBase -= dmg[lvl, armyTypeNum - 1];
			}
		}
		canGiveDmgR = true;
	}
	private void OnDrawGizmosSelected()		//vykreslí kruh okolo jednotky
	{
		Gizmos.color = Color.red;
		//Gizmos.DrawWireSphere(distanceFromAllie, 0.1f);
		//Gizmos.DrawWireSphere(transform.position, ranges[1]);
		//Gizmos.DrawWireSphere(transform.position, ranges[0]);
	}
}