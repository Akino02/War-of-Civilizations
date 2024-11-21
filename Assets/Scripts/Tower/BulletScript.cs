using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    UnitScript enemyS;
    Tower towerS;

    private bool canHit;

    Rigidbody2D rb;

    //public LayerMask allieMask;
    public LayerMask enemyMask;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject towerG = GameObject.FindWithTag("Turret");
        towerS = towerG.GetComponent<Tower>();

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
        if (!hitBox.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
        if (!canHit)
        {
            Destroy(gameObject);
        }
        enemyS = hitBox.GetComponent<UnitScript>();
        enemyS.currhp -= towerS.bulletDamage;
        canHit = false;
    }
    void DestroyBullet()
    {
        float distanceFromTurret = Vector3.Distance(towerS.transform.position, transform.position);
        if (distanceFromTurret >= towerS.bulletDistance) Destroy(gameObject);
    }
}
