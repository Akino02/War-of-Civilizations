using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;   //import teto funkce, abych mohl pracovat s UI vecmi v unity enginu
using Random = UnityEngine.Random;

public class EnemySpawn : MonoBehaviour
{
	//import scriptu
        //od hrace
	//EvolutionPlayerScript evolutionPlayerS;
        //od enemy (sebe)
	EvolutionEnemyScript evolutionEnemyS;
	//HpScript hpEnemyS;

    //importovani scriptu jednotky
    UnitScript army;

    //objekt pro propojeni scriptu
    public GameObject objectArmyE;

    //Factory (Unit)
    //soldier, ranger, tank, cant Build
    private int[] waitTime = { 6, 9, 13};
    public float timeToCreateUnit = 0f;
    private float speedForCreatingUnit = 0;
    public bool isCreatingUnit = false;

    public bool scrapUnit = false;

	//spawnovani jednotek
	public bool canSpawn = true;
	private int randomPickUnit = 0;
    private int[] countToMakeTankCombo = {0,0};

    public static int currDifficulty;

    private void Awake()
    {
        //GameObject item = GameObject.FindWithTag("baseP");		//toto najde zakladnu hrace pomoci tagu ktery ma
        //evolutionPlayerS = item.GetComponent<EvolutionPlayerScript>();          //zde se dosadi script za objekt


        //hpEnemyS = GetComponent<HpScript>();
        evolutionEnemyS = GetComponent<EvolutionEnemyScript>();

        army = objectArmyE.GetComponent<UnitScript>();             //propojeni scriptu UniArmy s ProgresScript
    }
    // Start is called before the first frame update
    void Start()
	{
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
        if (canSpawn) CreatingUnit();
        PickUnit();
    }

    private void PickUnit()
    {
        if (!GameScript.isGameOver && isCreatingUnit != true)
        {
            //speedForCreatingUnit = 0f;
            randomPickUnit = Random.Range(0, UnityConstants.numberOfUnitsIndex);        //muze se tam dat +1 aby vyrabel i tanky
            //give help with random scrap unity
            if (Random.Range(0, 4)%2==0) scrapUnit = true;
            else scrapUnit = false;

            //pick combo
            if (countToMakeTankCombo[1] >= 1)
            {
                countToMakeTankCombo[1] = 0;
                randomPickUnit = 1;
                //Debug.Log("End of Combo!!");
            }
            else if (countToMakeTankCombo[0] >= currDifficulty)      //je tady nastavene nevyssi difficulty
            {
                countToMakeTankCombo[0] = 0;
                countToMakeTankCombo[1] = 1;
                randomPickUnit = 2;
                //Debug.Log("Combo started !!!!!");
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
                if (evolutionEnemyS.level < 4) Instantiate(objectArmyE, transform.position, transform.rotation);
                else if (evolutionEnemyS.level >= 4 && scrapUnit) Debug.Log("Unit was scraped");
                /*Debug.Log($"Enemy built {randomPickUnit}");
                Debug.Log(randomPickUnit);*/
                countToMakeTankCombo[0]++;
                isCreatingUnit = false;
            }
        }
    }
}