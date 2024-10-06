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
	HpScript hpS;

    UnitScript army;                                                //importovani pro pracovani s vojacky
    public GameObject objectArmyE;                                  //objekt pro propojeni scriptu

    /*[SerializeField] GameObject soldierP;						//import objektu
	[SerializeField] GameObject soldierE;						//import objektu*/

    //
    //import layeru nepratel(hracovych)
    /*public LayerMask opponentSoldier;							//layer hracovych jednotek typu soldier
	public LayerMask opponentRanger;							//layer hracovych jednotek typu ranger
	public LayerMask opponentTank;								//layer hracovych jednotek typu tank*/
	//
	//
	//Factory (Unit)
	private int[] waitTime = { 6, 9, 13};					//soldier, ranger, tank, cant Build
    public float timeToCreateUnit = 0f;
    private float speedForCreatingUnit = 0;
    public bool isCreatingUnit = false;
	//
	//veci ohledne baseHP ci damage pro base
	//public float[] maxHPBase = {1000,2000,3000,4000,5000};		//zivoty zakladny
	public static float currHPBase;
	public float hpbaseinprocents = 1f;

	public int level = 0;
	public int lvlTypeWait = 15;							//cas byl upraven a jeste podminka pro evoluce
	public bool evolving = false;
	private int EvolveExperiencePro = 90;						//procenta zkusenosti od, kterych se zacne vylepsovat enemy

    public GameObject[] baseAppearance = new GameObject[5];     //vzhled budov v array ohledne nove evoluce

    public Image hpBaseBarcurr;
    public Text hpEnemyTextShow;                                     //aktualni zivoty do textu viditelneho
    public GameObject basePosition;


	//
	//spawnovani jednotek
	public bool canSpawn = true;
	private int randomPickUnit = 0;
    private int[] countToMakeTankCombo = {0,0};
    private int[] difficulty = { 15, 10, 5};					    //obtiznost hry (ve forme vyroby comba)
	//
	// Start is called before the first frame update
	void Start()
	{
        GameObject item = GameObject.FindWithTag("baseP");		//toto najde zakladnu hrace pomoci tagu ktery ma
        progresS = item.GetComponent<ProgresScript>();			//zde se dosadi script za objekt
		//hpS = item.GetComponent<HpScript>();				

        /*soldierPscript = soldierP.GetComponent<SoldierP>();	//import protivnika a jeho promìnných
		soldierEscript = soldierE.GetComponent<SoldierE>();		//import protivnika a jeho promìnných*/

        army = objectArmyE.GetComponent<UnitScript>();             //propojeni scriptu UniArmy s ProgresScript

        currHPBase = UnityConfiguration.maxHPBase[level];
    }

	// Update is called once per frame
	void Update()
	{
        /*if(level == 4)
        {
            for (int i = 0; i < waitTime.Length; i++)
            {
                waitTime[i] = waitTime[i] + 2;
            }
        }*/
        CreatingUnit();
        PickUnit();
        if(level != 4 && currHPBase > 0 && HpScript.currHPBase > 0)
		{
			StartCoroutine(Evolution());
        }
        if (currHPBase <= 0)
        {
            currHPBase = 0;
        }
        /*if (canSpawn == true && currHPBase > 0 && HpScript.currHPBase > 0)
		{
			StartCoroutine(CoolDownArmySpawn());
		}*/
        hpBaseBarcurr.fillAmount = Mathf.Lerp(hpBaseBarcurr.fillAmount, currHPBase / UnityConfiguration.maxHPBase[level], 3f * Time.deltaTime);       //kolik mame aktualne, kolik budeme mit, rychlost jak se to bude posouvat nasobeno synchronizovany cas
        Color healthColor = Color.Lerp(Color.red, Color.green, (currHPBase / UnityConfiguration.maxHPBase[level]));                                   //nastaveni barev pro hpBar, pokud minHP tak red a pokud maxHP tak green a je to gradian
        hpBaseBarcurr.color = healthColor;                      //zde se aplikuje barva gradianu, podle toho kolik ma hpBar zivotu
        hpEnemyTextShow.text = Mathf.Round(currHPBase).ToString();
    }

    private void PickUnit()
    {
        if (currHPBase > 0 && HpScript.currHPBase > 0 && isCreatingUnit != true)
        {
            //speedForCreatingUnit = 0f;
            randomPickUnit = Random.Range(0, 2);
            for (int i = 0; i < 2; i++)
            {
                if (i == randomPickUnit)
                {
                    //speedForCreatingUnit = Time.deltaTime / waitTime[i];
                    //zacatek comba pro nepritele spawne nejdrive tank a potom spawne rangera a tim se ukonci combo
                    if (countToMakeTankCombo[1] >= 1)
                    {
                        countToMakeTankCombo[1] = 0;
                        randomPickUnit = 1;
                        Debug.Log("End of Combo!!");
                    }
                    else if (countToMakeTankCombo[0] >= 5)
                    {
                        countToMakeTankCombo[0] = 0;
                        countToMakeTankCombo[1] = 1;
                        randomPickUnit = 2;
                        Debug.Log("Combo started !!!!!");
                    }
                    //
                    isCreatingUnit = true;
                    timeToCreateUnit = 0f;
                    /*Debug.Log("Enemy building" + randomPickUnit);
                    Debug.Log(waitTime[i]);*/
                }
            }
        }
    }
    private void CreatingUnit()
    {
        if (isCreatingUnit)
        {
            speedForCreatingUnit = Time.deltaTime / waitTime[randomPickUnit];
            timeToCreateUnit = Mathf.Lerp(timeToCreateUnit, timeToCreateUnit + 1f, speedForCreatingUnit);
            if (timeToCreateUnit >= 1 && !LogScript.isGameOver)
            {
                army.armyType = army.armyTypeLayer[randomPickUnit];
                Instantiate(objectArmyE, transform.position, transform.rotation);
                /*Debug.Log($"Enemy built {randomPickUnit}");
                Debug.Log(randomPickUnit);*/
                countToMakeTankCombo[0]++;
                isCreatingUnit = false;
            }
        }
    }
    private void sortOrder()
    {

    }

	/*IEnumerator CoolDownArmySpawn()								//nastaveni na prestavku at nemuze to spamovat to klikani a spawnovani                      //OPRAVIT OPRAVIT OPRAVIT
	{
		canSpawn = false;
        //for (int unitType = 1; unitType <= army.armyTypeLayer.Length; unitType++)
        //{
        //    if(nahoda == unitType)
        //    {
        //        yield return new WaitForSeconds(waitTime[unitType-1]);
        //        army.armyType = army.armyTypeLayer[unitType - 1];
        //        Instantiate(soldier, baseSpawner.transform.position, baseSpawner.transform.rotation);
        //    }
        //    else
        //    {
        //        yield return new WaitForSeconds(waitTime[3]);
        //        Debug.Log("Cant build");
        //    }
        //    Debug.Log(nahoda);
        //}
		if(randomPickUnit == 1)
		{
			yield return new WaitForSeconds(waitTime[0]);
            army.armyType = army.armyTypeLayer[0];
            Instantiate(objectArmyE, transform.position, transform.rotation);
		}
		else if(randomPickUnit == 2)
		{
			yield return new WaitForSeconds(waitTime[1]);
            army.armyType = army.armyTypeLayer[0];
            Instantiate(objectArmyE, transform.position, transform.rotation);
		}
		else if(randomPickUnit == 3)
		{
			yield return new WaitForSeconds(waitTime[2]);
            army.armyType = army.armyTypeLayer[0];
            Instantiate(objectArmyE, transform.position, transform.rotation);
		}
		else
		{
			yield return new WaitForSeconds(waitTime[3]);
			Debug.Log("Cant build");
		}
        randomPickUnit = Random.Range(1, difficulty[2]);			//easy 0, normal 1, hard 2
		canSpawn = true;
	}*/
    void UpgradeHp()										//zachova procentuelne hp pri upgradu			//sledovat fungovani
    {
        if (level > 0)
        {
            Debug.Log(currHPBase);
            Debug.Log(UnityConfiguration.maxHPBase[level - 1]);
            hpbaseinprocents = currHPBase / UnityConfiguration.maxHPBase[level - 1];						//pomoc pri pocitani procent(zde se zjistuje rozdil aktualnich hp a maximalnich, aby se to pak podle procent upravilo v dalsi fazi)
            currHPBase = hpbaseinprocents * UnityConfiguration.maxHPBase[level];						//vypocita aktualniho poctu hp v novych zivotech
        }
        //yield return currHPBase;
    }
    IEnumerator Evolution()                                     //toto bude primo pro enemy system pro evoluce
    {
        if (evolving == false && level != 4 && isCreatingUnit == false)
        {
			if (progresS.experience >= (UnityConfiguration.nextlevelup * EvolveExperiencePro) / 100 && progresS.level == level)		//urcit jinak podminku
			{
                evolving = true;
                yield return new WaitForSeconds(lvlTypeWait);
                level += 1;
                evolving = false;

                for (int i = 0; i < 5; i++)
                {
                    if (level == i)
                    {
                        baseAppearance[i].SetActive(true);
                    }
                    else
                    {
                        baseAppearance[i].SetActive(false);
                    }
                }
                UpgradeHp();
            }
			else if(progresS.level > level)
			{
                evolving = true;
                yield return new WaitForSeconds(lvlTypeWait);
                level += 1;
                evolving = false;

                for (int i = 0; i < 5; i++)
                {
                    if (level == i)
                    {
                        baseAppearance[i].SetActive(true);
                    }
                    else
                    {
                        baseAppearance[i].SetActive(false);
                    }
                }
                UpgradeHp();
            }
        }
    }
}
