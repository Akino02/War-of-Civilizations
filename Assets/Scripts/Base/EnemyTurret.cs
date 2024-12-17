using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    Turret towerS;
    HpScript hpEnemyS;
    EvolutionEnemyScript evolutionEnemyS;

    public int timeForCheck = 5;
    public float barForCheck = 0f;
    public float lastHPValue = 0;
    public int lastLevelValue = 0;

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
    }

    // Update is called once per frame
    void Update()
    {
        CheckHPDiff();
    }
    private void CheckHPDiff()
    {
        barForCheck += Time.deltaTime/ timeForCheck;
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
}
