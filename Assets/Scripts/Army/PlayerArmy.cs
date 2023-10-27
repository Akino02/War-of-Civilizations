using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerArmy : MonoBehaviour
{
    //																						TENTO SCRIPT BYL UZAVREN Z DUVODU NEFUNGOVANI(MOZNA BUDE OPRAVEN)[CHYBA V TOM ZE NEMUZE NAJIT NOVE OBJEKTY KTERYM DA SCRIPT]

	EnemyArmy SoldierEscript;			//import scriptu protivnika
	//BaseScriptP BaseScript;				//importuje script protivnikovy zakladny
	[SerializeField] GameObject enemyBase;

	public Rigidbody2D rb;              //funkce pro gravitaci
	public LayerMask opponent;          //layer nepratelskych jednotek typu soldier
	public LayerMask opponentBase;          //layer nepratelske zakladny
	public float range;                 //velikost kde muze bojovat
	public float movespeed;             //rychlost pohybu objektu
	public LayerMask armyType;          //typ jednotky

	//vsechny typy jednotek
	public LayerMask Soldier;
	public LayerMask Ranger;
	public LayerMask Tank;
	public int armyTypeNum = 0;			//toto definuje jaky je to typ vojaka

	//Ohledne HPbaru
	public GameObject hpBar;

	private float maxhp = 100;
	public float currhp = 100;
	private int[] hptypes = { 100, 60, 300 };       //Typy zivotu pro jednotky (soldier, ranger, tank)
	private float hpinprocents = 1f;

	public int made = 0;


	//Ohledne utoku
	private int[] dmg = { 40, 60, 40 };             //sila pro postavy (soldier, ranger, tank)

	/*public float dmgR = 60;             //Range Ranger sila postavy
	public float dmgT = 40;             //Melee Tank sila postavy*/
	public bool canGiveDmgM = false;     //Muze bojovat melee
	public bool canGiveDmgR = false;     //Muze bojovat na dalku
	// Start is called before the first frame update
	void Start()
	{
        if (armyType == Soldier)
        {
            armyTypeNum = 1;
            maxhp = hptypes[armyTypeNum - 1];
            currhp = maxhp;
            canGiveDmgM = true;
        }
        else if (armyType == Ranger)
        {
            armyTypeNum = 2;
            maxhp = hptypes[armyTypeNum - 1];
            currhp = maxhp;
            canGiveDmgR = true;
        }
        if (armyType == Tank)
        {
            armyTypeNum = 3;
            maxhp = hptypes[armyTypeNum - 1];
            currhp = maxhp;
            canGiveDmgM = true;
        }
    }
	// Update is called once per frame
	void Update()
	{
        hpinprocents = ((100 * currhp) / maxhp) / 100;
		rb.velocity = new Vector2((movespeed * 1), rb.velocity.y);      //bude se hybyt do leva zatim je to testovaci
		if (Physics2D.OverlapCircle(transform.position, range, opponent) != null && canGiveDmgM == true || canGiveDmgR == true)     //je tam if, aby to poznaval hned
		{
			if (armyTypeNum == 1 || armyTypeNum == 3)
			{
				GameObject[] allEnemies = FindObjectsOfType<GameObject>();		//najde vsechny objekty

				foreach (GameObject obj in allEnemies)							//vypise vsechny objekty
				{
					if (obj.layer == 13)			//poradi vrstvy 
					{
						SoldierEscript = obj.GetComponent<EnemyArmy>();					//Toto najde dalsiho nepritele, ktery splnuje pozadavky

					}
				}
				StartCoroutine(DmgdealcooldownMelee());
				Debug.Log("Can give dmg Enemy");
			}
			/*else if (armyTypeNum == 2)
			{
				StartCoroutine(DmgdealcooldownRange());
			}*/
		}

		/*if (Physics2D.OverlapCircle(transform.position, rangeR, opponentRanger) != null)		//je tam if, aby to poznaval hned
		{
			if (currhp <= 0)
			{
				Destroy(gameObject);
			}
			else if (currhp > 0 && canGetdmgR == true)
			{
				StartCoroutine(DmgdealcooldownRange());
			}
		}*/
		if (currhp <= 0)
		{
			Destroy(gameObject);
		}
		hpBar.transform.localScale = new Vector2(hpinprocents, hpBar.transform.localScale.y);
	}

	IEnumerator DmgdealcooldownMelee()
	{
		canGiveDmgM = false;
		if (armyTypeNum == 1)
		{
			/*if(opponent != null)
			{*/
				SoldierEscript.currhp -= dmg[armyTypeNum - 1];
			//}
			/*else if (opponentBase != null)
			{
				BaseScriptE.currHPBase -= dmg[armyTypeNum - 1];
			}*/
		}
		else if (armyTypeNum == 2)
		{
			SoldierEscript.currhp -= dmg[armyTypeNum - 1];
			//BaseScriptE.currHPBase -= dmg[armyTypeNum - 1];
		}
		/*Debug.Log("Player " + SoldierEscript.currhp);
		Debug.Log("Player " + BaseScriptE.currHPBase);*/
		yield return new WaitForSeconds(3);
		canGiveDmgM = true;
	}
	/*IEnumerator DmgdealcooldownRange()
	{
		canGiveDmgR = false;
		currhp -= soldierPscript.dmgR;
		Debug.Log("Player " + currhp);
		yield return new WaitForSecondsRealtime(2);
		canGiveDmgR = true;
	}*/

	/*private void OnDrawGizmosSelected()		//vykreslí kruh okolo jednotky
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, rangeS);
	}*/
}