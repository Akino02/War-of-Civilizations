using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;								//importovani random

public class UniArmy : MonoBehaviour
{
	//importovane scripty
	ProgresScript progresS;                                     //importuje script zakladny v levo(hrace)
	HpScript hpS;                                               //importuje script zakladny v levo(hrace)
	EnemySpawn enemyS;                                          //importuje script zakladny v pravo(enemy)

	UniArmy armyScriptE;                                        //import scriptu protivnika
	UniArmy armyScriptP;                                        //import scriptu spojence

	private UniArmy SoldierArmyScript;                          //importovani scriptu, ktery bude slouzit pro vojacka, aby si nasel nepritele

	//animace
	public Animator animator;                                   //pro import animatoru

    //sound
    public AudioSource attackSound;
	public static float sfxSound = 0.35f;

    //
    public LayerMask opponent;                                  //layer nepratelskych jednotek typu soldier
	public LayerMask opponentBase;                              //layer nepratelske zakladny
	public float[] ranges = { 0.6f, 0.6f, 0.1f };                //velikost kde muze bojovat				//puvodne to bylo { 0.6f, 1.7f, 0.1f}
	public LayerMask armyType;                                  //typ jednotky

	//zaklad pro pohyb a gravitaci
	public Rigidbody2D rb;                                      //funkce pro gravitaci
	public float movespeed;                                     //rychlost pohybu objektu

	//vsechny typy jednotek
	public LayerMask soldier;                                   //Layer pro vojacka typu SOLDIER
	public LayerMask ranger;                                    //Layer pro vojacka typu RANGER
	public LayerMask tank;                                      //Layer pro vojacka typu TANK
	public LayerMask allies;                                    //Layer pratelskych vojacku
	public int armyTypeNum = 0;                                 //toto definuje jaky je to typ vojaka

	//Ohledne HPbaru
	public GameObject hpBar;

	public float[,] maxhp = { { 100, 60, 300 }, { 200, 160, 400 }, { 300, 260, 500 }, { 400, 360, 600 }, { 500, 460, 700 } };
	public float currhp = 100;
	private float hpinprocents = 1f;
	public int lvl = 0;                                         //uchovani urovne vojacka


	private int[] moveDir = { 1, -1, 0 };
	public int dir;

	public bool[] checkCollision = { false, false, false, false, false, false };    //zda soldier vidi, zda ranger vidi, zda spojenci se vidi, zda vidi zakladnu(melee), zda vidi zakladnu(ranger), pojistka
	public bool isBase = false;
	public Vector3 distanceFromAllie;


	//Ohledne utoku
	private int[,] dmgMin = { { 20, 30, 15 }, { 40, 60, 30 }, { 60, 90, 45 }, { 80, 120, 60 }, { 100, 150, 75 } };             //sila pro vojacky (soldier, ranger, tank)
	private int[,] dmgMax = { { 40, 60, 30 }, { 80, 120, 60 }, { 120, 180, 90 }, { 160, 240, 120 }, { 200, 300, 150 } };             //sila pro vojacky (soldier, ranger, tank)
	//private int[] betterDmg = { 150, 300, 120 };

	public bool canGiveDmgM = false;                            //Muze bojovat melee
	public bool canGiveDmgR = false;                            //Muze bojovat na dalku

	//Odmeny za to ze zemre
	public int[,] moneykill = { { 20, 35, 135 }, { 40, 65, 265 }, { 80, 135, 535 }, { 160, 265, 1065 }, { 320, 535, 2135 } };             //peniza za zabiti nepritele (soldier, ranger, tank)
	public int[] expperkill = { 100, 125, 300 };                //zkusenosti za zabiti nepritele (soldier, ranger, tank)
																//

	//TEST
	//public GameObject hitbox;

