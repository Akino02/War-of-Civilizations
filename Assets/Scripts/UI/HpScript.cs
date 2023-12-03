using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpScript : MonoBehaviour
{
	ProgresScript progresS;										//propojeni zakladnich scriptu pro funkci UI
	ButtonScript buttonS;											//propojeni zakladnich scriptu pro funkci UI

	SoldierE soldierEscript;                                    //import scriptu protivnika
	[SerializeField] GameObject soldierE;                       //import objektu

	//nepratele (layers)
	public LayerMask[] opponents = new LayerMask[3];            //layer nepratelskych jednotek soldier,ranger,tank
																//
	//hp a ubirani base
	public float maxHPBase = 1000;								//potøeba zmìnit poèet životù pøi updatu !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	public float currHPBase = 1000;
	public float hpbaseinprocents = 1f;
																//
	public Image hpBaseBarcurr;                                 //vizualni ukazatel zivotu
	public GameObject basePosition;                             //misto kde se nachazi zakladna
																//
	public bool canGetdmgM = true;
	public bool canGetdmgR = true;
	//
	// Start is called before the first frame update
	void Start()
	{
		progresS = GetComponent<ProgresScript>();     //propojeni zakladnich scriptu pro funkci UI
		buttonS = GetComponent<ButtonScript>();     //propojeni zakladnich scriptu pro funkci UI
		soldierEscript = soldierE.GetComponent<SoldierE>();     //import protivnika a jeho promìnných
	}

	// Update is called once per frame
	void Update()
	{
		hpbaseinprocents = ((currHPBase) / maxHPBase);			//pomoc pri pocitani procent
		//toto slouzi pro ubirani zivotu zakladny
		for (int i = 0; i < 3; i++)
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
		}
		hpBaseBarcurr.fillAmount = Mathf.Lerp(hpBaseBarcurr.fillAmount, currHPBase / maxHPBase, 3f* Time.deltaTime);		//kolik mame aktualne, kolik budeme mit, rychlost jak se to bude posouvat nasobeno synchronizovany cas
		Color healthColor = Color.Lerp(Color.red, Color.green, (currHPBase / maxHPBase));									//nastaveni barev pro hpBar, pokud minHP tak red a pokud maxHP tak green a je to gradian
		hpBaseBarcurr.color = healthColor;						//zde se aplikuje barva gradianu, podle toho kolik ma hpBar zivotu
	}
	IEnumerator DmgdealcooldownMelee()                          //base bude dostavat dmg od enemy melee				//potreba pak upravit system ubirani zivotu
	{
		canGetdmgM = false;
		if (Physics2D.OverlapCircle(basePosition.transform.position, 0.7f, opponents[0]) != null)
		{
			currHPBase -= soldierEscript.dmg[soldierEscript.level, 0];                                                       //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
		}
		else if (Physics2D.OverlapCircle(basePosition.transform.position, 0.7f, opponents[2]) != null)
		{
			currHPBase -= soldierEscript.dmg[soldierEscript.level, 2];                                                       //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
		}
		Debug.Log("Player " + currHPBase);
		yield return new WaitForSeconds(3);
		canGetdmgM = true;
	}
	IEnumerator DmgdealcooldownRange()                          //base bude dostavat dmg od enemy ranged
	{
		canGetdmgR = false;
		currHPBase -= soldierEscript.dmg[soldierEscript.level, 1];                                                          //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
		Debug.Log("Player " + currHPBase);
		yield return new WaitForSecondsRealtime(2);
		canGetdmgR = true;
	}
}
