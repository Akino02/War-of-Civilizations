using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private UnitScript SoldierArmyScript;

    UnitScript armyScriptForOpponent;

    public int waitForCoolDown = 3;
    public float waitingBar = 0f;

    public bool foundEnemy = false;
    public bool canAttack = true;
    public bool isRotated = false;

    public int damage;
    public float turretRange = 2f;

    public float defaultGunRotation = 0f;
    public float turretRotatingSpeed = 3f;
    public Transform rotatingGun;

    public LayerMask opponentLayer;

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
        RotateGun();
    }
    public void DetectEnemy()
    {
        //Nalezne nepritele a dosadi script za konkretni objekt
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(transform.position, turretRange, opponentLayer);
        if (detectedObjects.Length > 0)
        {
            float distance = 10f;
            for (int i = 0; i < detectedObjects.Length; i++)
            {
                if (Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(detectedObjects[i].transform.position.x)) < distance)
                {
                    distance = Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(detectedObjects[i].transform.position.x));
                    SoldierArmyScript = detectedObjects[i].GetComponent<UnitScript>();
                    armyScriptForOpponent = SoldierArmyScript;
                }
            }
            foundEnemy = true;
            //isRotated = false;
            return;
        }
        foundEnemy = false;
        return;
    }
    private void Attack()
    {
        //maybe projectals
        if (foundEnemy && canAttack && isRotated)       //nefunguje podminka isRotated i kdyz je true
        {
            /*if (isRotated)
            {
                Debug.Log("Hey why can you not start");
            }*/
            canAttack = false;
            armyScriptForOpponent.currhp -= damage;
        }
    }
    private void RotateGun()
    {
        Quaternion targetRotation;
        if (foundEnemy)
        {
            //urceni na kolikaty stupen se musi turreta otocit, aby videla nepritele
            float angle = Mathf.Atan2(armyScriptForOpponent.transform.position.y - rotatingGun.position.y, armyScriptForOpponent.transform.position.x - rotatingGun.position.x) * Mathf.Rad2Deg;

            targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            //rotatingGun.rotation = targetRotation;
        }
        else
        {
            targetRotation = Quaternion.Euler(new Vector3(0f, 0f, defaultGunRotation));
        }

        if(Mathf.Abs(Mathf.Abs(rotatingGun.rotation.z) - Mathf.Abs(targetRotation.z)) <= 0.01f && foundEnemy)
        {
            //Attack();
            isRotated = true;
        }
        else
        {
            isRotated= false;
        }

        //smooth rotate
        rotatingGun.rotation = Quaternion.RotateTowards(rotatingGun.rotation, targetRotation, turretRotatingSpeed * Time.deltaTime);
        /*Debug.Log(rotatingGun.rotation.z);
        Debug.Log(targetRotation.z);*/
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
    private void OnDrawGizmosSelected()     //vykreslí kruh okolo towerky
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, turretRange);
    }
}
