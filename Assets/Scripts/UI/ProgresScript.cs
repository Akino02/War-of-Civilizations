using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgresScript : MonoBehaviour
{
    //importovani pro pracovani s vojacky
    UnitScript army;
    //objekt pro propojeni scriptu
    public GameObject objectArmyP;

    //penize
    public int money = 175;
    //kolik jich vyrabime stats
    //public int currentUnit = 0;
    //public int made = 0;

    //poradi jednotek
    public Queue<int> orders = new Queue<int>();
    public Queue<int> completedOrders = new Queue<int>();

    public float deployAfter = 0.4f;
    public float delayedDeployBar = 0f;


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

    public bool canDeploy = true;
    public int[] armyToDeploy = new int[UnityConstants.queueSize];

    public Image[] orderVizual = new Image[UnityConstants.queueSize];
    //obrazky pro vizualni order (sprites)
    public Sprite[] unitProduced = new Sprite[UnityConstants.numberOfProductionButtons];

    private void Awake()
    {
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
        if (orders.Count > 0 && !GameScript.isGameOver)
        {
            OrderFactory();
        }

        //pusti do hry jednotky, ktere byly vyrobeny, ale nebyly nasazeny
        if (canDeploy && completedOrders.Count > 0)
        {
            DeployDelayedUnits();
        }
    }
    private void OrderFactory()
    {
        if (orders.Count > 0 && progBar.fillAmount != 1f && canProduce == true)
        {
            canProduce = false;

            //zacne se psat co se vyrabi
            TrainingText();

            //podle toho se urci co se bude vyrabet a jak dlouho pomoci arraye
            speedOfBar = (Time.deltaTime / waitTime[orders.Peek() - 1]);

            //Debug.Log(progbarfill);
            progBar.fillAmount += Mathf.Lerp(0f, 1f, speedOfBar);      //min, max, speed

            canProduce = true;
            if (progBar.fillAmount >= 1f)
            {
                progBar.fillAmount = 0f;

                DeployUnit();
            }
        }
        if (orders.Count == 0)
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
        int placed = 0;
        for (int index = 0; index < UnityConstants.queueSize; index++)
        {
            if (placed < orders.Count)
            {
                foreach (int unit in orders)
                {
                    orderVizual[index].fillAmount = 1;
                    orderVizual[index].sprite = unitProduced[unit - 1];
                    index++;
                    placed++;
                }
            }
            if (orders.Count != UnityConstants.queueSize)
            {
                orderVizual[index].fillAmount = 0;
            }
        }
    }
    private void DeployUnit()
    {
        if (canDeploy && completedOrders.Count == 0)
        {
            army.armyType = army.armyTypeLayer[orders.Dequeue()-1];

            Instantiate(objectArmyP, playerSpawner.transform.position, playerSpawner.transform.rotation);
        }
        else
        {
            completedOrders.Enqueue(orders.Dequeue());
        }
    }

    private void DeployDelayedUnits()
    {
        delayedDeployBar += Time.deltaTime;
        if (delayedDeployBar >= deployAfter)
        {
            delayedDeployBar = 0f;
            army.armyType = army.armyTypeLayer[completedOrders.Dequeue() - 1];
            Instantiate(objectArmyP, playerSpawner.transform.position, playerSpawner.transform.rotation);
        }
    }
    //slouzi proto, aby clovek videl co se prave vyrabi
    void TrainingText()
	{
        //vepisuje se text do boxu co se prave vyrabi
        for (int i = 0; i < 4; i++)
		{
            if (orders.Count > 0)
            {
                if (orders.Peek() == i)
                {
                    trainText.text = UnityConfiguration.trainingTextShow[i];
                }
            }
            else
            {
                trainText.text = UnityConfiguration.trainingTextShow[0];
            }
		}
	}
}