using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierP : MonoBehaviour
{
	SoldierE soldierEscript;									//import scriptu protivnika
	[SerializeField] GameObject soldierE;

	public Rigidbody2D rb;										//funkce pro gravitaci
	public LayerMask[] armyTypes = new LayerMask[3];			//to jsou vrstvy spolubojovniku
	//public LayerMask[] armyTypesE = new LayerMask[3];			//to jsou vrstvy nepratel //meli by byt jen v enemyscriptu ale nejde to idk proc
	public float[] ranges = { 0.5f, 1.4f, 0.5f };
	//public float[] rangesE = { 0.5f, 1.4f, 0.5f };
	public float movespeed;										//rychlost pohybu objektu
	public LayerMask armyType;
	public int armyTypeNum;

	//Ohledne HPbaru
	public GameObject hpBar;

	public float[] maxhp = {100, 60 , 300};
	public float currhp;
	private float hpinprocents = 1f;

	//Ohledne utoku
	public int[] dmg = { 40, 60, 40 };
	public bool canGetdmgM = true;								//na blizko
	public bool canGetdmgR = true;								//na dalku

	// Start is called before the first frame update
	void Start()
	{
		soldierEscript = soldierE.GetComponent<SoldierE>();		//import protivnika a jeho promennych
		for (int i = 0; i < 3; i++)								//prirazuje hodnoty podle toho co je to za typ jednotky
		{
			if (armyType == armyTypes[i])
			{
				currhp = maxhp[i];
				armyTypeNum = i;
			}
		}
	}
	// Update is called once per frame
	void Update()
	{
		//																										chyba neche brát vìci od nepøítele************************
		hpinprocents = ((100 * currhp) / maxhp[armyTypeNum]) / 100;															//premena zivotu na procenta
		rb.velocity = new Vector2((movespeed * 1), rb.velocity.y);															//bude se hybyt do leva zatim je to testovaci
		if (Physics2D.OverlapCircle(transform.position, soldierEscript.ranges[0], soldierEscript.armyTypes[0]) != null || Physics2D.OverlapCircle(transform.position, soldierEscript.ranges[2], soldierEscript.armyTypes[2]) != null)  //je tam if, aby to poznaval hned
		{
			if (currhp <= 0)									//pokud objekt nema zivoty tak zemre
			{
				Destroy(gameObject);
			}
			else if (currhp > 0 && canGetdmgM == true)			//pokud objekt ma zivoty a muze dostat dmg tak se spusti IEnumerator
			{
				StartCoroutine(DmgdealcooldownMelee());
			}
		}
		if (Physics2D.OverlapCircle(transform.position, soldierEscript.ranges[1], soldierEscript.armyTypes[1]) != null)		//je tam if, aby to poznaval hned
		{
			if (currhp <= 0)									//pokud objekt nema zivoty tak zemre
			{
				Destroy(gameObject);
			}
			else if (currhp > 0 && canGetdmgR == true)          ////pokud objekt ma zivoty a muze dostat dmg tak se spusti IEnumerator
			{
				StartCoroutine(DmgdealcooldownRange());
			}
		}
		hpBar.transform.localScale = new Vector2(hpinprocents, hpBar.transform.localScale.y);								//zapisovani do hpbaru
	}

	IEnumerator DmgdealcooldownMelee()							//zde dostava dmg od jednotek, ktere jsou na blizko
	{
		canGetdmgM = false;
		if (Physics2D.OverlapCircle(transform.position, soldierEscript.ranges[0], soldierEscript.armyTypes[0]) != null)		//pokud je to soldier
		{
			currhp -= soldierEscript.dmg[0];
		}
		else if (Physics2D.OverlapCircle(transform.position, soldierEscript.ranges[2], soldierEscript.armyTypes[2]) != null)//pokud je to tank
		{
			currhp -= soldierEscript.dmg[2];
		}
		Debug.Log("Player " + currhp + " Melee");
		yield return new WaitForSeconds(3);
		canGetdmgM = true;
	}
	IEnumerator DmgdealcooldownRange()							//zde dostava dmg od jednotek, ktere jsou na dalku (ranger)
	{
		canGetdmgR = false;
		currhp -= soldierEscript.dmg[1];
		Debug.Log("Player " + currhp + "Range");
		yield return new WaitForSecondsRealtime(2);
		canGetdmgR = true;
	}

	/*private void OnDrawGizmosSelected()						//vykreslí kruh okolo jednotky
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, rangeS);
	}*/
}