using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    //propojeni zakladnich scriptu pro funkci UI
    ProgresScript progresS;
    DisasterScript DisasterS;
	EvolutionPlayerScript evolutionPlayerS;
	GameScript logS;
    public Turret towerS;

	//co a kde to bude spawnovat
    //To je objekt soldier
	public GameObject soldierP;

    //zjisteni na jaky button kliknul
    private int buttonN = 0;
    public static int specialAttackLevel = 0;

    public int currLevelBase;

    //ohledne turret
    public Text changeLabelofTurretButton;
    public Text changeLabelOfActionButton;

    public GameObject unitBoard;
    public GameObject turretBoard;

    public GameObject changeActiveBoard;
    public Sprite[] imageOfActiveBoard;

    private void Awake()
    {
        //propojeni zakladnich scriptu pro funkci UI
        progresS = GetComponent<ProgresScript>();
        DisasterS = GetComponent<DisasterScript>();
        evolutionPlayerS = GetComponent<EvolutionPlayerScript>();
        logS = GetComponent<GameScript>();

        GameObject towerG = GameObject.FindWithTag("TurretP");
        towerS = towerG.GetComponent<Turret>();
        towerS.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
	{
        
    }

	// Update is called once per frame
	void Update()
	{
        currLevelBase = evolutionPlayerS.level;
        //turret
        changeLabelofTurretButton.text = DetectTurretLabel();
    }
    //to jsou funkce pro cudliky
    //tato funkce na kliknuti spawne jednoho vojaka				PRO SOLDIERA
    public void SoldierSpawn()
	{
		buttonN = 1;
		if (progresS.order < 5 && !GameScript.isGameOver && progresS.money >= UnityConfiguration.moneyperunit[0] * (currLevelBase+1))
		{
            progresS.order += 1;
            progresS.orderv2[progresS.order -1] = 1;
            progresS.money -= UnityConfiguration.moneyperunit[0] * (currLevelBase + 1);
			//Debug.Log("Prirazeno do fronty " + progresS.order);
            //Debug.Log("Cena Soldier: " + progresS.moneyperunit[progresS.level, 0]);
		}
        else if (progresS.money >= UnityConfiguration.moneyperunit[0] * (currLevelBase+1) && progresS.order == 5)
        {
            Warning();
        }
        else
        {
            Warning();
        }
    }
    //tato funkce na kliknuti spawne jednoho vojaka				PRO RANGERA
    public void RangerSpawn()
	{
        buttonN = 2;
        if (progresS.order < 5 && !GameScript.isGameOver && progresS.money >= UnityConfiguration.moneyperunit[1] * (currLevelBase + 1))
		{
            progresS.order += 1;
            progresS.orderv2[progresS.order - 1] = 2;
            progresS.money -= UnityConfiguration.moneyperunit[1] * (currLevelBase + 1);
			//Debug.Log("Prirazeno do fronty " + progresS.order);
            //Debug.Log("Cena Ranger: " + progresS.moneyperunit[progresS.level, 1]);
        }
        else if (progresS.money >= UnityConfiguration.moneyperunit[1] * (currLevelBase + 1) && progresS.order == 5)
        {
            Warning();
        }
        else
        {
            Warning();
        }
    }
    // tato funkce na kliknuti spawne jednoho vojaka			PRO TANK
    public void TankSpawn()
	{
        buttonN = 3;
        if (progresS.order < 5 && !GameScript.isGameOver && progresS.money >= UnityConfiguration.moneyperunit[2] * (currLevelBase + 1))
		{
            progresS.order += 1;
            progresS.orderv2[progresS.order - 1] = 3;
            progresS.money -= UnityConfiguration.moneyperunit[2] * (currLevelBase + 1);
            //Debug.Log("Prirazeno do fronty " + progresS.order);
            //Debug.Log("Cena Tank: " + progresS.moneyperunit[progresS.level, 2]);
        }
        else if (progresS.money >= UnityConfiguration.moneyperunit[2] * (currLevelBase + 1) && progresS.order == 5)
        {
            Warning();
        }
        else
        {
            Warning();
        }
	}
    public void ManageTurret()
    {
        //funkce pro koupeni veze
        if (towerS.gameObject.activeSelf == false && !GameScript.isGameOver && progresS.money >= UnityConfiguration.moneyForTurret * (currLevelBase+1))
        {
            progresS.money -= UnityConfiguration.moneyForTurret * (currLevelBase + 1);
            towerS.lvl = currLevelBase;
            Debug.Log(currLevelBase);
            towerS.gameObject.SetActive(true);
            towerS.isVisible();
            //Debug.Log("You bought new turret");
        }
        //funkce pro prodani veze
        else if (towerS.gameObject.activeSelf == true)
        {
            //hrac dostane zpet 0.75 procent puvodni ceny s tim ze se to jeste zaokrouhli dolu a tu hodnotu dostane
            progresS.money += (int)Mathf.Round((UnityConfiguration.moneyForTurret * (towerS.lvl + 1)) * UnityConstants.getMoneyBackPercentage);
            towerS.gameObject.SetActive(false);
        }
        //Debug.Log("If you want to upgrade turret you must sold it and bought new one");
        //Debug.Log("You dont have turret so you cant sell it");
    }
    public void ChangeActionBoard()
    {
        if (unitBoard.activeSelf == true)
        {
            unitBoard.SetActive(false);
            turretBoard.SetActive(true);
            changeActiveBoard.GetComponent<Image>().sprite = imageOfActiveBoard[1];
            changeLabelOfActionButton.text = UnityConfiguration.buttonLabelChangeActionBoard[1];
        }
        else
        {
            unitBoard.SetActive(true);
            turretBoard.SetActive(false);
            changeActiveBoard.GetComponent<Image>().sprite = imageOfActiveBoard[0];
            changeLabelOfActionButton.text = UnityConfiguration.buttonLabelChangeActionBoard[0];
        }
    }
    /*public void DeleteTurret()
    {
        if(towerS.gameObject.activeSelf == true)
        {
            //hrac dostane zpet 0.75 procent puvodni ceny s tim ze se to jeste zaokrouhli dolu a tu hodnotu dostane
            progresS.money += (int)Mathf.Round((UnityConfiguration.moneyForTurret * (towerS.lvl + 1))*UnityConstants.getMoneyBackPercentage);
            towerS.gameObject.SetActive(false);
            //Debug.Log("You sold a turret");
        }
        else
        {
            //Debug.Log("You dont have turret so you cant sell it");
        }
    }*/

    private string DetectTurretLabel()
    {
        //ternarni operator pro ziskani textu zda muze prodat nebo koupit vez
        return towerS.gameObject.activeSelf == true ? UnityConfiguration.buttonLabelTurret[1] : UnityConfiguration.buttonLabelTurret[0];
    }

    public void Warning()
	{
		if (progresS.money < UnityConfiguration.moneyperunit[buttonN-1] * (currLevelBase + 1) && logS.canShow && !GameScript.isGameOver)
		{
            //Debug.Log("Nemas Dostatek penez");
            logS.placeText.text = UnityConfiguration.possibleText[0];
            StartCoroutine(logS.ShowText());
        }
		if (progresS.order == 5 && logS.canShow)
		{
            //Debug.Log("Fronta je plna " + progresS.order);
            logS.placeText.text = UnityConfiguration.possibleText[1];
            StartCoroutine(logS.ShowText());
        }
	}

    //katastrofa
    public void Disaster()
    {
        if (DisasterS.canDoDisaster && !GameScript.isGameOver)
        {
            specialAttackLevel = currLevelBase;
            DisasterS.canDoDisaster = false;
            StartCoroutine(DisasterS.SpawnFireBall());
        }
        else
        {
            //Debug.Log("You must wait until next attack");
            logS.placeText.text = UnityConfiguration.possibleText[5];
            StartCoroutine(logS.ShowText());
        }
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
