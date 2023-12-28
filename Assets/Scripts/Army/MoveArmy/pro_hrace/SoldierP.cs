using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoldierP : MonoBehaviour
{
	SoldierE soldierEscript;									//import scriptu protivnika
	[SerializeField] GameObject soldierE;
	ProgresScript progresS;                                     //import script, pro urcovani okolnich veci
	EnemySpawn enemyS;

	public Rigidbody2D rb;										//funkce pro gravitaci

	public LayerMask[] armyTypes = new LayerMask[3];			//to jsou vrstvy spolubojovniku		//mohu si pak upravit
	//public LayerMask[] armyTypes = {10,11,12};				//to jsou vrstvy spolubojovniku		//zde jsou nadefinované jednotky, coz spatne funguje (ma se tam napsat cislo layeru)

	//public LayerMask[] armyTypesE = new LayerMask[3];			//to jsou vrstvy nepratel			//je to v komentarich, protoze to bere ze scriptu SoldierE

	public float[] ranges = { 0.5f, 1.4f, 0.5f };				//tady jsou nadefinovane vzdalenosti kde mohou ubrat zivoty		(Soldier, Ranger, Tank)

	//public float[] rangesE = { 0.5f, 1.4f, 0.5f };			//tady jsou nadefinovano vzdalenosti kde mohou ubrat zivoty, ale je to urceno pro enemy

	public float movespeed;										//rychlost pohybu objektu(vojacka)

	public LayerMask armyType;                                  //zde se nastavi objektu layer jaky typ vojaka to je
    public int armyTypeNum;

	//Ohledne HPbaru
	public GameObject hpBar;

	public float[,] maxhp = { { 100, 60, 300 },{150,90,450 },{225,135,675},{ 350,200,1000},{400,300,1500 } };
	public float currhp;
	private float hpinprocents = 1f;
	public int level = 0;

	//Ohledne utoku
	public int[,] dmg = { {40, 60, 30 },{ 60, 90, 50}, { 90, 135, 70 }, { 135, 90, 115 }, { 150, 200, 120 } };
	public bool canGetdmgM = true;								//na blizko
	public bool canGetdmgR = true;                              //na dalku
	public bool[] enemies = { false, false, false };            //
	public Vector3 distanceFromAllie;							//nastaveni pro odstup od jednotek
	public bool[] alliesStop = { false, false, false };			//odstup od spojencu
	public bool[] enemiesStop = { false, false, false };		//odstup od nepratel

	public int[,] moneykill = { { 30, 50, 150 }, {60,100,300}, {120,200,600}, {240,400,1200}, {480,800,2400} };             //peniza za zabiti nepritele (soldier, ranger, tank)
	public int[] expperkill = { 100, 125, 300 };                //peniza za zabiti nepritele (soldier, ranger, tank)

	// Start is called before the first frame update
	void Start()
	{

		soldierEscript = soldierE.GetComponent<SoldierE>();     //import protivnika a jeho promennych
        //
        GameObject script1 = GameObject.FindWithTag("baseP");       //toto najde zakladnu hrace pomoci tagu ktery ma
        progresS = script1.GetComponent<ProgresScript>();
        //
        GameObject script2 = GameObject.FindWithTag("baseE");      //toto najde zakladnu nepritele pomoci tagu ktery ma
        enemyS = script2.GetComponent<EnemySpawn>();
        //
        level = progresS.level;									//potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
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
        CheckCollision();
		//														chyba nechce brat veci od nepritele****** uz to funguje ale to je warning tady jen kdyby
		hpinprocents = ((100 * currhp) / maxhp[level, armyTypeNum]) / 100;                                                  //premena zivotu na procenta		//potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
		rb.velocity = new Vector2((movespeed * 1), rb.velocity.y);                                                          //bude se hybyt do leva zatim je to testovaci
		for (int i = 0; i < 3; i++)
		{
			if (enemies[i] == true)
			{
				if (currhp <= 0)								//podminka proto zda ma zivoty
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
			if (currhp <= 0)									//pojistna podminka pro zkontrolovani zda ma objekt zivoty jeste
			{
				Destroy(gameObject);
			}
		}
		for (int i = 0;i < 3;i++)
		{
			if (alliesStop[i] == true || enemiesStop[i] == true)
			{
				movespeed = 0;
				break;
			}
			else
			{
				movespeed = 3;
			}
		}
        //pojistka proto kdyz zabije nepratelskou jednotku drive a nema okolo sebe nikoho jineho a ma 0 hp a nebo mene
        hpBar.transform.localScale = new Vector2(hpinprocents, hpBar.transform.localScale.y);								//zapisovani do hpbaru
	}
	public void CheckCollision()		//kontroluje kolize mezi vojaky
	{
		distanceFromAllie = new Vector3(transform.position.x + 1, transform.position.y, transform.position.y);
        for (int i = 0; i < 3; i++)
		{
			enemies[i] = Physics2D.OverlapCircle(transform.position, soldierEscript.ranges[i], soldierEscript.armyTypes[i]) != null;		//kontrola pro range nepratel
            alliesStop[i] = Physics2D.OverlapCircle(distanceFromAllie, 0.5f, armyTypes[i]) != null;											//kontrola pro odstup od jednotek		//udelat lepsi odstup
            enemiesStop[i] = Physics2D.OverlapCircle(transform.position, ranges[i], soldierEscript.armyTypes[i]) != null;					//kontrola pro odstup od nepratel		//udelat lepsi odstup
        }
	}

	IEnumerator DmgdealcooldownMelee()							//zde dostava dmg od jednotek, ktere jsou na blizko (melee)
	{
		canGetdmgM = false;
		if (enemies[0])		//pokud je to soldier
		{
			currhp -= soldierEscript.dmg[enemyS.level,0];                                                           //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
        }
		else if (enemies[2])//pokud je to tank
		{
			currhp -= soldierEscript.dmg[enemyS.level, 2];                                                          //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
        }
		//Debug.Log("Player " + currhp + " Melee");
		yield return new WaitForSeconds(3);
		canGetdmgM = true;
	}
	IEnumerator DmgdealcooldownRange()							//zde dostava dmg od jednotek, ktere jsou na dalku (ranger)
	{
		canGetdmgR = false;
		currhp -= soldierEscript.dmg[enemyS.level, 1];                                                              //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
		//Debug.Log("Player " + currhp + "Range");
		yield return new WaitForSecondsRealtime(2);
		canGetdmgR = true;
	}

	/*private void OnDrawGizmosSelected()						//vykreslí kruh okolo jednotky
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, rangeS);
	}*/
}