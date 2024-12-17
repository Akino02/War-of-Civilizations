using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private EvolutionPlayerScript evolutionPlayerS;
    private EvolutionEnemyScript evolutionEnemyS;

    private UnitScript SoldierArmyScript;

    public UnitScript armyScriptForOpponent;

    public BulletScript bulletS;

    public int lvl = 0;

    //public float fireRate = 0.1f;
    public float waitingBar = 0f;

    public bool foundEnemy = false;
    public bool canAttack = true;
    public bool isRotated = false;

    public float bulletDamage;
    public float bulletSpeed;
    public float turretRange;
    public float bulletDistance;

    public float defaultGunRotation = 0f;
    public float turretRotatingSpeed = 3f;
    public Transform rotatingGun;

    public Transform target;
    private Transform defaultTurretRotation;

    /*public Sprite[] Body;
    public Sprite[] Gun;
    public Sprite[] Bullet;*/
    public Animator animatorTurrBody;
    public Animator animatorTurrGun;

    public LayerMask opponentLayer;

    public GameObject bulletPrefab;
    public GameObject bulletPoss;

    public Team teamTurret;

    private void Awake()
    {
        GameObject objectOfScriptP = GameObject.FindWithTag("baseP");
        evolutionPlayerS = objectOfScriptP.GetComponent<EvolutionPlayerScript>();

        //toto najde zakladnu nepritele pomoci tagu ktery ma
        GameObject objectOfScriptE = GameObject.FindWithTag("baseE");
        evolutionEnemyS = objectOfScriptE.GetComponent<EvolutionEnemyScript>();

    }
    // Start is called before the first frame update
    void Start()
    {
        defaultTurretRotation = transform;
        target = transform;
        //nastaveni lvl podle zakladny pri startu hry
        lvl = evolutionPlayerS.level;

        //nastaveni hodnot
        bulletDamage = UnityConfiguration.bulletDamage * (lvl+1);
        bulletSpeed = UnityConfiguration.bulletSpeed * (lvl + 1);
        turretRange = UnityConfiguration.turretRange;
        bulletDistance = UnityConfiguration.bulletDistance * (lvl + 1);
}

    // Update is called once per frame
    void Update()
    {
        DetectEnemy();
        Shoot();
        RelodingAttack();
        isRotated = RotateGun();

        //isVisible();
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
            return;
        }
        foundEnemy = false;
        return;
    }
    private void Shoot()
    {
        if (foundEnemy && canAttack)
        {
            Quaternion bulletRotation;
            if (isRotated)
            {
                bulletRotation = Quaternion.Euler(new Vector3(0f, 0f, rotatingGun.rotation.z));
                GameObject bullet = Instantiate(bulletPrefab, bulletPoss.transform.position, bulletRotation);
                bulletS = bullet.GetComponent<BulletScript>();
                bulletS.towerG = gameObject;
                bulletS.animatorBullet.SetInteger("Level", lvl);
            }
            canAttack = false;
        }
    }
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
            waitingBar += Time.deltaTime;
            //waitingBar = Mathf.Lerp(waitingBar, waitingBar + 1f, Time.deltaTime / fireRate);
            if (waitingBar >= UnityConfiguration.fireRate)
            {
                canAttack = true;
                waitingBar = 0;
            }
        }
    }
    public void isVisible()
    {
        animatorTurrBody.SetInteger("Level", lvl);
        animatorTurrGun.SetInteger("Level", lvl);
    }
    private void OnDrawGizmosSelected()     //vykreslí kruh okolo towerky
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, turretRange);
    }
}