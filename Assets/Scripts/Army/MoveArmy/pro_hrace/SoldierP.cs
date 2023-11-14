using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierP : MonoBehaviour
{
	SoldierE soldierEscript;									//import scriptu protivnika
	[SerializeField] GameObject soldierE;
	BaseScriptP basePscript;                                    //import script 
	GameObject item;                                            //import objektu

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

	public float[,] maxhp = { { 100, 60, 300 },{150,90,450 },{225,135,675},{ 350,200,1000},{400,300,1500 } };               //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
    public float currhp;
	private float hpinprocents = 1f;
	public int level = 0;                                                                                                   //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******

    //Ohledne utoku
    public int[,] dmg = { {40, 60, 30 },{ 60, 90, 50}, { 90, 135, 70 }, { 135, 90, 115 }, { 150, 200, 120 } };              //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
    public bool canGetdmgM = true;								//na blizko
	public bool canGetdmgR = true;                              //na dalku
	public bool[] enemies = { false, false, false };			//

	public int[,] moneykill = { { 30, 50, 150 }, {60,100,300}, {120,200,600}, {240,400,1200}, {480,800,2400} };             //peniza za zabiti nepritele (soldier, ranger, tank)//potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
    public int[] expperkill = { 100, 125, 300 };                //peniza za zabiti nepritele (soldier, ranger, tank)		//potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******

    // Start is called before the first frame update
    void Start()
	{

		soldierEscript = soldierE.GetComponent<SoldierE>();     //import protivnika a jeho promennych
		//
		GameObject item = GameObject.FindWithTag("baseP");              //toto najde zakladnu hrace pomoci tagu ktery ma
		basePscript = item.GetComponent<BaseScriptP>();
		//
		level = basePscript.level;                              //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
        for (int i = 0; i < 3; i++)								//prirazuje hodnoty podle toho co je to za typ jednotky
		{
			if (armyType == armyTypes[i])
			{
				currhp = maxhp[level,i];                        //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
                armyTypeNum = i;
			}
		}
	}
	// Update is called once per frame
	void Update()
	{
		checkEnemy();
		//														chyba nechce brat veci od nepritele****** uz to funguje ale to je warning tady jen kdyby
		hpinprocents = ((100 * currhp) / maxhp[level, armyTypeNum]) / 100;                                                          //premena zivotu na procenta		//potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
        rb.velocity = new Vector2((movespeed * 1), rb.velocity.y);                                                          //bude se hybyt do leva zatim je to testovaci
		for (int i = 0; i < 3; i++)
		{
			if (enemies[i] == true)
			{
				if (currhp <= 0)
				{
					Destroy(gameObject);
				}
				else if (currhp > 0)
				{
					if ((i == 0 || i == 2) && canGetdmgM == true)
					{
						StartCoroutine(DmgdealcooldownMelee());
					}
					else if (i == 1 && canGetdmgR == true)
					{
						StartCoroutine(DmgdealcooldownRange());
					}
				}
			}
		}
		hpBar.transform.localScale = new Vector2(hpinprocents, hpBar.transform.localScale.y);								//zapisovani do hpbaru
	}
	public void checkEnemy()
	{
		for (int i = 0; i < 3; i++)
		{
			enemies[i] = Physics2D.OverlapCircle(transform.position, soldierEscript.ranges[i], soldierEscript.armyTypes[i]) != null;
		}
	}

	IEnumerator DmgdealcooldownMelee()							//zde dostava dmg od jednotek, ktere jsou na blizko
	{
		canGetdmgM = false;
		if (enemies[0])		//pokud je to soldier
		{
			currhp -= soldierEscript.dmg[soldierEscript.level,0];                                                           //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
        }
		else if (enemies[2])//pokud je to tank
		{
			currhp -= soldierEscript.dmg[soldierEscript.level, 2];                                                          //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
        }
		Debug.Log("Player " + currhp + " Melee");
		yield return new WaitForSeconds(3);
		canGetdmgM = true;
	}
	IEnumerator DmgdealcooldownRange()							//zde dostava dmg od jednotek, ktere jsou na dalku (ranger)
	{
		canGetdmgR = false;
		currhp -= soldierEscript.dmg[soldierEscript.level, 1];                                                              //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
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