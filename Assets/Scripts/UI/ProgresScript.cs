using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgresScript : MonoBehaviour
{
	ButtonScript buttonS;
	EvolutionPlayerScript evolutionS;
	//HpScript hpS;
	HpScript hpPlayerS;

    UnitScript army;                                               //importovani pro pracovani s vojacky
	public GameObject objectArmyP;                              //objekt pro propojeni scriptu

    //funkce zakladny ukazuje rychlost plneni, zkusenosti(%), pocet penez, co se vyrabi, co je ve fronte
    [Header("Base stats")]
    //public float speedOfFill = 3f;

	public int money = 175;                              //penize			
	public int order = 0;                                       //kolik jich vyrabime   //udìlat poudìji jako array, protoze bude vyrabet vice jednotek
	public int made = 0;
	public int[] orderv2 = { 0, 0, 0, 0, 0 };                   //poradi jednotek


    /*[Header("Base appearance")]
    public GameObject[] baseAppearance = new GameObject[UnityConstats.MaxLevel];     //vzhled budov v array ohledne nove evoluce*/

    [Header("Text")]
    //public Text experienceText;                                 //Prehled ohledne dalsi evoluce v %
	public Text moneyText;                                      //Prehled kolik ma hrac financi
    public Text trainText;                                      //tento text se bude prepisovat podle toho co se vyrabi

    [Header("Spawner")]
    public GameObject playerSpawner;                            //misto kde se tyto objekty spawnou


	//vyrobnik v procentech graficky
	[Header("Crafting")]
	public Image progBar;
	//public int[,] moneyperunit = { { 15, 25, 100 }, { 30, 50, 200 }, { 60, 100, 400 }, { 120, 200, 800 }, { 240, 400, 1600 } };		//vícerozmìrné pole pro cenu jednotek	//potrebuje upravu
	private int[] waitTime = { 5, 8, 10 };                      //vyroba soldiera, rangera, tanka
	private float speedOfBar = 0f;								//kolik bude vyplnovat v progbaru
	public bool canProduce = true;                              //zda muze vyrabet

	//public Text[] actionButtonText = new Text[UnityConstats.NumberOfProductionButtons*2];               //upraveni textu u buttonu soldier, ranger, tank		//pozdeji budou jeste dalsi dva na dobrovolny update evoluce a pro pohromy

	public Image[] orderVizual = new Image[UnityConstants.queueSize];
    public Sprite[] unitProduced = new Sprite[UnityConstants.numberOfProductionButtons];				//obrazky pro vizualni order (sprites)
    //

    /*//ukazuje zkusenosti graficky
    [Header("XP bar")]
    public Image xpBar;*/

    //Katastrofa
    [Header("Disaster")]
    public GameObject fireBall;									//tohle pak zmenit na array 
	public GameObject borderL;
    public GameObject borderR;
    public GameObject disasterZone;
	private float waitDisasterFill = 0f;
	private int waitingTimeForDisaster = 40;
	public Image waitDisasterFillBox;
    public bool canDoDisaster = true;

    // Start is called before the first frame update
    void Start()
	{
        buttonS = GetComponent<ButtonScript>();					//propojeni zakladnich scriptu pro funkci UI
		hpPlayerS = GetComponent<HpScript>();                          //propojeni zakladnich scriptu pro funkci UI
        evolutionS = GetComponent<EvolutionPlayerScript>();                          //propojeni zakladnich scriptu pro funkci UI

		army = objectArmyP.GetComponent<UnitScript>();				//propojeni scriptu UniArmy s ProgresScript
																//nastaveni aktualnich penez
		moneyText.text = money.ToString();
		//experienceText.text = experienceinprocents.ToString() + "%";
		TrainingText();                         //zapise se co se vyrabi

        /*for (int i = 0; i < 3; i++)								//na zacatku se definuje co tam bude na tom buttonu
        {
            actionButtonText[i].text = "lvl." + (level + 1);
            actionButtonText[i + (actionButtonText.Length) / 2].text = "Cost " + UnityConfiguration.moneyperunit[level, i] + " $";
        }*/
    }

	// Update is called once per frame
	void Update()
	{
		OrderView();                                            //graficke videni fronty
		//ExperienceBar();
		moneyText.text = money.ToString();						//opakovatelne se budou vpisovat penize do textu
		//Evolution();							//funkce pro vylepsovani urovne doby
		if (order > 0 && !GameScript.isGameOver)					//zacne se produkce jakmile bude neco v rade a taky se zacne hybat progbar
		{
			//StartCoroutine(Orderfactory());
			OrderFactory();
		}
		if (!canDoDisaster && !GameScript.isGameOver)
		{
			WaitDisaster();
        }
    }
	private void OrderFactory()									//potrebuje celkove sledovani **********
	{
		if (order > 0 && progBar.fillAmount != 1f && canProduce == true)
		{
			canProduce = false;
			TrainingText();                     //zacne se psat co se vyrabi
			speedOfBar = (Time.deltaTime / waitTime[orderv2[0] - 1]);        //podle toho se urci co se bude vyrabet a jak dlouho pomoci arraye
			//Debug.Log(progbarfill);
			//progBar.fillAmount = progbarinprocents;
			//yield return new WaitForSecondsRealtime(1);
			progBar.fillAmount = Mathf.Lerp(progBar.fillAmount, progBar.fillAmount + 1f, speedOfBar);      //min, max, speed
																											//yield return new WaitForSecondsRealtime(1);
            canProduce = true;
			if (progBar.fillAmount >= 1f)
			{
				made += 1;
				//timer = 0;
				progBar.fillAmount = 0f;
				for (int unitType = 1; unitType <= army.armyTypeLayer.Length; unitType++)
				{
					if (orderv2[0] == unitType)
					{
                        army.armyType = army.armyTypeLayer[orderv2[0]-1];
						//UnitStats unitStats = new UnitStats.Factory(level, true).createSoldier();
                        Instantiate(buttonS.soldierP, playerSpawner.transform.position, playerSpawner.transform.rotation);
                        Debug.Log("Byl vyroben " + army.armyTypeNum);
                    }
				}
				order -= 1;
				if (order >= 0)
				{
					OrderSorter();
				}
			}
        }
		if (order == 0)
		{
            //progbarinprocents = ((100 * timer) / waitTime[orderv2[0]]) / 100;
            speedOfBar = 0;
			progBar.fillAmount = speedOfBar;
			TrainingText();                     //zapise se viditelne ze se nic nevyrabi
		}
    }
	private void OrderView()                                    //toto zajistuje vizualni frontu vyroby jednotek
	{
		for (int i = 0; i < UnityConstants.queueSize; i++)
		{
			if (orderv2[i] != 0)
			{
				orderVizual[i].fillAmount = 1;
				orderVizual[i].sprite = unitProduced[orderv2[i]-1];
				//Debug.Log(orderv2[i]);
            }
			else
			{
				orderVizual[i].fillAmount = 0;
			}
		}
    }
	void OrderSorter()                                   //toto serazuje array podle toho co je na rade ve vyrobe
	{
		for (int i = 0; i < UnityConstants.queueSize; i++)
		{
			if (i != 4)
			{
				orderv2[i] = orderv2[i + 1];
			}
			else
			{
				orderv2[i] = 0;
			}
		}
    }
	void TrainingText()                                  //slouzi proto, aby clovek videl co se prave vyrabi
	{
        for (int i = 0; i < 4; i++)								//vepisuje se text do boxu co se prave vyrabi
		{
			if (orderv2[0] == i)
			{
				trainText.text = UnityConfiguration.trainingTextShow[i];
			}
		}
	}
	/*void Evolution()										//docasne dokud neni button tak se to evolvuje automaticky
	{
		if (experience >= UnityConfiguration.nextlevelup && level != UnityConstats.MaxLevel)			//pokud jeho level neni roven 4 coz je nejvysi uroven tak se muze vylepsit
		{
			experience -= UnityConfiguration.nextlevelup;
			level += 1;
			for (int i = 0; i < actionButtonText.Length/2; i++)                         //pise do vsech textu, ktere jsou uchovany v poli				**mozna pak chyba bude tady
			{
				actionButtonText[i].text = "lvl." + (level + 1);
				actionButtonText[i+(actionButtonText.Length)/2].text = "Cost " + UnityConfiguration.moneyperunit[level, i] + " $";
			}
			for (int i = 0; i < baseAppearance.Length; i++)						//zde se zmeni vzhled zakladny
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
			hpPlayerS.UpgradeHp();					//pro vylepseni zivotu s tim, ze se zachova %
		}
		else if(level == UnityConstats.MaxLevel)
		{
			experienceText.text = "Max";
		}
		else
		{
			experienceText.text = experienceinprocents.ToString() + "%";
		}
	}*/
	/*void ExperienceBar()
	{
		if(level != UnityConstats.MaxLevel)
		{
            experienceinprocents = ((100 * experience) / UnityConfiguration.nextlevelup);              //vytvori proceznta ze zkusenosti
            xpBar.fillAmount = Mathf.Lerp(xpBar.fillAmount, (float)experienceinprocents / 100f, speedOfFill * Time.deltaTime);
        }
		else
		{
			xpBar.fillAmount = 1f;
		}
		//Debug.Log(experience / nextlevelup);
        return;
	}*/
    public IEnumerator SpawnFireBall()
    {
		waitDisasterFill = 1f;
        waitDisasterFillBox.fillAmount = waitDisasterFill;
        WaitDisaster();
        int i = 0;
        while (i <= 30)
        {
            float randomPosX = Random.Range(borderL.transform.position.x, borderR.transform.position.x);								//
			float randomRotZ = Random.Range(0, 360);																					//
            Quaternion changedRotationZ = Quaternion.Euler(disasterZone.transform.rotation.x, 0, randomRotZ);							//
            Vector3 disasterZonePos = new Vector3(randomPosX, disasterZone.transform.position.y, fireBall.transform.position.z);		//
			if (evolutionS.level == 0)
			{
                Instantiate(fireBall, disasterZonePos, changedRotationZ);
            }
			else
			{
                Instantiate(fireBall, disasterZonePos, disasterZone.transform.rotation);
            }
            yield return new WaitForSeconds(0.4f);
            i += 1;
        }
		yield return null;
    }

	public void WaitDisaster()
	{
        if (waitDisasterFillBox.fillAmount > 0)
		{
            waitDisasterFill = (Time.deltaTime / waitingTimeForDisaster);				//definice rychlosti klesani (cas snimku/celkova doba cekani)
            waitDisasterFillBox.fillAmount = Mathf.Lerp(waitDisasterFillBox.fillAmount, waitDisasterFillBox.fillAmount -1f, waitDisasterFill);		//min, max, speed
        }
		else													//pokud je to rovno nule ci mensi 
		{
			waitDisasterFill = 0;
			waitDisasterFillBox.fillAmount = waitDisasterFill;
			canDoDisaster = true;
        }
		return;
    }
}