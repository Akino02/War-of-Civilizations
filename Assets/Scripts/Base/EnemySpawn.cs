using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;   //import teto funkce, abych mohl pracovat s UI vecmi v unity enginu

public class EnemySpawn : MonoBehaviour
{
	//import scriptu
        //od hrace
	EvolutionPlayerScript evolutionPlayerS;
        //od enemy (sebe)
	EvolutionEnemyScript evolutionEnemyS;
	HpScript hpEnemyS;

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

	//spawnovani jednotek
	public bool canSpawn = true;
	private int randomPickUnit = 0;
    private int[] countToMakeTankCombo = {0,0};
    private int[] difficulty = { 15, 10, 5};					    //obtiznost hry (ve forme vyroby comba)

    private void Awake()
    {
        GameObject item = GameObject.FindWithTag("baseP");		//toto najde zakladnu hrace pomoci tagu ktery ma
        evolutionPlayerS = item.GetComponent<EvolutionPlayerScript>();          //zde se dosadi script za objekt


        hpEnemyS = GetComponent<HpScript>();
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
        CreatingUnit();
        PickUnit();
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
                //Debug.Log("End of Combo!!");
            }
            else if (countToMakeTankCombo[0] >= difficulty[2])      //je tady nastavene nevyssi difficulty
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
                Instantiate(objectArmyE, transform.position, transform.rotation);
                /*Debug.Log($"Enemy built {randomPickUnit}");
                Debug.Log(randomPickUnit);*/
                countToMakeTankCombo[0]++;
                isCreatingUnit = false;
            }
        }
    }
}
