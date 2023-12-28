using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierE : MonoBehaviour
{
	SoldierP soldierPscript;									//import scriptu protivnika
	[SerializeField] GameObject soldierP;						//import objektu
    ProgresScript progresS;										//import script
    EnemySpawn enemyS;										//import script

	public Rigidbody2D rb;										//funkce pro gravitaci
	public LayerMask[] armyTypes = new LayerMask[3];
	//public LayerMask[] armyTypesE = new LayerMask[3];
	public float[] ranges = { 0.5f, 1.4f, 0.5f };
	public float movespeed;										//rychlost pohybu objektu
	public LayerMask armyType;									//zde se nastavi objektu layer jaky typ vojaka to je
	public int armyTypeNum;

	//Ohledne HPbaru
	public GameObject hpBar;

	public float[,] maxhp = { { 100, 60, 300 }, { 150, 90, 450 }, { 225, 135, 675 }, { 350, 200, 1000 }, { 400, 300, 1500 } };              //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
	public float currhp;
	private float hpinprocents = 1f;
	public int level = 0;                                                                                                   //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******

	//Ohledne utoku
	public int[,] dmg = { { 40, 60, 30 }, { 60, 90, 50 }, { 90, 135, 70 }, { 135, 90, 115 }, { 150, 200, 120 } };           //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
	public bool canGetdmgM = true;								//na blizko
	public bool canGetdmgR = true;								//na dalku
	public bool[] enemies = { false, false, false };
    public Vector3 distanceFromAllie;                           //nastaveni pro odstup od jednotek
    public bool[] alliesStop = { false, false, false };         //odstup od spojencu
    public bool[] enemiesStop = { false, false, false };        //odstup od nepratel


    private bool givemoney = true;								//cooldown na penize

	// Start is called before the first frame update
	void Start()
	{
		soldierPscript = soldierP.GetComponent<SoldierP>();		//import protivnika a jeho promìnných
		//
		GameObject script1 = GameObject.FindWithTag("baseP");		//toto najde zakladnu hrace pomoci tagu ktery ma
		progresS = script1.GetComponent<ProgresScript>();
        //
        GameObject script2 = GameObject.FindWithTag("baseE");      //toto najde zakladnu nepritele pomoci tagu ktery ma
        enemyS = script2.GetComponent<EnemySpawn>();
		//
		level = enemyS.level;
        for (int i = 0; i < 3; i++)
		{
			if (armyType == armyTypes[i])
			{
				//maxhp = hptype[i];
				currhp = maxhp[level,i];                        //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
				armyTypeNum = i;
			}
			//Debug.Log(i);
		}
	}
	// Update is called once per frame
	void Update()
	{
        CheckCollision();
        hpinprocents = ((100 * currhp) / maxhp[level,armyTypeNum]) / 100;													//potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
		rb.velocity = new Vector2((movespeed * -1), rb.velocity.y);															//bude se hybyt do leva zatim je to testovaci
		for (int i = 0; i < 3; i++)
		{
			if(enemies[i] == true)
			{
				movespeed = 0;
                if (currhp <= 0 && givemoney == true)
				{
					Death();									//funkce kdyz zemre
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
            if (currhp <= 0 && givemoney == true)
			{
				Death();										//pojistna funkce jestli je mrtvy
			}
		}
        for (int i = 0; i < 3; i++)
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
        //pojistka proto kdyz zabije hracovu jednotku drive a nema okolo sebe nikoho jineho a ma 0 hp a nebo mene
        hpBar.transform.localScale = new Vector2(hpinprocents, hpBar.transform.localScale.y);
	}
    public void CheckCollision()								//kontroluje kolize mezi vojaky
    {
        distanceFromAllie = new Vector3(transform.position.x - 1, transform.position.y, transform.position.y);
        for (int i = 0; i < 3; i++)
        {
            enemies[i] = Physics2D.OverlapCircle(transform.position, soldierPscript.ranges[i], soldierPscript.armyTypes[i]) != null;		//kontrola pro range nepratel
            alliesStop[i] = Physics2D.OverlapCircle(distanceFromAllie, 0.5f, armyTypes[i]) != null;											//kontrola pro odstup od jednotek
            enemiesStop[i] = Physics2D.OverlapCircle(transform.position, ranges[i], soldierPscript.armyTypes[i]) != null;					//kontrola pro odstup od nepratel
        }
    }
    public void Death()											//funkce pro zjisteni zda je objekt mrtvy a da penize a zkusenosti za jeho smrt
	{
		givemoney = false;
        progresS.money += soldierPscript.moneykill[level, armyTypeNum];                                      //zatim to dava penez tolik kdo ho zabil coz je spatne     potreba to dostat do UI z prefabu
        progresS.experience += soldierPscript.expperkill[armyTypeNum];                                       //zatim to dava penez tolik kdo ho zabil coz je spatne     potreba to dostat do UI z prefabu
		Debug.Log(soldierPscript.moneykill[level, armyTypeNum]);                                                            //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
		Debug.Log(soldierPscript.expperkill[armyTypeNum]);                                                                  //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
		Destroy(gameObject);
	}

	IEnumerator DmgdealcooldownMelee()
	{
		canGetdmgM = false;
		if (enemies[0] == true)
		{
			currhp -= soldierPscript.dmg[progresS.level,0];                                                           //tady je problem, ale je zatim opraven ale potrebuje se opravit mby(level od vojacka, je tam od base)
        }
		else if (enemies[2])
		{
			currhp -= soldierPscript.dmg[progresS.level,2];                                                           //tady je problem, ale je zatim opraven ale potrebuje se opravit mby(level od vojacka, je tam od base)
        }
		//Debug.Log("Enemy " + currhp);
		yield return new WaitForSecondsRealtime(3);
		canGetdmgM = true;
	}
	IEnumerator DmgdealcooldownRange()
	{
		canGetdmgR = false;
		currhp -= soldierPscript.dmg[progresS.level, 1];                                                              //tady je problem, ale je zatim opraven ale potrebuje se opravit mby(level od vojacka, je tam od base)
                                                                                                                      //Debug.Log("Enemy " + currhp);
        yield return new WaitForSecondsRealtime(2);
		canGetdmgR = true;
	}
}
