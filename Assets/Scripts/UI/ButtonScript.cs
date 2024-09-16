using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;                                           //import teto funkce, abych mohl pracovat s UI vecmi v unity enginu
using UnityEngine.UIElements;

public class ButtonScript : MonoBehaviour
{
	ProgresScript progresS;										//propojeni zakladnich scriptu pro funkci UI
	HpScript hpS;												//propojeni zakladnich scriptu pro funkci UI
	LogScript logS;												//propojeni zakladnich scriptu pro funkci UI
																//
	//co a kde to bude spawnovat
	public GameObject soldierP;                                 //To je objekt soldier

	private int buttonN = 0;                                    //zjisteni na jaky button kliknul
	//

	// Start is called before the first frame update
	void Start()
	{

        progresS = GetComponent<ProgresScript>();				        //propojeni zakladnich scriptu pro funkci UI
        hpS = GetComponent<HpScript>();							        //propojeni zakladnich scriptu pro funkci UI
        logS = GetComponent<LogScript>();							    //propojeni zakladnich scriptu pro funkci UI
	}

	// Update is called once per frame
	void Update()
	{

	}
	//to jsou funkce pro cudliky
	public void SoldierSpawn()									//tato funkce na kliknuti spawne jednoho vojaka				PRO SOLDIERA
	{
		buttonN = 1;
		if (progresS.order < 5 && !LogScript.isGameOver && ProgresScript.money >= UnityConfiguration.moneyperunit[ProgresScript.level, 0])												//jeste tam pak doplnit ze za to bude platit
		{
            progresS.order += 1;
            progresS.orderv2[progresS.order -1] = 1;
            ProgresScript.money -= UnityConfiguration.moneyperunit[ProgresScript.level,0];
			//Debug.Log("Prirazeno do fronty " + progresS.order);
            //Debug.Log("Cena Soldier: " + progresS.moneyperunit[progresS.level, 0]);
		}
        else if (ProgresScript.money >= UnityConfiguration.moneyperunit[ProgresScript.level, 0] && progresS.order == 5)
        {
            Warning();
        }
        else
        {
            Warning();
        }
    }
	public void RangerSpawn()									//tato funkce na kliknuti spawne jednoho vojaka				PRO RANGERA
	{
        buttonN = 2;
        if (progresS.order < 5 && !LogScript.isGameOver && ProgresScript.money >= UnityConfiguration.moneyperunit[ProgresScript.level, 1])												//jeste tam pak doplnit ze za to bude platit
		{
            progresS.order += 1;
            progresS.orderv2[progresS.order - 1] = 2;
            ProgresScript.money -= UnityConfiguration.moneyperunit[ProgresScript.level, 1];
			//Debug.Log("Prirazeno do fronty " + progresS.order);
            //Debug.Log("Cena Ranger: " + progresS.moneyperunit[progresS.level, 1]);
        }
        else if (ProgresScript.money >= UnityConfiguration.moneyperunit[ProgresScript.level, 1] && progresS.order == 5)
        {
            Warning();
        }
        else
        {
            Warning();
        }
    }
	public void TankSpawn()										// tato funkce na kliknuti spawne jednoho vojaka			PRO TANK
	{
        buttonN = 3;
        if (progresS.order < 5 && !LogScript.isGameOver && ProgresScript.money >= UnityConfiguration.moneyperunit[ProgresScript.level, 2])												//jeste tam pak doplnit ze za to bude platit
		{
            progresS.order += 1;
            progresS.orderv2[progresS.order - 1] = 3;
            ProgresScript.money -= UnityConfiguration.moneyperunit[ProgresScript.level, 2];
            //Debug.Log("Prirazeno do fronty " + progresS.order);
            //Debug.Log("Cena Tank: " + progresS.moneyperunit[progresS.level, 2]);
        }
        else if (ProgresScript.money >= UnityConfiguration.moneyperunit[ProgresScript.level, 2] && progresS.order == 5)
        {
            Warning();
        }
        else
        {
            Warning();
        }
	}
	public void Warning()
	{
		if (ProgresScript.money < UnityConfiguration.moneyperunit[ProgresScript.level, buttonN-1] && logS.canShow && !LogScript.isGameOver)
		{
            Debug.Log("Nemas Dostatek penez");
            logS.placeText.text = logS.possibleText[0];
            StartCoroutine(logS.ShowText());
        }
		if (progresS.order == 5 && logS.canShow)
		{
            Debug.Log("Fronta je plna " + progresS.order);
            logS.placeText.text = logS.possibleText[1];
            StartCoroutine(logS.ShowText());
        }
	}
	/*public void EvolutionUpgrade()							//funkce pro button, ktery bude evolvovat hracovy jednotky a zakladnu	(BUTTON NENI HOTOVY)
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
    public void Disaster()                  //katastrofa
    {
        if (progresS.canDoDisaster && !LogScript.isGameOver)
        {
            progresS.canDoDisaster = false;
            StartCoroutine(progresS.SpawnFireBall());
        }
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