	// Start is called before the first frame update
	void Start()
	{
        //Sound
        attackSound.volume = ButtonsMenu.volumeSFX;
        sfxSound = attackSound.volume;


        GameObject script1 = GameObject.FindWithTag("baseP");   //toto najde zakladnu hrace pomoci tagu ktery ma
		progresS = script1.GetComponent<ProgresScript>();
		hpS = script1.GetComponent<HpScript>();
		//
		GameObject script2 = GameObject.FindWithTag("baseE");   //toto najde zakladnu nepritele pomoci tagu ktery ma
		enemyS = script2.GetComponent<EnemySpawn>();

		if (dir == 0)
		{
			lvl = progresS.level;                               //zde se urci jaky level bude mit pro hrace
		}
		else
		{
			lvl = enemyS.level;                                 //zde se urci jaky level bude mit pro nepritele
			transform.Rotate(0f, 180f, 0f);
		}

		if (armyType == soldier)                                //nastaveni co je to za druh vojacka
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
		/*if (dir == 0)											//zvyseni dmg pro hrace kdyz dosahne posledni urovne
		{
			for (int i = 0; i < 3; i++)
			{
				dmgMin[4, i] = betterDmg[i];
                dmgMax[4, i] = betterDmg[i];
            }
        }*/
	}
	// Update is called once per frame
	void Update()
	{
		CheckForEnemy();    //tato funkce zajistuje kontrolovani kolizi v okoli
		hpinprocents = ((100 * currhp) / maxhp[lvl, armyTypeNum - 1]) / 100;
		Move();      //tato funkce zajistuje pohyb
					 //RangerRange();
					 //DamageBase();											//ubirani zivotu primo zakladne
		if (checkCollision[0] && canGiveDmgM == true && armyTypeNum != 2)     //je tam if, aby to poznaval hned
		{
			FindMyEnemy();
            //StartCoroutine(DmgdealcooldownMelee());
            isBase = false;
        }
		else if (checkCollision[1] && canGiveDmgR == true && armyTypeNum == 2)      //je tam if, aby to poznaval hned
		{
			FindMyEnemy();
            //StartCoroutine(DmgdealcooldownRange());
            isBase = false;
        }
		else if (checkCollision[3] && canGiveDmgM == true && armyTypeNum != 2)
		{
			//StartCoroutine(DmgDealCoolDownMeleeBase());
			StartCoroutine(DmgdealcooldownMelee());
            checkCollision[5] = true;
            animator.SetBool("ScriptFound", true);
			isBase = true;
        }
		else if (checkCollision[4] && canGiveDmgR == true && armyTypeNum == 2)
		{
			//StartCoroutine(DmgDealCoolDownRangerBase());
			StartCoroutine(DmgdealcooldownRange());
            checkCollision[5] = true;
            animator.SetBool("ScriptFound", true);
			isBase = true;

        }
		if (currhp <= 0)
		{
			if (dir != 0)                                       //tato podminka bude davat penize a zkusenosti pri tom kdyz zemre enemy
			{
				Reward();
			}
			Destroy(gameObject);
		}
		else if (transform.position.y < -20)					//pokud tam bude hodne nepratel od nepritele ci hrace tak by mohli padat dolu a pokud ano tak zemrou
		{
            Destroy(gameObject);
        }
		hpBar.transform.localScale = new Vector2(hpinprocents, hpBar.transform.localScale.y);
	}
	void FindMyEnemy()											//tato funkce hleda nepritele a k nemu priradi script a jeste se zavola dalsi funkce na utok
	{
		checkCollision[5] = false;
		animator.SetBool("ScriptFound", false);
		armyScriptP = null;
		armyScriptE = null;

        //pokud true tak hleda tag s player, pokud false tak hleda tag s enemy
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
			checkCollision[5] = true;
			animator.SetBool("ScriptFound", true);
		}
		//uderi jakmile najde nepritele
		if (armyTypeNum == 1 || armyTypeNum == 3 && canGiveDmgM)
		{
			StartCoroutine(DmgdealcooldownMelee());
		}
		else if (armyTypeNum == 2 && canGiveDmgR)
		{
			StartCoroutine(DmgdealcooldownRange());
		}
		//Debug.Log("Can give ");
	}
	void CheckForEnemy()
	{
		//float notFullsize = 0.30f;                              //slouzi pro mensi odstup od objektu, aby kontroloval odstup mezi spojenci
		float borderObject = GetComponent<SpriteRenderer>().bounds.size.x / 2;
		distanceFromAllie = new Vector3(transform.position.x + (ranges[2] * 1.25f) * moveDir[dir] + borderObject * moveDir[dir], transform.position.y, transform.position.y);
		checkCollision[0] = Physics2D.OverlapCircle(transform.position, ranges[0], opponent);           //zda vidi enemy tak stoji (urceno pro melee class)
		checkCollision[3] = Physics2D.OverlapCircle(transform.position, ranges[0], opponentBase);           //zda vidi nepratelskou zakladnu (urceno pro melee class(range))
		if (armyTypeNum == 2)           //pokud je to Ranger tak ten se zastavi dale, protoze muze zabijet z dalky
		{
			checkCollision[1] = Physics2D.OverlapCircle(transform.position, ranges[1], opponent);           //zda vidi enemy tak stoji (urceno pro ranger class)
			checkCollision[4] = Physics2D.OverlapCircle(transform.position, ranges[1], opponentBase);           //zda vidi nepratelskou zakladnu (urceno pro ranger class(range))
		}
		checkCollision[2] = Physics2D.OverlapCircle(distanceFromAllie, 0.09f, allies);           //zda vidi spojence tak se zastavi (je urceno pro vsechny)
																								 //if (armyTypeNum == 2)
																								 //{
																								 //    RangerRange();
																								 //}
																								 //asfdlkjasfdjklasfdklj
	}
	void Move()
	{                               //problem RESIT  && checkCollision[5]
		if (checkCollision[0] || checkCollision[1] || checkCollision[2] || checkCollision[3] || checkCollision[4])            //pokud vojacek narazi na jakoukoliv kolizi tak se zastavi (base, armyP, armyE)
		{
			rb.velocity = new Vector2((movespeed * moveDir[2]), rb.velocity.y);     //nebude se hybat pokud je poblic kolize
			animator.SetFloat("Speed", 0);                                          //dosazeni za promennou speed, ktera urcuje animace
		}
		else                                                                        //pokud nebude zadna kolize tak bude chodit
		{
			rb.velocity = new Vector2((movespeed * moveDir[dir]), rb.velocity.y);
			animator.SetFloat("Speed", rb.velocity.x * moveDir[dir]);               //dosazeni za promennou speed, ktera urcuje animace
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
		progresS.money += moneykill[lvl, armyTypeNum - 1];
		progresS.experience += expperkill[armyTypeNum - 1];
		//Debug.Log(moneykill[lvl, armyTypeNum - 1]);
		//Debug.Log(expperkill[armyTypeNum - 1]);
	}
	IEnumerator DmgdealcooldownMelee()                          //tato funkce slouzi pro utok pro soldier a tank
	{
		canGiveDmgM = false;
        yield return new WaitForSecondsRealtime(2);
        int randomDmg = Random.Range(dmgMin[lvl, armyTypeNum - 1], dmgMax[lvl, armyTypeNum - 1]);                           //nahodna hodnota pro utok na jednotky
		if (checkCollision[0] && checkCollision[5] && !isBase && !LogScript.isGameOver)
		{
			if (armyTypeNum == 1)
			{
				if (dir == 0)
				{
					armyScriptE.currhp -= randomDmg;
				}
				else
				{
					armyScriptP.currhp -= randomDmg;
				}
                attackSound.Play();
            }
			else if (armyTypeNum == 3)
			{
				if (dir == 0)
				{
					armyScriptE.currhp -= randomDmg;
				}
				else
				{
					armyScriptP.currhp -= randomDmg;
				}
                attackSound.Play();
            }
		}
		else if (checkCollision[3] && !LogScript.isGameOver)
		{
            if (armyTypeNum == 1)
			{
				if (dir == 0)
				{
					enemyS.currHPBase -= randomDmg;
				}
				else
				{
					hpS.currHPBase -= randomDmg;
				}
                attackSound.Play();
            }
			else if (armyTypeNum == 3)
			{
				if (dir == 0)
				{
					enemyS.currHPBase -= randomDmg;
				}
				else
				{
					hpS.currHPBase -= randomDmg;
				}
                attackSound.Play();
            }
		}
		checkCollision[5] = false;                              //mozna chyba*************************
		animator.SetBool("ScriptFound", false);
		//Debug.Log("Bum kdyz nic");
		canGiveDmgM = true;
	}
	IEnumerator DmgdealcooldownRange()                          //tato funkce slouzi pro utok pro ranger
	{
		canGiveDmgR = false;
		yield return new WaitForSecondsRealtime(2);
		int randomDmg = Random.Range(dmgMin[lvl, armyTypeNum - 1], dmgMax[lvl, armyTypeNum - 1]);                           //nahodna hodnota pro utok na jednotky
		if (checkCollision[1] && checkCollision[5] && !isBase && !LogScript.isGameOver)                 //problem RESIT  && checkCollision[5]
		{
			if (dir == 0)
			{
				armyScriptE.currhp -= randomDmg;
			}
			else
			{
				armyScriptP.currhp -= randomDmg;
			}
            attackSound.Play();
            //Debug.Log("Strel");
		}
		else if (checkCollision[4] && !LogScript.isGameOver)            //problem RESIT  && checkCollision[5]
		{
            checkCollision[5] = true;
            animator.SetBool("ScriptFound", true);
            if (dir == 0)
			{
				enemyS.currHPBase -= randomDmg;
			}
			else
			{
				hpS.currHPBase -= randomDmg;
			}
            attackSound.Play();
            //Debug.Log("Strel");
		}
		checkCollision[5] = false;								//mozna chyba*************************
		animator.SetBool("ScriptFound", false);
		canGiveDmgR = true;
	}
	/*IEnumerator DmgDealCoolDownMeleeBase()						//potencionalni problem************************
	{
		canGiveDmgM = false;
		yield return new WaitForSeconds(2);
		int randomDmg = Random.Range(dmgMin[lvl, armyTypeNum - 1], dmgMax[lvl, armyTypeNum - 1]);                           //nahodna hodnota pro utok na zakladu
		if (checkCollision[3])
		{
			if (armyTypeNum == 1)
			{
				if (dir == 0)
				{
					enemyS.currHPBase -= randomDmg;
				}
				else
				{
					hpS.currHPBase -= randomDmg;
				}
			}
			else if (armyTypeNum == 3)
			{
				if (dir == 0)
				{
					enemyS.currHPBase -= randomDmg;
				}
				else
				{
					hpS.currHPBase -= randomDmg;
				}
			}
		}
		canGiveDmgM = true;
	}*/

	/*IEnumerator DmgDealCoolDownRangerBase()                     //potencionalni problem************************
	{
		canGiveDmgR = false;
		yield return new WaitForSecondsRealtime(2);
		int randomDmg = Random.Range(dmgMin[lvl, armyTypeNum - 1], dmgMax[lvl, armyTypeNum - 1]);							//nahodna hodnota pro utok na zakladu
		if (checkCollision[4])
		{
			if (dir == 0)
			{
				enemyS.currHPBase -= randomDmg;
			}
			else
			{
				hpS.currHPBase -= randomDmg;
			}
		}
		canGiveDmgR = true;
	}*/
	private void OnDrawGizmosSelected()     //vykreslí kruh okolo jednotky
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(distanceFromAllie, 0.09f);
		Gizmos.DrawWireSphere(transform.position, ranges[1]);
		Gizmos.DrawWireSphere(transform.position, ranges[0]);
	}
	/*public void OnTriggerEnter2D(Collider2D hitbox)				//detekce nepritele											PROBLEM tady resit
	{
		if (hitbox.gameObject.CompareTag("Enemy") && dir == 0)
		{

			var SoldierArmyScript = hitbox.GetComponent<UniArmy>();
			armyScriptE = SoldierArmyScript;                //dosazeni scriptu za objekt
		}
		else if (hitbox.gameObject.CompareTag("Player") && dir == 0)							
		{
			checkCollision[2] = true;
			if (armyTypeNum == 2 && checkCollision[2] == true)
			{
				if (hitbox.gameObject.CompareTag("Enemy") && dir == 0)
				{
					checkCollision[1] = true;
					Debug.Log("To je Enemy");

					var SoldierArmyScript = hitbox.GetComponent<UniArmy>();
					armyScriptE = SoldierArmyScript;                //dosazeni scriptu za objekt
				}
			}
		}
		else if (hitbox.gameObject.CompareTag("Player") && dir == 1)
		{
			//Debug.Log("To je player");

			var SoldierArmyScript = hitbox.GetComponent<UniArmy>();
			armyScriptP = SoldierArmyScript;                //dosazeni scriptu za objekt
		}
		else
		{
			checkCollision[0] = false;						//kolize nepritel s melee
			checkCollision[1] = false;						//kolize nepritel s ranger(ranger pres spojence)
			//checkCollision[5] = false;				
			checkCollision[2] = false;						//kolize mezi spojenci
		}
	}*/
	/*void RangerRange()											//funkce pro rangera, pokud je prednim spojenec tak se podiva dale a muze strilet pres spojence
	{
		if (checkCollision[2] && armyTypeNum == 2)
		{
			hitbox.transform.localScale = new Vector2(3f, hitbox.transform.localScale.y);
			//hitbox.transform.position = new Vector2(1f, hitbox.transform.position.y);
			//ranges[1] = 1.7f;
		}
		else if (!checkCollision[2] && armyTypeNum == 2)
		{
			hitbox.transform.localScale = new Vector2(0.7f, hitbox.transform.localScale.y);
			//hitbox.transform.position = new Vector2(0.9f, hitbox.transform.position.y);
			//ranges[1] = 1.65f;
		}
	}*/
}