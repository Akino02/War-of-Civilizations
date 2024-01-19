using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;   //import teto funkce, abych mohl pracovat s UI vecmi v unity enginu

public class EnemySpawn : MonoBehaviour
{
	//import enemy scriptu pro damage jaky davaji
	/*SoldierP soldierPscript;									//import scriptu protivnika
	SoldierE soldierEscript;*/
	ProgresScript progresS;                                     //importuje script zakladnu hrace

    UniArmy army;                                               //importovani pro pracovani s vojacky
    public GameObject objectArmyE;                              //objekt pro propojeni scriptu

    /*[SerializeField] GameObject soldierP;          //import objektu
	[SerializeField] GameObject soldierE;          //import objektu*/

    //
    //import layeru nepratel(hracovych)
    public LayerMask opponentSoldier;       //layer hracovych jednotek typu soldier
	public LayerMask opponentRanger;       //layer hracovych jednotek typu ranger
	public LayerMask opponentTank;       //layer hracovych jednotek typu tank
	//
	//
	//co bude spawnovat a kde
	public GameObject soldier;          //co spawne
	/*public GameObject ranger;          //co spawne
	public GameObject tank;          //co spawne*/
	public GameObject baseSpawner;    //kde to spawne
	//
	//veci ohledne baseHP ci damage pro base
	public float[] maxHPBase = {1000,2000,3000,4000,5000};
	public float currHPBase;
	public float hpbaseinprocents = 1f;

	public int level = 0;
	public int[] lvltype = {60, 15};
	public bool evolving = false;

    public GameObject[] baseAppearance = new GameObject[5];     //vzhled budov v array ohledne nove evoluce

    public Image hpBaseBarcurr;
	public GameObject basePosition;

	public bool canGetdmgM = true;
	public bool canGetdmgR = true;
	//
	//spawnovani jednotek
	public bool canSpawn = true;
	private int nahoda = 0;
	//
	// Start is called before the first frame update
	void Start()
	{
        GameObject item = GameObject.FindWithTag("baseP");		//toto najde zakladnu hrace pomoci tagu ktery ma
        progresS = item.GetComponent<ProgresScript>();			//zde se dosadi script za objekt

        /*soldierPscript = soldierP.GetComponent<SoldierP>();  //import protivnika a jeho promìnných
		soldierEscript = soldierE.GetComponent<SoldierE>();  //import protivnika a jeho promìnných*/

        army = objectArmyE.GetComponent<UniArmy>();				//propojeni scriptu UniArmy s ProgresScript
    }

	// Update is called once per frame
	void Update()
	{
        if(currHPBase > 0 && level != 4)																				//mimo provoz !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!****Zkoumani problemu
		{
			StartCoroutine(Evolution());
        }
        //toto slouzi pro spawn vojaku
        if (canSpawn == true && nahoda >= 1 && nahoda <= 3 && currHPBase > 0)
		{
			StartCoroutine(CoolDownArmySpawn());
		}
		else if(nahoda == 0 || nahoda >= 4)
		{
			nahoda = Random.Range(1, 5);
		}
		//damage pro base
		/*if ((Physics2D.OverlapCircle(basePosition.transform.position, 0.7f, opponentSoldier) != null || Physics2D.OverlapCircle(basePosition.transform.position, 0.7f, opponentTank) != null) && canGetdmgM == true && currHPBase > 0)  //nejaky nepritel muze ubrat zivoty zakladny
		{
			StartCoroutine(DmgdealcooldownMelee());
		}
		if (Physics2D.OverlapCircle(basePosition.transform.position, 1.4f, opponentRanger) != null && canGetdmgR == true && currHPBase > 0)
		{
			StartCoroutine(DmgdealcooldownRange());
		}*/
        hpBaseBarcurr.fillAmount = Mathf.Lerp(hpBaseBarcurr.fillAmount, currHPBase / maxHPBase[level], 3f * Time.deltaTime);       //kolik mame aktualne, kolik budeme mit, rychlost jak se to bude posouvat nasobeno synchronizovany cas
        Color healthColor = Color.Lerp(Color.red, Color.green, (currHPBase / maxHPBase[level]));                                   //nastaveni barev pro hpBar, pokud minHP tak red a pokud maxHP tak green a je to gradian
        hpBaseBarcurr.color = healthColor;                      //zde se aplikuje barva gradianu, podle toho kolik ma hpBar zivotu
    }

