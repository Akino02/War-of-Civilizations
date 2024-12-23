using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    Turret towerS;
    HpScript hpEnemyS;
    EvolutionEnemyScript evolutionEnemyS;

    public int timeToCreate;
    private int maxTimeForCreate = 6;
    public float barForCheck = 0f;
    public float lastHPValue = 0;
    public int lastLevelValue = 0;
    public bool gotHit = false;

    private void Awake()
    {
        hpEnemyS = GetComponent<HpScript>();
        evolutionEnemyS = GetComponent<EvolutionEnemyScript>();

        GameObject towerG = GameObject.FindWithTag("TurretE");
        towerS = towerG.GetComponent<Turret>();
        towerS.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        lastHPValue = UnityConfiguration.maxHPBase;

        timeToCreate = RandomizeSpawnOfTurret(maxTimeForCreate);
    }

    // Update is called once per frame
    void Update()
    {
        CheckHPDiff();
    }
    private void CheckHPDiff()
    {
        /*barForCheck += Time.deltaTime/ timeForCheck;
        if (barForCheck >= 1)
        {
            //pokud mu hrac zautoci na nepratelskou base tak nepritel si koupi vez
            //podminka je nastavena tak aby se kontroloval i level
            if(lastHPValue != hpEnemyS.currHPBase && lastLevelValue == evolutionEnemyS.level)
            {
                SpawnTurret();
            }
            lastHPValue = hpEnemyS.currHPBase;
            lastLevelValue = evolutionEnemyS.level;
            barForCheck = 0;
        }*/
        //pokud nepratelska zakladna dostane damage a nebo uz dostala
        if (lastHPValue != hpEnemyS.currHPBase || gotHit == true)
        {
            //pokud se neshoduji hp s kontrolnimi hp
            if(lastLevelValue != evolutionEnemyS.level)
            {
                lastHPValue = hpEnemyS.currHPBase;
                lastLevelValue = evolutionEnemyS.level;
                //pokud dostal hit a ma evoluci zrovna tak se zachova vyroba/vylepseni veze
                barForCheck = gotHit == true ? barForCheck : 0;
                return;
            }
            gotHit = true;
            barForCheck += Time.deltaTime / timeToCreate;
            if (barForCheck >= 1)
            {
                SpawnTurret();
                lastHPValue = hpEnemyS.currHPBase;
                timeToCreate = RandomizeSpawnOfTurret(maxTimeForCreate);
                barForCheck = 0;
                gotHit = false;
            }
        }
    }
    private void SpawnTurret()
    {
        //nasledne pokud vez nebude koupena
        if (towerS.gameObject.activeSelf == false)
        {
            towerS.gameObject.SetActive(true);
        }
        //pokud vez bude koupena tak ji jen vylepsi
        towerS.lvl = evolutionEnemyS.level;
        Debug.Log(evolutionEnemyS.level);
        //nastavi vzhled veze podle urovne
        towerS.isVisible();
    }

    private int RandomizeSpawnOfTurret(int maxTime)
    {
        return Random.Range(1, maxTime);
    }
}
