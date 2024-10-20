using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ButtonScript : MonoBehaviour
{
    //propojeni zakladnich scriptu pro funkci UI
    ProgresScript progresS;
	EvolutionPlayerScript evolutionPlayerS;
	GameScript logS;

	//co a kde to bude spawnovat
    //To je objekt soldier
	public GameObject soldierP;

    //zjisteni na jaky button kliknul
    private int buttonN = 0;
    public static int specialAttackLevel = 0;

    public int currLevelBase;

    private void Awake()
    {
        //propojeni zakladnich scriptu pro funkci UI
        progresS = GetComponent<ProgresScript>();
        evolutionPlayerS = GetComponent<EvolutionPlayerScript>();
        logS = GetComponent<GameScript>();
    }
    // Start is called before the first frame update
    void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
        currLevelBase = evolutionPlayerS.level;
    }
    //to jsou funkce pro cudliky
    //tato funkce na kliknuti spawne jednoho vojaka				PRO SOLDIERA
    public void SoldierSpawn()
	{
		buttonN = 1;
		if (progresS.order < 5 && !GameScript.isGameOver && progresS.money >= UnityConfiguration.moneyperunit[currLevelBase, 0])
		{
            progresS.order += 1;
            progresS.orderv2[progresS.order -1] = 1;
            progresS.money -= UnityConfiguration.moneyperunit[currLevelBase, 0];
			//Debug.Log("Prirazeno do fronty " + progresS.order);
            //Debug.Log("Cena Soldier: " + progresS.moneyperunit[progresS.level, 0]);
		}
        else if (progresS.money >= UnityConfiguration.moneyperunit[currLevelBase, 0] && progresS.order == 5)
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
        if (progresS.order < 5 && !GameScript.isGameOver && progresS.money >= UnityConfiguration.moneyperunit[currLevelBase, 1])
		{
            progresS.order += 1;
            progresS.orderv2[progresS.order - 1] = 2;
            progresS.money -= UnityConfiguration.moneyperunit[currLevelBase, 1];
			//Debug.Log("Prirazeno do fronty " + progresS.order);
            //Debug.Log("Cena Ranger: " + progresS.moneyperunit[progresS.level, 1]);
        }
        else if (progresS.money >= UnityConfiguration.moneyperunit[currLevelBase, 1] && progresS.order == 5)
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
        if (progresS.order < 5 && !GameScript.isGameOver && progresS.money >= UnityConfiguration.moneyperunit[currLevelBase, 2])
		{
            progresS.order += 1;
            progresS.orderv2[progresS.order - 1] = 3;
            progresS.money -= UnityConfiguration.moneyperunit[currLevelBase, 2];
            //Debug.Log("Prirazeno do fronty " + progresS.order);
            //Debug.Log("Cena Tank: " + progresS.moneyperunit[progresS.level, 2]);
        }
        else if (progresS.money >= UnityConfiguration.moneyperunit[currLevelBase, 2] && progresS.order == 5)
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
		if (progresS.money < UnityConfiguration.moneyperunit[currLevelBase, buttonN-1] && logS.canShow && !GameScript.isGameOver)
		{
            Debug.Log("Nemas Dostatek penez");
            logS.placeText.text = UnityConfiguration.possibleText[0];
            StartCoroutine(logS.ShowText());
        }
		if (progresS.order == 5 && logS.canShow)
		{
            Debug.Log("Fronta je plna " + progresS.order);
            logS.placeText.text = UnityConfiguration.possibleText[1];
            StartCoroutine(logS.ShowText());
        }
	}

    //katastrofa
    public void Disaster()
    {
        if (progresS.canDoDisaster && !GameScript.isGameOver)
        {
            specialAttackLevel = currLevelBase;
            progresS.canDoDisaster = false;
            StartCoroutine(progresS.SpawnFireBall());
        }
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