	IEnumerator CoolDownArmySpawn()      //nastaveni na prestavku at nemuze to spamovat to klikani a spawnovani
	{
		canSpawn = false;
		if(nahoda == 1)
		{
			yield return new WaitForSecondsRealtime(5);
            army.armyType = army.soldier;
            Instantiate(soldier, baseSpawner.transform.position, baseSpawner.transform.rotation);
		}
		else if(nahoda == 2)
		{
			yield return new WaitForSecondsRealtime(8);
            army.armyType = army.ranger;
            Instantiate(soldier, baseSpawner.transform.position, baseSpawner.transform.rotation);
		}
		else if(nahoda == 3)
		{
			yield return new WaitForSecondsRealtime(10);
            army.armyType = army.tank;
            Instantiate(soldier, baseSpawner.transform.position, baseSpawner.transform.rotation);
		}
		nahoda = Random.Range(1, 5);
		canSpawn = true;
	}
	//base bude dostavat dmg od enemy
	/*IEnumerator DmgdealcooldownMelee()
	{
		canGetdmgM = false;
		if (Physics2D.OverlapCircle(basePosition.transform.position, 0.7f, opponentSoldier) != null)
		{
			currHPBase -= soldierPscript.dmg[progresS.level, 0];                                                      //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
        }
		else if (Physics2D.OverlapCircle(basePosition.transform.position, 0.7f, opponentTank) != null)
		{
			currHPBase -= soldierPscript.dmg[progresS.level, 2];                                                      //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
        }
		Debug.Log("Player " + currHPBase);
		yield return new WaitForSeconds(3);
		canGetdmgM = true;
	}
	IEnumerator DmgdealcooldownRange()
	{
		canGetdmgR = false;
		currHPBase -= soldierPscript.dmg[progresS.level, 1];                                                          //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
        Debug.Log("Player " + currHPBase);
		yield return new WaitForSecondsRealtime(2);
		canGetdmgR = true;
	}*/
    IEnumerator Evolution()                                     //toto bude primo pro enemy system pro evoluce			//jeste to neni upravene pro Enemy	//mimo provoz !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    {
        if (evolving == false && level != 4)
        {
            evolving = true;
            StartCoroutine(UpgradeHp());					//pro vylepseni zivotu s tim, ze se zachova %
			if(progresS.level > level)
			{
				yield return new WaitForSeconds(lvltype[1]);    //pokud je nepritel pozadu tak jeho evolution time bude kazdych 15s
                level += 1;
                evolving = false;
            }
			else
			{
				yield return new WaitForSeconds(lvltype[0]);    //pokud je nepritel stejne rychly nebo rychlejsi tak jeho evolution time bude kazdych 60s
                level += 1;
                evolving = false;

            }
            for (int i = 0; i < 5; i++)
            {
                 if (level == i)                                 //zatim jsou jen 4, aby to mohlo fungovat pozdeji jich bude 5 mozna vice
                 {
					baseAppearance[i].SetActive(true);
                 }
                 else
                 {
					baseAppearance[i].SetActive(false);
                 }
            }
        }
    }
    IEnumerator UpgradeHp()								//zachova procentuelne hp pri upgradu			//sledovat fungovani
    {
        if (level > 0)
        {
            Debug.Log(currHPBase);
            Debug.Log(maxHPBase[level - 1]);
            hpbaseinprocents = currHPBase / maxHPBase[level - 1];      //pomoc pri pocitani procent
            currHPBase = hpbaseinprocents * maxHPBase[level];          //vypocita aktualni pocet hp v novych zivotech
        }
        yield return currHPBase;
    }
}
