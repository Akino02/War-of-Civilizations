using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private UnitScript SoldierArmyScript;

    UnitScript armyScriptForOpponent;

    public int damage;
    public int waitForCoolDown = 3;
    public bool canAttack = true;
    public float waitingBar = 0f;
    private float towerRange = 2f;
    public bool foundEnemy = false;
    private float distance;
    public LayerMask opponent;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        DetectEnemy();
        Attack();
        RelodingAttack();
    }
    public void DetectEnemy()                                                                                                               //logika funguje stejne jako u jednotek
    {
        //Nalezne nepritele a dosadi script za konkretni objekt
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(transform.position, towerRange, opponent);
        if (detectedObjects.Length > 0)
        {
            distance = 100f;
            for (int i = 0; i < detectedObjects.Length; i++)
            {
                if (Mathf.Abs(transform.position.x) - Mathf.Abs(detectedObjects[i].transform.position.x) < distance)
                {
                    distance = Mathf.Abs(transform.position.x) - Mathf.Abs(detectedObjects[i].transform.position.x);
                    SoldierArmyScript = detectedObjects[i].GetComponent<UnitScript>();
                    armyScriptForOpponent = SoldierArmyScript;
                }
            }
            foundEnemy = true;
            return;
        }
        foundEnemy = false;
        return;
    }
    private void Attack()
    {
        //maybe projectals
        if (foundEnemy && canAttack)
        {
            canAttack = false;
            //Debug.Log("Tower hit");
            armyScriptForOpponent.currhp -= damage;
        }
    }
    private void RelodingAttack()
    {
        if (!canAttack)
        {
            waitingBar = Mathf.Lerp(waitingBar, waitingBar + 1f, Time.deltaTime / waitForCoolDown);
            if (waitingBar >= 1)
            {
                canAttack = true;
                waitingBar = 0;
            }
        }
    }
}
