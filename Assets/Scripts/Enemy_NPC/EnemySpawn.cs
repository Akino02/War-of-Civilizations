using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;   //import teto funkce, abych mohl pracovat s UI vecmi v unity enginu

public class EnemySpawn : MonoBehaviour
{
	//import enemy scriptu pro damage jaky davaji
	/*SoldierP soldierPscript;									//import scriptu protivnika
	SoldierE soldierEscript;*/
	EvolutionPlayerScript evolutionPlayerS;                                     //importuje script zakladnu hrace
	EvolutionEnemyScript evolutionEnemyS;                                     //importuje script zakladnu hrace
	HpScript hpEnemyS;

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
    //public static float currHPBase;

    //public int currLevelBase;
    public float hpbaseinprocents = 1f;

    /*//Evolution data
	public int level = 0;
	public int lvlTypeWait = 15;							//cas byl upraven a jeste podminka pro evoluce
	public bool evolving = false;
	private int EvolveExperiencePro = 90;						//procenta zkusenosti od, kterych se zacne vylepsovat enemy

    public GameObject[] baseAppearance = new GameObject[5];     //vzhled budov v array ohledne nove evoluce*/

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
        evolutionPlayerS = item.GetComponent<EvolutionPlayerScript>();          //zde se dosadi script za objekt


        hpEnemyS = GetComponent<HpScript>();
        evolutionEnemyS = GetComponent<EvolutionEnemyScript>();				

        /*soldierPscript = soldierP.GetComponent<SoldierP>();	//import protivnika a jeho promìnných
		soldierEscript = soldierE.GetComponent<SoldierE>();		//import protivnika a jeho promìnných*/

        army = objectArmyE.GetComponent<UnitScript>();             //propojeni scriptu UniArmy s ProgresScript

        //currHPBase = UnityConfiguration.maxHPBase * (evolutionEnemyS.level + 1);
        /*Queue myQ = new Queue();
        myQ.Enqueue("Hello");
        myQ.Enqueue("Hello2");
        //Debug.Log(myQ.ToArray());
        foreach (string obj in myQ)
        {
            Debug.Log(obj);
        }*/
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


        /*if(level != 4 && !GameScript.isGameOver)
		{
			StartCoroutine(Evolution());
        }*/


        /*if (currHPBase <= 0)
        {
            currHPBase = 0;
        }
        //if (canSpawn == true && currHPBase > 0 && HpScript.currHPBase > 0)
		//{
		//	StartCoroutine(CoolDownArmySpawn());
		//}
        hpBaseBarcurr.fillAmount = Mathf.Lerp(hpBaseBarcurr.fillAmount, currHPBase / (UnityConfiguration.maxHPBase * (level + 1)), 3f * Time.deltaTime);       //kolik mame aktualne, kolik budeme mit, rychlost jak se to bude posouvat nasobeno synchronizovany cas
        Color healthColor = Color.Lerp(Color.red, Color.green, (currHPBase / (UnityConfiguration.maxHPBase * (level + 1))));                                   //nastaveni barev pro hpBar, pokud minHP tak red a pokud maxHP tak green a je to gradian
        hpBaseBarcurr.color = healthColor;                      //zde se aplikuje barva gradianu, podle toho kolik ma hpBar zivotu
        hpEnemyTextShow.text = Mathf.Round(currHPBase).ToString();*/
    }

    private void PickUnit()
    {
        if (!GameScript.isGameOver && isCreatingUnit != true)
        {
            //speedForCreatingUnit = 0f;
            randomPickUnit = Random.Range(0, 2);
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
        }
    }
    private void CreatingUnit()
    {
        if (isCreatingUnit)
        {
            speedForCreatingUnit = Time.deltaTime / waitTime[randomPickUnit];
            timeToCreateUnit = Mathf.Lerp(timeToCreateUnit, timeToCreateUnit + 1f, speedForCreatingUnit);
            if (timeToCreateUnit >= 1 && !GameScript.isGameOver)
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
    /*void UpgradeHp()										//zachova procentuelne hp pri upgradu			//sledovat fungovani
    {
        if (level > 0)
        {
            Debug.Log(currHPBase);
            Debug.Log(UnityConfiguration.maxHPBase * (level-1));
            hpbaseinprocents = currHPBase / (UnityConfiguration.maxHPBase * (level - 1));						//pomoc pri pocitani procent(zde se zjistuje rozdil aktualnich hp a maximalnich, aby se to pak podle procent upravilo v dalsi fazi)
            currHPBase = hpbaseinprocents * (UnityConfiguration.maxHPBase * level);						//vypocita aktualniho poctu hp v novych zivotech
        }
        //yield return currHPBase;
    }*/
}
