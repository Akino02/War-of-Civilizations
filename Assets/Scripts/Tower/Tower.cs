using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private UnitScript SoldierArmyScript;

    public UnitScript armyScriptForOpponent;

    public float fireRate = 0.1f;
    public float waitingBar = 0f;

    public bool foundEnemy = false;
    public bool canAttack = true;
    public bool isRotated = false;

    public float bulletDamage;
    public float bulletSpeed;
    public float turretRange = 2f;
    public float bulletDistance;

    public float defaultGunRotation = 0f;
    public float turretRotatingSpeed = 3f;
    public Transform rotatingGun;

    public Transform target;
    private Transform defaultTurretRotation;

    public LayerMask opponentLayer;

    public GameObject bulletPrefab;
    public GameObject bulletPoss;

    // Start is called before the first frame update
    void Start()
    {
        defaultTurretRotation = transform;
        target = transform;
    }

    // Update is called once per frame
    void Update()
    {
        DetectEnemy();
        Shoot();
        RelodingAttack();
        RotateGun();
        /*if (target == defaultTurretRotation)
        {
            DetectEnemy();
        }

        RotateGun();

        if (!CheckIfTargetIsInRange())
        {
            target = defaultTurretRotation;
        }*/
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
    /*private void DetectEnemy()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, turretRange, (Vector2)transform.position, 0f, opponentLayer);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }

    }*/
    private void Shoot()
    {
        //maybe projectals
        if (foundEnemy && canAttack /*&& isRotated*/)       //nefunguje podminka isRotated i kdyz je true
        {
            Quaternion bulletRotation;
            if (isRotated)
            {
                Debug.Log(rotatingGun.rotation.z*100);
                bulletRotation = Quaternion.Euler(new Vector3(0f, 0f, rotatingGun.rotation.z));
                Instantiate(bulletPrefab, bulletPoss.transform.position, bulletRotation);
            }
            //Debug.Log("Can Shooot");
            canAttack = false;
            //Instantiate(bulletPrefab, new Vector3(1.0f, 0f, 0f), transform.rotation);
            
            
            //armyScriptForOpponent.currhp -= damage;
        }
    }
    /*private bool CheckIfTargetIsInRange()
    {
        return Vector2.Distance(transform.position, target.position) <= turretRange;
    }*/
    private bool CheckIfTurretIsRotated(Quaternion targetRotation)
    {
        return (Mathf.Abs(Mathf.Abs(rotatingGun.rotation.z) - Mathf.Abs(targetRotation.z)) <= 0.01f && foundEnemy);
    }

    private bool RotateGun()
    {
        Quaternion targetRotation;
        if (foundEnemy)
        {
            //urceni na kolikaty stupen se musi turreta otocit, aby videla nepritele
            float angle = Mathf.Atan2(armyScriptForOpponent.transform.position.y - rotatingGun.position.y, armyScriptForOpponent.transform.position.x - rotatingGun.position.x) * Mathf.Rad2Deg;
            //float angle = Mathf.Atan2(target.transform.position.y - rotatingGun.position.y, target.transform.position.x - rotatingGun.position.x) * Mathf.Rad2Deg;

            targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            //rotatingGun.rotation = targetRotation;
        }
        else
        {
            targetRotation = Quaternion.Euler(new Vector3(0f, 0f, defaultGunRotation));
        }

        isRotated = CheckIfTurretIsRotated(targetRotation);

        //smooth rotate
        rotatingGun.rotation = Quaternion.RotateTowards(rotatingGun.rotation, targetRotation, turretRotatingSpeed * Time.deltaTime);
        return isRotated;
    }
    private void RelodingAttack()
    {
        if (!canAttack)
        {
            waitingBar = Mathf.Lerp(waitingBar, waitingBar + 1f, Time.deltaTime / fireRate);
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
