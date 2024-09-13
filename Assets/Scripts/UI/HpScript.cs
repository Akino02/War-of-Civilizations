using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpScript : MonoBehaviour
{
	//ProgresScript progresS;										//propojeni zakladnich scriptu pro funkci UI
	//ButtonScript buttonS;										//propojeni zakladnich scriptu pro funkci UI

	EnemySpawn enemyS;


	//hp a ubirani base
	public float[] maxHPBase = {1000,2000,3000,4000,5000};		//potøeba zmìnit poèet životù pøi updatu !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	public float currHPBase;
	public float hpbaseinprocents = 1f;
																//
	public Image hpBaseBarcurr;                                 //vizualni ukazatel zivotu
	public GameObject basePosition;                             //misto kde se nachazi zakladna
                                                                //

    public Text hpTextShow;										//aktualni zivoty do textu viditelneho
    public Text hpEnemyTextShow;


    public bool upgradingHp = false;
	//
	// Start is called before the first frame update
	void Start()
	{
		//progresS = GetComponent<ProgresScript>();				//propojeni zakladnich scriptu pro funkci UI
		//buttonS = GetComponent<ButtonScript>();					//propojeni zakladnich scriptu pro funkci UI
		/*soldierEscript = soldierE.GetComponent<SoldierE>();   //import protivnika a jeho promìnných*/
        currHPBase = maxHPBase[ProgresScript.level];

        //
        GameObject script2 = GameObject.FindWithTag("baseE");																//toto najde zakladnu nepritele pomoci tagu ktery ma
        enemyS = script2.GetComponent<EnemySpawn>();
		//
    }

	// Update is called once per frame
	void Update()
	{
		hpTextShow.text = Mathf.Round(currHPBase).ToString();
        hpEnemyTextShow.text = Mathf.Round(enemyS.currHPBase).ToString();


		hpBaseBarcurr.fillAmount = Mathf.Lerp(hpBaseBarcurr.fillAmount, currHPBase / maxHPBase[ProgresScript.level], 3f* Time.deltaTime);		//kolik mame aktualne, kolik budeme mit, rychlost jak se to bude posouvat nasobeno synchronizovany cas
		Color healthColor = Color.Lerp(Color.red, Color.green, (currHPBase / maxHPBase[ProgresScript.level]));									//nastaveni barev pro hpBar, pokud minHP tak red a pokud maxHP tak green a je to gradian
		hpBaseBarcurr.color = healthColor;						//zde se aplikuje barva gradianu, podle toho kolik ma hpBar zivotu
	}
    public void UpgradeHp()								//zachova procentuelne hp pri upgradu
    {
		if(ProgresScript.level > 0)
		{
			Debug.Log(currHPBase);
			Debug.Log(maxHPBase[ProgresScript.level - 1]);
            hpbaseinprocents = currHPBase / maxHPBase[ProgresScript.level - 1];			//pomoc pri pocitani procent(zde se zjistuje rozdil aktualnich hp a maximalnich, aby se to pak podle procent upravilo v dalsi fazi)
            currHPBase = hpbaseinprocents * maxHPBase[ProgresScript.level];				//vypocita aktualniho poctu hp v novych zivotech
        }
    }
}
