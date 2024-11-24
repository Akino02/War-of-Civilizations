using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    UnitScript opponentS;
    Turret towerS;

    Rigidbody2D rb;

    public GameObject towerG;

    public Team teamBullet;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        towerS = towerG.GetComponent<Turret>();

        teamBullet = towerS.teamTurret;
        /*if (teamBullet == Team.Player)
        {
            GameObject towerG = GameObject.FindWithTag("TurretP");
            towerS = towerG.GetComponent<Turret>();
        }
        else if (teamBullet == Team.Enemy)
        {
            GameObject towerG = GameObject.FindWithTag("TurretE");
            towerS = towerG.GetComponent<Turret>();
        }*/

        //ziskani pozine nepritele
        Vector3 directionOfEnemy = new Vector3(towerS.armyScriptForOpponent.transform.position.x, towerS.armyScriptForOpponent.transform.position.y, towerS.armyScriptForOpponent.transform.position.z);
        

        Vector2 bulletSpeedDirection = new Vector2((towerS.transform.position.x - directionOfEnemy.x) * (-1), (towerS.transform.position.y - directionOfEnemy.y) * (-1)).normalized;
        
        
        //smer vektoru kudy se bude bullet pohybovat
        rb.velocity = new Vector2 (bulletSpeedDirection.x * towerS.bulletSpeed, bulletSpeedDirection.y * towerS.bulletSpeed);

        //urceni rotace streli
        float rotationOfBullet = Mathf.Atan2(bulletSpeedDirection.y, bulletSpeedDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0 ,rotationOfBullet+90f);
    }

    // Update is called once per frame
    void Update()
    {
        DestroyBullet();
    }

    private void OnTriggerEnter2D(Collider2D hitBox)
    {
        opponentS = hitBox.GetComponent<UnitScript>();
        if (opponentS != null)
        {
            if ((teamBullet == Team.Player && hitBox.tag == "Enemy") || (teamBullet == Team.Enemy && hitBox.tag == "Player"))
            {
                opponentS.currhp -= towerS.bulletDamage;
                Destroy(gameObject);
            }
        }
    }
    void DestroyBullet()
    {
        //Pouziti funkce Vector3.Distance, ktera vypocita vzdalenost mezi dvema objekty
        float distanceFromTurret = Vector3.Distance(towerS.transform.position, transform.position);
        if (distanceFromTurret >= towerS.bulletDistance) Destroy(gameObject);
    }
}
