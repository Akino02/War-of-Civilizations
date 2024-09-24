using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpScript : MonoBehaviour
{
	ProgresScript progresS;										//propojeni zakladnich scriptu pro funkci UI
	//ButtonScript buttonS;										//propojeni zakladnich scriptu pro funkci UI

	//EnemySpawn enemyS;


	//hp a ubirani base
	//public float[] maxHPBase = {1000,2000,3000,4000,5000};		//potøeba zmìnit poèet životù pøi updatu !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	public static float currHPBase;
	public float hpbaseinprocents = 1f;
																//
	//public GameObject basePosition;                             //misto kde se nachazi zakladna
                                                                //

    public Text hpTextShow;                                     //aktualni zivoty do textu viditelneho
    public Image hpBaseBarcurr;                                 //vizualni ukazatel zivotu

    public bool upgradingHp = false;
	//
	// Start is called before the first frame update
	void Start()
	{
		progresS = GetComponent<ProgresScript>();				//propojeni zakladnich scriptu pro funkci UI
		//buttonS = GetComponent<ButtonScript>();					//propojeni zakladnich scriptu pro funkci UI
		/*soldierEscript = soldierE.GetComponent<SoldierE>();   //import protivnika a jeho promìnných*/
        currHPBase = UnityConfiguration.maxHPBase[progresS.level];

        /*//
        GameObject script2 = GameObject.FindWithTag("baseE");																//toto najde zakladnu nepritele pomoci tagu ktery ma
        enemyS = script2.GetComponent<EnemySpawn>();
		//*/
    }

	// Update is called once per frame
	void Update()
	{
		hpTextShow.text = Mathf.Round(currHPBase).ToString();


		hpBaseBarcurr.fillAmount = Mathf.Lerp(hpBaseBarcurr.fillAmount, currHPBase / UnityConfiguration.maxHPBase[progresS.level], 3f* Time.deltaTime);		//kolik mame aktualne, kolik budeme mit, rychlost jak se to bude posouvat nasobeno synchronizovany cas
		Color healthColor = Color.Lerp(Color.red, Color.green, (currHPBase / UnityConfiguration.maxHPBase[progresS.level]));									//nastaveni barev pro hpBar, pokud minHP tak red a pokud maxHP tak green a je to gradian
		hpBaseBarcurr.color = healthColor;						//zde se aplikuje barva gradianu, podle toho kolik ma hpBar zivotu
	}
    public void UpgradeHp()								//zachova procentuelne hp pri upgradu
    {
		if(progresS.level > 0)
		{
			Debug.Log(currHPBase);
			Debug.Log(UnityConfiguration.maxHPBase[progresS.level - 1]);
            hpbaseinprocents = currHPBase / UnityConfiguration.maxHPBase[progresS.level - 1];			//pomoc pri pocitani procent(zde se zjistuje rozdil aktualnich hp a maximalnich, aby se to pak podle procent upravilo v dalsi fazi)
            currHPBase = hpbaseinprocents * UnityConfiguration.maxHPBase[progresS.level];				//vypocita aktualniho poctu hp v novych zivotech
        }
    }
}
