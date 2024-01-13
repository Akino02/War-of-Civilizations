using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UniArmy : MonoBehaviour
{
	ProgresScript progresS;            //importuje script zakladny v levo(hrace)
	EnemySpawn enemyS;				//importuje script zakladny v pravo(enemy)

	UniArmy SoldierEscript;         //import scriptu protivnika
	UniArmy SoldierPscript;         //import scriptu protivnika

	//opraveni scriptu
	private UniArmy SoldierArmyScript;

	//BaseScriptP BaseScript;				//importuje script protivnikovy zakladny
	//[SerializeField] GameObject enemyBase;						//nevyuzite

	public Rigidbody2D rb;              //funkce pro gravitaci
	public LayerMask opponent;          //layer nepratelskych jednotek typu soldier
	public LayerMask opponentBase;          //layer nepratelske zakladny
	public float[] ranges = { 0.5f, 1.4f};                 //velikost kde muze bojovat
	public float movespeed;             //rychlost pohybu objektu
	public LayerMask armyType;          //typ jednotky

	//vsechny typy jednotek
	public LayerMask soldier;
	public LayerMask ranger;
	public LayerMask tank;
	public LayerMask allies;
	public int armyTypeNum = 0;         //toto definuje jaky je to typ vojaka

	//Ohledne HPbaru
	public GameObject hpBar;

	private float maxhp = 100;
	public float currhp = 100;
	private int[,] hptypes = { { 100, 60, 300 }, { 150, 90, 450 }, { 225, 135, 675 }, { 350, 200, 1000 }, { 400, 300, 1500 } };       //Typy zivotu pro jednotky (soldier, ranger, tank)
	private float hpinprocents = 1f;
	public int lvl = 0;

	public int made = 0;

	private int[] moveDir = { 1, -1, 0};
	public int dir;

	public bool[] checkCollision = { false, false , false, false, false};
	public Vector3 distanceFromAllie;


    //Ohledne utoku
    private int[] dmg = { 40, 60, 40 };             //sila pro postavy (soldier, ranger, tank)

	/*public float dmgR = 60;             //Range Ranger sila postavy
	public float dmgT = 40;             //Melee Tank sila postavy*/
	public bool canGiveDmgM = false;     //Muze bojovat melee
	public bool canGiveDmgR = false;     //Muze bojovat na dalku

	//Odmeny za to ze zemre
    public int[,] moneykill = { { 30, 50, 150 }, { 60, 100, 300 }, { 120, 200, 600 }, { 240, 400, 1200 }, { 480, 800, 2400 } };             //peniza za zabiti nepritele (soldier, ranger, tank)
    public int[] expperkill = { 100, 125, 300 };																							//zkusenosti za zabiti nepritele (soldier, ranger, tank)
	//

    // Start is called before the first frame update
    void Start()
	{
        GameObject script1 = GameObject.FindWithTag("baseP");       //toto najde zakladnu hrace pomoci tagu ktery ma
        progresS = script1.GetComponent<ProgresScript>();
        //
        GameObject script2 = GameObject.FindWithTag("baseE");      //toto najde zakladnu nepritele pomoci tagu ktery ma
        enemyS = script2.GetComponent<EnemySpawn>();

        if (armyType == soldier)
		{
			armyTypeNum = 1;
			maxhp = hptypes[lvl,armyTypeNum - 1];
			currhp = maxhp;
			canGiveDmgM = true;
		}
		else if (armyType == ranger)
		{
			armyTypeNum = 2;
			maxhp = hptypes[lvl,armyTypeNum - 1];
			currhp = maxhp;
			canGiveDmgR = true;
		}
		else if (armyType == tank)
		{
			armyTypeNum = 3;
			maxhp = hptypes[lvl,armyTypeNum - 1];
			currhp = maxhp;
			canGiveDmgM = true;
		}
	}
	// Update is called once per frame
	void Update()
	{
		CheckForEnemy();	//tato funkce zajistuje kontrolovani kolizi v okoli
		hpinprocents = ((100 * currhp) / maxhp) / 100;
		Move();      //tato funkce zajistuje pohyb
		if (checkCollision[0] && canGiveDmgM == true && armyTypeNum != 2)     //je tam if, aby to poznaval hned
		{
			FindMyEnemy();
		}
		else if (checkCollision[1] && canGiveDmgR == true && armyTypeNum == 2)		//je tam if, aby to poznaval hned
		{
			FindMyEnemy();
		}
		if (currhp <= 0)
		{
			if(dir != 0)		//tato podminka bude davat penize a zkusenosti pri tom kdyz zemre enemy
			{
				Reward();
            }
			Destroy(gameObject);
		}
		hpBar.transform.localScale = new Vector2(hpinprocents, hpBar.transform.localScale.y);
	}
	void FindMyEnemy()						//tato funkce hleda nepritele a k nemu priradi script a jeste se zavola dalsi funkce na utok
	{
		SoldierPscript = null;
		SoldierEscript = null;

		GameObject[] allEnemies = (dir == 1) ? GameObject.FindGameObjectsWithTag("Player") : GameObject.FindGameObjectsWithTag("Enemy");

		foreach (GameObject obj in allEnemies.Reverse())                //tady se otoci porazi aby bral toho prvniho enemy vzdy
		{
			SoldierArmyScript = obj.GetComponent<UniArmy>();
			if (obj.layer == 10 && dir == 1)
			{
				SoldierPscript = SoldierArmyScript;                     //dosazeni scriptu za objekt
				//Debug.Log("Allie attacked");
			}
			else if (obj.layer == 13 && dir == 0)
			{
				SoldierEscript = SoldierArmyScript;                     //dosazeni scriptu za objekt
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
		float notFullsize = 0.50f;
        distanceFromAllie = new Vector3(transform.position.x + notFullsize, transform.position.y, transform.position.y);
        checkCollision[0] = Physics2D.OverlapCircle(transform.position, ranges[0], opponent);           //zda vidi enemy tak stoji (urceno pro melee class)
		if(armyTypeNum == 2)			//pokud je to Ranger tak ten se zastavi dale, protoze muze zabijet z dalky
		{
            checkCollision[1] = Physics2D.OverlapCircle(transform.position, ranges[1], opponent);           //zda vidi enemy tak stoji (urceno pro ranger class)
        }
		checkCollision[2] = Physics2D.OverlapCircle(distanceFromAllie, 0.1f, allies);           //zda vidi spojence tak se zastavi (je urceno pro vsechny)
	}
	void Move()
	{
		if (checkCollision[0] || checkCollision[1] || checkCollision[2])            //pokud vojacek narazi na jakoukoliv kolizi tak se zastavi
		{
			rb.velocity = new Vector2((movespeed * moveDir[2]), rb.velocity.y);      //bude se hybat podle toho zda je to enemy ci player
		}
		else																		//pokud nebude zadna kolize tak bude chodit
		{
			rb.velocity = new Vector2((movespeed * moveDir[dir]), rb.velocity.y);
		}
	}
	void Reward()
	{
		progresS.money += moneykill[lvl, armyTypeNum-1];
		progresS.experience += expperkill[armyTypeNum-1];
		Debug.Log(moneykill[lvl, armyTypeNum - 1]);
		Debug.Log(expperkill[armyTypeNum - 1]);
	}

	IEnumerator DmgdealcooldownMelee()				//tato funkce slouzi pro utok pro soldier a tank
	{
		canGiveDmgM = false;
		if (armyTypeNum == 1)
		{
			/*if(opponent != null)
			{*/
			if(dir == 0)
			{
				SoldierEscript.currhp -= dmg[armyTypeNum - 1];
			}
			else
			{
				SoldierPscript.currhp -= dmg[armyTypeNum - 1];
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
				SoldierEscript.currhp -= dmg[armyTypeNum - 1];
			}
			else
			{
				SoldierPscript.currhp -= dmg[armyTypeNum - 1];
			}
			//SoldierPscript.currhp -= dmg[armyTypeNum - 1];
			//BaseScriptE.currHPBase -= dmg[armyTypeNum - 1];
		}
		/*Debug.Log("Player " + SoldierEscript.currhp);
		Debug.Log("Player " + BaseScriptE.currHPBase);*/
		yield return new WaitForSeconds(3);
		canGiveDmgM = true;
	}
	IEnumerator DmgdealcooldownRange()				//tato funkce slouzi pro utok pro ranger
	{
		canGiveDmgR = false;
		if (dir == 0)
		{
			SoldierEscript.currhp -= dmg[armyTypeNum - 1];
		}
		else
		{
			SoldierPscript.currhp -= dmg[armyTypeNum - 1];
		}
		yield return new WaitForSecondsRealtime(2);
		canGiveDmgR = true;
	}

	private void OnDrawGizmosSelected()		//vykreslí kruh okolo jednotky
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(distanceFromAllie, 0.1f);
	}
}