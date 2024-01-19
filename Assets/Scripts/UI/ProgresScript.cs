using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgresScript : MonoBehaviour
{
	ButtonScript buttonS;
	HpScript hpS;

	UniArmy army;                                               //importovani pro pracovani s vojacky
	public GameObject objectArmyP;								//objekt pro propojeni scriptu

	//funkce zakladny ukazuje zkusenosti(%), pocet penez, co se vyrabi, co je ve fronte
	public int experience = 0;									//zkusenosti
	public int experienceinprocents = 0;						//zkusenosti
	public int nextlevelup = 4000;                              //pokud dosahne tolika zkusenosti tak se evolvuje			//potrebuje i prenastavit v unity!!
	public int level = 0;                                       //toto ukazuje level evoluce v zakladu je to 0
	public int money = 175;                                     //penize			
	public int order = 0;                                       //kolik jich vyrabime   //udìlat poudìji jako array, protoze bude vyrabet vice jednotek
	public int made = 0;
	public int[] orderv2 = { 0, 0, 0, 0, 0 };                   //poradi jednotek
	public GameObject[] baseAppearance = new GameObject[4];     //vzhled budov v array ohledne nove evoluce

	public Text experienceText;                                 //Prehled ohledne dalsi evoluce v %
	public Text moneyText;                                      //Prehled kolik ma hrac financi

    public GameObject playerSpawner;                            //misto kde se tyto objekty spawnou


    //vyrobnik v procentech graficky
    public Image progBar;
	public int[,] moneyperunit = { { 15, 25, 100 }, { 30, 50, 200 }, { 60, 100, 400 }, { 120, 200, 800 }, { 240, 400, 1600 } };		//vícerozmìrné pole pro cenu jednotek	//potrebuje upravu
	private int[] waitTime = { 5, 8, 10 };                      //vyroba soldiera, rangera, tanka
	public float progbarfill = 0f;								//kolik bude vyplnovat v progbaru
	//public float timer = 0;
	public bool canProduce = true;                              //zda muze vyrabet
	public Text[] actionButtonText = new Text[3];               //upraveni textu u buttonu soldier, ranger, tank		//pozdeji budou jeste dalsi dva na dobrovolny update evoluce a pro pohromy

	public Image[] orderVizual = new Image[5];

	public Text trainText;                                      //tento text se bude prepisovat podle toho co se vyrabi
																//

	// Start is called before the first frame update
	void Start()
	{
		buttonS = GetComponent<ButtonScript>();     //propojeni zakladnich scriptu pro funkci UI
		hpS = GetComponent<HpScript>();				//propojeni zakladnich scriptu pro funkci UI

		army = objectArmyP.GetComponent<UniArmy>();				//propojeni scriptu UniArmy s ProgresScript
                                                                //nastaveni aktualnich penez
        moneyText.text = money.ToString();
		experienceText.text = experienceinprocents.ToString() + "%";
		StartCoroutine(TrainingText());                         //zapise se co se vyrabi
	}

	// Update is called once per frame
	void Update()
	{
		OrderView();                            //graficke videni fronty
		experienceinprocents = ((100 * experience) / nextlevelup);				//vytvori proceznta ze zkusenosti
		moneyText.text = money.ToString();                                      //opakovatelne se budou vpisovat penize do textu
		StartCoroutine(Evolution());                                            //funkce pro vylepsovani urovne doby
		if (order > 0 && hpS.currHPBase != 0)                       //zacne se produkce jakmile bude neco v rade a taky se zacne hybat progbar
		{
			//StartCoroutine(Orderfactory());
			OrderFactory();
        }
	}
	/*IEnumerator Orderfactory()                                  //bude vyrabet jednoho 5s           //pak udelat na if (aby se menil ten vyrobni cas)              // pozdeji udelat smooth
	{
		if (order > 0 && progbarinprocents != 1f && canProduce == true)
		{
			canProduce = false;
			timer += 1;                                         //je to trosicku opozdene, ale nevadi
			StartCoroutine(TrainingText());                     //zacne se psat co se vyrabi
			progbarinprocents = (timer / waitTime[orderv2[0] - 1]);        //podle toho se urci co se bude vyrabet a jak dlouho pomoci arraye
            //progBar.fillAmount = progbarinprocents;
            yield return new WaitForSecondsRealtime(1);
            progBar.fillAmount = Mathf.Lerp(progBar.fillAmount, progbarinprocents, progbarinprocents);
			//yield return new WaitForSecondsRealtime(1);
			canProduce = true;
			if (progbarinprocents == 1f)
			{
				made += 1;
				timer = 0;
				progbarinprocents = 0f;
				if (orderv2[0] == 1)
				{
					Instantiate(baseS.soldierP, baseS.playerSpawner.transform.position, baseS.playerSpawner.transform.rotation);
					Debug.Log("Byl vyroben Soldier");
				}
				else if (orderv2[0] == 2)
				{
					Instantiate(baseS.rangerP, baseS.playerSpawner.transform.position, baseS.playerSpawner.transform.rotation);
				}
				else if (orderv2[0] == 3)
				{
					Instantiate(baseS.tankP, baseS.playerSpawner.transform.position, baseS.playerSpawner.transform.rotation);
				}
				Debug.Log("Byl vyroben " + order);
				order -= 1;
				if (order >= 0)
				{
					StartCoroutine(OrderSorter());
				}
			}
		}
		if (order == 0)
		{
			timer = 0;
			progbarinprocents = ((100 * timer) / waitTime[orderv2[0]]) / 100;
			progBar.fillAmount = progbarinprocents;
			StartCoroutine(TrainingText());                     //zapise se viditelne ze se nic nevyrabi
		}
	}*/
	private void OrderFactory()									//potrebuje celkove sledovani **********
	{
        if (order > 0 && progBar.fillAmount != 1f && canProduce == true)
        {
            canProduce = false;
            //timer += 1;                                         //je to trosicku opozdene, ale nevadi
            StartCoroutine(TrainingText());                     //zacne se psat co se vyrabi
            progbarfill = (Time.deltaTime / waitTime[orderv2[0] - 1]);        //podle toho se urci co se bude vyrabet a jak dlouho pomoci arraye
            //progBar.fillAmount = progbarinprocents;
            //yield return new WaitForSecondsRealtime(1);
            progBar.fillAmount = Mathf.Lerp(progBar.fillAmount, progBar.fillAmount + 1f, progbarfill);      //min, max, speed
                                                                                                            //yield return new WaitForSecondsRealtime(1);
            canProduce = true;
            if (progBar.fillAmount >= 1f)
            {
                made += 1;
                //timer = 0;
                progBar.fillAmount = 0f;
                if (orderv2[0] == 1)
                {
					army.armyType = army.soldier;
                    Instantiate(buttonS.soldierP, playerSpawner.transform.position, playerSpawner.transform.rotation);
                    Debug.Log("Byl vyroben Soldier");
                }
                else if (orderv2[0] == 2)
                {
                    army.armyType = army.ranger;
                    Instantiate(buttonS.soldierP, playerSpawner.transform.position, playerSpawner.transform.rotation);
                }
                else if (orderv2[0] == 3)
                {
                    army.armyType = army.tank;
                    Instantiate(buttonS.soldierP, playerSpawner.transform.position, playerSpawner.transform.rotation);
                }
                //Debug.Log("Byl vyroben " + order);
                order -= 1;
                if (order >= 0)
                {
                    StartCoroutine(OrderSorter());
                }
            }
        }
        if (order == 0)
        {
            //timer = 0;
            //progbarinprocents = ((100 * timer) / waitTime[orderv2[0]]) / 100;
            progbarfill = 0;
            progBar.fillAmount = progbarfill;
            StartCoroutine(TrainingText());                     //zapise se viditelne ze se nic nevyrabi
        }
    }
	private void OrderView()                                    //toto zajistuje vizualni frontu vyroby jednotek
	{
		for (int i = 0; i < 5; i++)
		{
			if (orderv2[i] == 0)
			{
				orderVizual[i].fillAmount = 0;
			}
			else
			{
				orderVizual[i].fillAmount = 1;
			}
		}
	}
	IEnumerator OrderSorter()                                   //toto serazuje array podle toho co je na rade ve vyrobe
	{
		for (int i = 0; i < 5; i++)
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
		yield return order;
	}
	IEnumerator TrainingText()                                  //slouzi proto, aby clovek videl co se prave vyrabi
	{
		string[] trainingTextWrite = { "Nothing...", "Training Soldier...", "Training Ranger...", "Training Tank..." };
		for (int i = 0; i < 4; i++)
		{
			if (orderv2[0] == i)
			{
				trainText.text = trainingTextWrite[i];
			}
		}
		yield return new WaitForSeconds(0);
	}
	IEnumerator Evolution()										//docasne dokud neni button tak se to evolvuje automaticky			//radsi sledovat !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	{
		if (experience >= nextlevelup && level != 4)
		{
            experience -= nextlevelup;
			level += 1;
			for (int i = 0; i < 3; i++)                         //pise do vsech textu, ktere jsou uchovany v poli
			{
				actionButtonText[i].text = "lvl." + (level + 1);
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
            StartCoroutine(hpS.UpgradeHp());					//pro vylepseni zivotu s tim, ze se zachova %
        }
		else if(level == 4)
		{
			experienceText.text = "Max";
        }
		else
		{
			experienceText.text = experienceinprocents.ToString() + "%";
		}
        yield return experience;
	}
}
