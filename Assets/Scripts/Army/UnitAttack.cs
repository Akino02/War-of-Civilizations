using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Random = UnityEngine.Random;                              //importovani random

public class UnitAttack : MonoBehaviour
{
    UnitScript unitData;

    HpScript hpPlayerS;
    HpScript hpEnemyS;

    //importovani scriptu, ktery bude slouzit pro vojacka, aby si nasel nepritele
    private UnitScript SoldierArmyScript;
    //import scriptu protivnika
    UnitScript armyScriptForOpponent;

    private void Awake()
    {
        unitData = GetComponent<UnitScript>();

        //toto najde zakladnu hrace pomoci tagu ktery ma
        GameObject script1 = GameObject.FindWithTag("baseP");
        hpPlayerS = script1.GetComponent<HpScript>();

        //toto najde zakladnu nepritele pomoci tagu ktery ma
        GameObject script2 = GameObject.FindWithTag("baseE");
        hpEnemyS = script2.GetComponent<HpScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectEnemy();
        //var foundEnemyUnit = DetectEnemy();

        /*if (foundEnemyUnit is not null)
        {
            ChargeAttack(foundEnemyUnit);
        }*/
        if (unitData.foundEnemy || unitData.checkCollision[1] && !GameScript.isGameOver)
        {
            ChargeAttack();
            //animator.SetBool("ScriptFound", true);
        }
        else
        {
            unitData.chargeAttack = 0f;
            unitData.animator.SetBool("ScriptFound", false);
        }
    }
    public void DetectEnemy()
    {
        //Nalezne nepritele a dosadi script za konkretni objekt
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(transform.position, unitData.unitRange, unitData.opponent);
        if (detectedObjects.Length > 0)
        {
            float distance = 5f;
            for (int i = 0; i < detectedObjects.Length; i++)
            {
                if (Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(detectedObjects[i].transform.position.x)) < distance)
                {
                    distance = Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(detectedObjects[i].transform.position.x));
                    SoldierArmyScript = detectedObjects[i].GetComponent<UnitScript>();
                    armyScriptForOpponent = SoldierArmyScript;
                }
            }

            unitData.foundEnemy = true;
            return;
        }

        unitData.foundEnemy = false;
        return;
    }
    private void ChargeAttack()
    {
        unitData.animator.SetBool("ScriptFound", true);
        unitData.chargeAttack = unitData.chargeAttack + Time.deltaTime;


        if (unitData.chargeAttack >= UnityConfiguration.attackDelay)      //pri dead animaci dat do podminky isDead
        {
            unitData.chargeAttack = 0;
            unitData.animator.SetBool("ScriptFound", true);
            int randomDmg = Random.Range(UnityConfiguration.dmgMin[unitData.armyTypeNum] * (unitData.lvl + 1), UnityConfiguration.dmgMax[unitData.armyTypeNum] * (unitData.lvl + 1));
            PlaySFX();
            if (unitData.checkCollision[0] && unitData.foundEnemy == true)
            {
                armyScriptForOpponent.currhp -= randomDmg;
            }
            else if (unitData.checkCollision[1])
            {
                if (unitData.team == 0)
                {
                    //Debug.Log($"Hit base to E: {hpEnemyS.currHPBase}");
                    hpEnemyS.currHPBase -= randomDmg;
                }
                else
                {
                    //Debug.Log($"Hit base to P: {hpPlayerS.currHPBase}");
                    hpPlayerS.currHPBase -= randomDmg;
                }
            }
            unitData.animator.SetBool("ScriptFound", false);
            //attackSound.Stop();
            //Debug.Log("Dohrala se animece a uderil");
        }
    }

    //funkce pro prehrani zvuku pri udereni zbrani
    public void PlaySFX()
    {
        if (gameObject != null || unitData.attackSound != null)
        {
            unitData.attackSound.Play();
        }
        //pro oddeleni audia od objektu
        //AudioSource.PlayClipAtPoint(attackSound.clip, transform.position);
    }
}
