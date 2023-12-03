using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;                                           //import teto funkce, abych mohl pracovat s UI vecmi v unity enginu

public class SpawnScript : MonoBehaviour
{
	//import enemy scriptu pro damage jaky davaji
	SoldierE soldierEscript;                                    //import scriptu protivnika
	[SerializeField] GameObject soldierE;                       //import objektu
	ProgresScript progresS;										//propojeni zakladnich scriptu pro funkci UI
																//

	//co a kde to bude spawnovat
	public GameObject soldierP;                                 //To je objekt soldier
	public GameObject rangerP;                                  //To je objekt ranger
	public GameObject tankP;                                    //To je objekt tank
	public GameObject playerSpawner;                            //misto kde se tyto objekty spawnou


	//nepratele (layers)
	public LayerMask opponentSoldier;                           //layer hracovych jednotek typu soldier						//tohle bych mel upravit na jeden odkaz
	public LayerMask opponentRanger;                            //layer hracovych jednotek typu ranger
	public LayerMask opponentTank;                              //layer hracovych jednotek typu tank
																//

	//hp a ubirani base
	public float maxHPBase = 1000;                                  //potøeba zmìnit poèet životù pøi updatu !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	public float currHPBase = 1000;
	public float hpbaseinprocents = 1f;

	public Image hpBaseBarcurr;
	public GameObject basePosition;

	public bool canGetdmgM = true;
	public bool canGetdmgR = true;
	//

	// Start is called before the first frame update
	void Start()
	{
		soldierEscript = soldierE.GetComponent<SoldierE>();     //import protivnika a jeho promìnných
        progresS = GetComponent<ProgresScript>();     //propojeni zakladnich scriptu pro funkci UI
		//Debug.Log(progresS.money);
	}

	// Update is called once per frame
	void Update()
	{
		hpbaseinprocents = ((100 * currHPBase) / maxHPBase) / 100;                                                          //pomoc pri pocitani procent
		//toto slouzi pro ubirani zivotu zakladny
		if ((Physics2D.OverlapCircle(basePosition.transform.position, 0.7f, opponentSoldier) != null || Physics2D.OverlapCircle(basePosition.transform.position, 0.7f, opponentTank) != null) && canGetdmgM == true && currHPBase > 0)  //nejaky nepritel muze ubrat zivoty zakladny
		{
			StartCoroutine(DmgdealcooldownMelee());
		}
		if (Physics2D.OverlapCircle(basePosition.transform.position, 1.4f, opponentRanger) != null && canGetdmgR == true && currHPBase > 0)
		{
			StartCoroutine(DmgdealcooldownRange());
		}
		hpBaseBarcurr.fillAmount = hpbaseinprocents;			//urcovani zivotu v procentech
	}
	//to jsou funkce pro cudliky
	public void SoldierSpawn()									//tato funkce na kliknuti spawne jednoho vojaka				PRO SOLDIERA
	{
		if (progresS.order < 5 && currHPBase > 0 && progresS.money >= 15)						//jeste tam pak doplnit ze za to bude platit
		{
            progresS.order += 1;
            progresS.orderv2[progresS.order -1] = 1;
            progresS.money -= 15;
			Debug.Log("Prirazeno do fronty " + progresS.order);
		}
		else
		{
			Debug.Log("Fronta je plna " + progresS.order + "nebo nemas penize");
		}
	}
	public void RangerSpawn()									// tato funkce na kliknuti spawne jednoho vojaka			PRO RANGERA
	{
		if (progresS.order < 5 && currHPBase > 0 && progresS.money >= 25)			//jeste tam pak doplnit ze za to bude platit
		{
            progresS.order += 1;
            progresS.orderv2[progresS.order - 1] = 2;
            progresS.money -= 25;
			Debug.Log("Prirazeno do fronty " + progresS.order);
		}
		else
		{
			Debug.Log("Fronta je plna " + progresS.order + "nebo nemas penize");
		}
	}
	public void TankSpawn()										// tato funkce na kliknuti spawne jednoho vojaka			PRO TANK
	{
		if (progresS.order < 5 && currHPBase > 0 && progresS.money >= 100)		//jeste tam pak doplnit ze za to bude platit
		{
            progresS.order += 1;
            progresS.orderv2[progresS.order - 1] = 3;
            progresS.money -= 100;
			Debug.Log("Prirazeno do fronty " + progresS.order);
		}
		else
		{
			Debug.Log("Fronta je plna " + progresS.order + "nebo nemas penize");
		}
	}
	/*public void EvolutionUpgrade()												//funkce pro button, ktery bude evolvovat hracovy jednotky a zakladnu	(BUTTON NENI HOTOVY)
	{
        if (experience >= nextlevelup && level != 4)
        {
            experience -= nextlevelup;
            level += 1;
            for (int i = 0; i < 3; i++)                         //pise do vsech textu, ktere jsou uchovany v poli
            {
                actionButtonText[i].text = "lvl." + (level + 1);
            }
            for (int i = 0; i < 2; i++)
            {
                if (level == i)                                 //zatim jsou jen 2, aby to mohlo fungovat pozdeji jich bude 5 mozna vice
                {
                    baseAppearance[i].SetActive(true);
                }
                else
                {
                    baseAppearance[i].SetActive(false);
                }
            }
        }
        else
        {
            experienceText.text = experienceinprocents.ToString() + "%";
        }
    }*/
	//funkce pro progressBar
	IEnumerator DmgdealcooldownMelee()                          //base bude dostavat dmg od enemy melee
	{
		canGetdmgM = false;
		if (Physics2D.OverlapCircle(basePosition.transform.position, 0.7f, opponentSoldier) != null)
		{
			currHPBase -= soldierEscript.dmg[soldierEscript.level,0];                                                       //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
		}
		else if (Physics2D.OverlapCircle(basePosition.transform.position, 0.7f, opponentTank) != null)
		{
			currHPBase -= soldierEscript.dmg[soldierEscript.level,2];                                                       //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
		}
		Debug.Log("Player " + currHPBase);
		yield return new WaitForSeconds(3);
		canGetdmgM = true;
	}
	IEnumerator DmgdealcooldownRange()							//base bude dostavat dmg od enemy ranged
	{
		canGetdmgR = false;
		currHPBase -= soldierEscript.dmg[soldierEscript.level, 1];                                                          //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
		Debug.Log("Player " + currHPBase);
		yield return new WaitForSecondsRealtime(2);
		canGetdmgR = true;
	}
}
