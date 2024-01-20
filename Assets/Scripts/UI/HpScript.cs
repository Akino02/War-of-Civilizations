using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpScript : MonoBehaviour
{
	ProgresScript progresS;										//propojeni zakladnich scriptu pro funkci UI
	ButtonScript buttonS;										//propojeni zakladnich scriptu pro funkci UI

	/*SoldierE soldierEscript;                                  //import scriptu protivnika									//potreba upravit ubirani hp pro zakladnu
	[SerializeField] GameObject soldierE;                       //import objektu*/

	/*UniArmy army;
	public GameObject objectArmyE;*/

	//EnemySpawn enemyS;

	//nepratele (layers)
	public LayerMask[] opponents = new LayerMask[3];            //layer nepratelskych jednotek soldier,ranger,tank
																//
	//hp a ubirani base
	public float[] maxHPBase = {1000,2000,3000,4000,5000};		//potøeba zmìnit poèet životù pøi updatu !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	public float currHPBase;
	public float hpbaseinprocents = 1f;
																//
	public Image hpBaseBarcurr;                                 //vizualni ukazatel zivotu
	public GameObject basePosition;                             //misto kde se nachazi zakladna
																//
	/*public bool canGetdmgM = true;
	public bool canGetdmgR = true;*/
	public bool upgradingHp = false;
	//
	// Start is called before the first frame update
	void Start()
	{
		progresS = GetComponent<ProgresScript>();				//propojeni zakladnich scriptu pro funkci UI
		buttonS = GetComponent<ButtonScript>();					//propojeni zakladnich scriptu pro funkci UI
		/*soldierEscript = soldierE.GetComponent<SoldierE>();   //import protivnika a jeho promìnných*/
        currHPBase = maxHPBase[progresS.level];

        //
        /*GameObject script2 = GameObject.FindWithTag("baseE");																//toto najde zakladnu nepritele pomoci tagu ktery ma
        enemyS = script2.GetComponent<EnemySpawn>();*/
		//
    }

	// Update is called once per frame
	void Update()
	{
        //toto slouzi pro ubirani zivotu zakladny
        /*for (int i = 0; i < 3; i++)
		{
			if (currHPBase > 0)
			{
				if (Physics2D.OverlapCircle(basePosition.transform.position, 1.4f, opponents[i]) != null && i == 1 && canGetdmgR == true)
				{
					StartCoroutine(DmgdealcooldownRange());
				}
				else if (Physics2D.OverlapCircle(basePosition.transform.position, 0.7f, opponents[i]) != null && i != 1 && canGetdmgM == true)
				{
					StartCoroutine(DmgdealcooldownMelee());
				}
			}
		}*/
		hpBaseBarcurr.fillAmount = Mathf.Lerp(hpBaseBarcurr.fillAmount, currHPBase / maxHPBase[progresS.level], 3f* Time.deltaTime);		//kolik mame aktualne, kolik budeme mit, rychlost jak se to bude posouvat nasobeno synchronizovany cas
		Color healthColor = Color.Lerp(Color.red, Color.green, (currHPBase / maxHPBase[progresS.level]));									//nastaveni barev pro hpBar, pokud minHP tak red a pokud maxHP tak green a je to gradian
		hpBaseBarcurr.color = healthColor;						//zde se aplikuje barva gradianu, podle toho kolik ma hpBar zivotu
	}
    /*IEnumerator DmgdealcooldownMelee()                          //base bude dostavat dmg od enemy melee				//potreba pak upravit system ubirani zivotu
	{
		canGetdmgM = false;
		if (Physics2D.OverlapCircle(basePosition.transform.position, 0.7f, opponents[0]) != null)
		{
			currHPBase -= soldierEscript.dmg[enemyS.level, 0];                                                       //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
		}
		else if (Physics2D.OverlapCircle(basePosition.transform.position, 0.7f, opponents[2]) != null)
		{
			currHPBase -= soldierEscript.dmg[enemyS.level, 2];                                                       //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
		}
		Debug.Log("Player " + currHPBase);
		yield return new WaitForSeconds(3);
		canGetdmgM = true;
	}
	IEnumerator DmgdealcooldownRange()                          //base bude dostavat dmg od enemy ranged
	{
		canGetdmgR = false;
		currHPBase -= soldierEscript.dmg[enemyS.level, 1];                                                          //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
		Debug.Log("Player " + currHPBase);
		yield return new WaitForSecondsRealtime(2);
		canGetdmgR = true;
	}*/
    public IEnumerator UpgradeHp()								//zachova procentuelne hp pri upgradu
    {
		if(progresS.level > 0)
		{
			Debug.Log(currHPBase);
			Debug.Log(maxHPBase[progresS.level - 1]);
            hpbaseinprocents = currHPBase / maxHPBase[progresS.level - 1];			//pomoc pri pocitani procent(zde se zjistuje rozdil aktualnich hp a maximalnich, aby se to pak podle procent upravilo v dalsi fazi)
            currHPBase = hpbaseinprocents * maxHPBase[progresS.level];				//vypocita aktualniho poctu hp v novych zivotech
        }
		yield return currHPBase;
    }
}
