using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgresScript : MonoBehaviour
{
	ButtonScript buttonS;
	EvolutionPlayerScript evolutionS;
	HpScript hpPlayerS;

    //importovani pro pracovani s vojacky
    UnitScript army;
    //objekt pro propojeni scriptu
    public GameObject objectArmyP;

    //funkce zakladny ukazuje rychlost plneni, zkusenosti(%), pocet penez, co se vyrabi, co je ve fronte
    [Header("Base stats")]
    //public float speedOfFill = 3f;

    //penize
    public int money = 175;		
    //kolik jich vyrabime
    public int order = 0;
	//public int made = 0;

    //poradi jednotek							//mohlo by to byt pres queue
    public int[] orderv2 = { 0, 0, 0, 0, 0 };


    [Header("Text")]
    //Prehled kolik ma hrac financi
    public Text moneyText;
    //tento text se bude prepisovat podle toho co se vyrabi
    public Text trainText;

    [Header("Spawner")]
    //misto kde se tyto objekty spawnou
    public GameObject playerSpawner;

	//vyrobnik v procentech graficky
	[Header("Crafting")]
	public Image progBar;

    //vyroba soldiera, rangera, tanka
    private int[] waitTime = { 5, 8, 10 };
    //kolik bude vyplnovat v progbaru
    private float speedOfBar = 0f;
    //zda muze vyrabet
    public bool canProduce = true;

	public Image[] orderVizual = new Image[UnityConstants.queueSize];
    //obrazky pro vizualni order (sprites)
    public Sprite[] unitProduced = new Sprite[UnityConstants.numberOfProductionButtons];

    private void Awake()
    {
        //propojeni zakladnich scriptu pro funkci UI
        buttonS = GetComponent<ButtonScript>();
        hpPlayerS = GetComponent<HpScript>();
        evolutionS = GetComponent<EvolutionPlayerScript>();

        // propojeni scriptu UniArmy s ProgresScript
        army = objectArmyP.GetComponent<UnitScript>();
    }
    // Start is called before the first frame update
    void Start()
	{
        //nastaveni aktualnich penez
        moneyText.text = money.ToString();

        //zapise se co se vyrabi
        TrainingText();
    }

	// Update is called once per frame
	void Update()
	{
        //graficke videni fronty
        OrderView();

        //opakovatelne se budou vpisovat penize do textu
        moneyText.text = money.ToString();

        //zacne se produkce jakmile bude neco v rade a taky se zacne hybat progbar
        if (order > 0 && !GameScript.isGameOver)
		{
			OrderFactory();
		}
    }
	private void OrderFactory()
	{
		if (order > 0 && progBar.fillAmount != 1f && canProduce == true)
		{
			canProduce = false;
            //zacne se psat co se vyrabi
            TrainingText();
            //podle toho se urci co se bude vyrabet a jak dlouho pomoci arraye
            speedOfBar = (Time.deltaTime / waitTime[orderv2[0] - 1]);
			//Debug.Log(progbarfill);
			progBar.fillAmount = Mathf.Lerp(progBar.fillAmount, progBar.fillAmount + 1f, speedOfBar);      //min, max, speed
            canProduce = true;
			if (progBar.fillAmount >= 1f)
			{
				progBar.fillAmount = 0f;
				for (int unitType = 1; unitType <= army.armyTypeLayer.Length; unitType++)
				{
					if (orderv2[0] == unitType)
					{
                        army.armyType = army.armyTypeLayer[orderv2[0]-1];
						//UnitStats unitStats = new UnitStats.Factory(level, true).createSoldier();
                        Instantiate(buttonS.soldierP, playerSpawner.transform.position, playerSpawner.transform.rotation);
                        //Debug.Log("Byl vyroben " + army.armyTypeNum);
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
            speedOfBar = 0;
			progBar.fillAmount = speedOfBar;
            //zapise se viditelne ze se nic nevyrabi (ve forme textu)
            TrainingText();
		}
    }
    //toto zajistuje vizualni frontu vyroby jednotek
    private void OrderView()
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
    //toto serazuje array podle toho co je na rade ve vyrobe		//mohlo byt nahrazeno queue
    void OrderSorter()
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
    //slouzi proto, aby clovek videl co se prave vyrabi
    void TrainingText()
	{
        //vepisuje se text do boxu co se prave vyrabi
        for (int i = 0; i < 4; i++)
		{
			if (orderv2[0] == i)
			{
				trainText.text = UnityConfiguration.trainingTextShow[i];
			}
		}
	}
}