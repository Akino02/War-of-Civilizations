using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierE : MonoBehaviour
{
	SoldierP soldierPscript;									//import scriptu protivnika
	[SerializeField] GameObject soldierP;						//import objektu
	BaseScriptP basePscript;									//import objektu
	GameObject item;						//import objektu

	public Rigidbody2D rb;										//funkce pro gravitaci
	public LayerMask[] armyTypes = new LayerMask[3];
	//public LayerMask[] armyTypesE = new LayerMask[3];
	public float[] ranges = { 0.5f, 1.4f, 0.5f };
	public float movespeed;										//rychlost pohybu objektu
	public LayerMask armyType;
	public int armyTypeNum;

	//Ohledne HPbaru
	public GameObject hpBar;

	public float[] maxhp = { 100, 60, 300 };
	public float currhp;
	//public float[] hptype = { 100, 60, 300 };
	private float hpinprocents = 1f;

	//Ohledne utoku
	public int[] dmg = { 40, 60, 40 };
	public bool canGetdmgM = true;      //na blizko
	public bool canGetdmgR = true;      //na dalku
	public bool[] enemies = { false, false, false };
	private bool givemoney = true;								//cooldown na penize

	// Start is called before the first frame update
	void Start()
	{
		soldierPscript = soldierP.GetComponent<SoldierP>();  //import protivnika a jeho promìnných
		//
		GameObject item = GameObject.FindWithTag("baseP");				//toto najde zakladnu hrace pomoci tagu ktery ma
		basePscript = item.GetComponent<BaseScriptP>();
		//
		for (int i = 0; i < 3; i++)
		{
			if (armyType == armyTypes[i])
			{
				//maxhp = hptype[i];
				currhp = maxhp[i];
				armyTypeNum = i;
			}
			//Debug.Log(i);
		}
	}
	// Update is called once per frame
	void Update()
	{
		hpinprocents = ((100 * currhp) / maxhp[armyTypeNum]) / 100;
		rb.velocity = new Vector2((movespeed * -1), rb.velocity.y);   //bude se hybyt do leva zatim je to testovaci
        checkEnemy();
		for (int i = 0; i < 3; i++)
		{
			if(enemies[i] == true)
			{
				if (currhp <= 0 && givemoney == true)
				{
					givemoney = false;
					basePscript.money += soldierPscript.moneykill[armyTypeNum];												//zatim to dava penez tolik kdo ho zabil coz je spatne     potreba to dostat do UI z prefabu
					Debug.Log(basePscript.money);
					Destroy(gameObject);
				}
				else if (currhp > 0)
				{
					if((i == 0 || i == 2) && canGetdmgM == true)
					{
						StartCoroutine(DmgdealcooldownMelee());
					}
					else if(i == 1 && canGetdmgR == true)
					{
						StartCoroutine(DmgdealcooldownRange());
					}
				}
			}
		}
		hpBar.transform.localScale = new Vector2(hpinprocents, hpBar.transform.localScale.y);
	}

	public void checkEnemy()
	{
		for (int i = 0; i < 3; i++)
		{
			enemies[i] = Physics2D.OverlapCircle(transform.position, soldierPscript.ranges[i], soldierPscript.armyTypes[i]) != null;
		}
	}

	IEnumerator DmgdealcooldownMelee()
	{
		canGetdmgM = false;
		if (enemies[0] == true)
		{
			currhp -= soldierPscript.dmg[0];
		}
		else if (enemies[2])
		{
			currhp -= soldierPscript.dmg[2];
		}
		Debug.Log("Enemy " + currhp);
		yield return new WaitForSecondsRealtime(3);
		canGetdmgM = true;
	}
	IEnumerator DmgdealcooldownRange()
	{
		canGetdmgR = false;
		currhp -= soldierPscript.dmg[1];
		Debug.Log("Enemy " + currhp);
		yield return new WaitForSecondsRealtime(2);
		canGetdmgR = true;
	}
}
