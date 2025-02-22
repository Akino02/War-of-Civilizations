using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    UnitScript unitData;

    private Rigidbody2D rb;

    private int teamInt => (int)unitData.team;

    //public LayerMask GroundLayer;

    private void Awake()
    {
        unitData = GetComponent<UnitScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForCollision();
        Move();
    }
    private void CheckForCollision()
    {
        float borderObject = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        unitData.distanceFromAllie = new Vector3(transform.position.x + (UnityConfiguration.ranges[2] * 1.25f) * unitData.moveDir[teamInt] + borderObject * unitData.moveDir[teamInt], transform.position.y, transform.position.y);

        //zda vidi enemy tak stoji
        unitData.checkCollision[0] = Physics2D.OverlapCircle(transform.position, unitData.unitRange, unitData.opponent);
        //zda vidi nepratelskou zakladnu
        unitData.checkCollision[1] = Physics2D.OverlapCircle(transform.position, unitData.unitRange, unitData.opponentBase);
        //zda vidi spojence tak se zastavi (je urceno pro vsechny)
        unitData.checkCollision[2] = Physics2D.OverlapCircle(unitData.distanceFromAllie, 0.09f, unitData.allies);

    }
    private void Move()
    {
        //pokud vojacek narazi na jakoukoliv kolizi tak se zastavi (enemy, enemy base, allies)
        if (unitData.checkCollision[0] || unitData.checkCollision[1] || unitData.checkCollision[2] || GameScript.isGameOver)
        {
            //nebude se hybat pokud je poblic kolize
            rb.velocity = new Vector2((unitData.movespeed * unitData.moveDir[2]), rb.velocity.y);

            //dosazeni za promennou speed, ktera urcuje animace
            unitData.animator.SetFloat("Speed", 0);

            /*if (GameScript.isGameOver)
            {
                CelebrateEndOfGame();
            }*/
        }
        //pokud nebude zadna kolize tak bude chodit
        else
        {
            rb.velocity = new Vector2((unitData.movespeed * unitData.moveDir[teamInt]), rb.velocity.y);

            //dosazeni za promennou speed, ktera urcuje animace
            unitData.animator.SetFloat("Speed", rb.velocity.x * unitData.moveDir[teamInt]);
        }
    }

    /*private void CelebrateEndOfGame()
    {
        if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 1), 1f, GroundLayer))
        {
            rb.AddForce(Vector2.up * 0.05f, ForceMode2D.Impulse);
            //Debug.Log("Jump");
        }
    }*/
}
