using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSoldierP : MonoBehaviour
{
    [SerializeField] MoveSoldierE enemy;         //import scriptu protivnika     //špatnì funguje

    public Rigidbody2D rb;              //funkce pro gravitaci
    public LayerMask opponentSoldier;       //layer hracovych jednotek typu soldier
    public LayerMask opponentRanger;       //layer hracovych jednotek typu ranger
    public LayerMask opponentTank;       //layer hracovych jednotek typu tank
    public float movespeed;             //rychlost pohybu objektu

    public int hp = 100;
    public int dmg = 25;
    public bool canGetdmg = true;

    // Start is called before the first frame update
    void Start()
    {   
    }
    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2((movespeed * 1), rb.velocity.y);   //bude se hybyt do leva zatim je to testovaci
        if (Physics2D.OverlapCircle(transform.position, 0.4f, opponentSoldier) != null)
        {
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
            else if (hp > 0 && canGetdmg == true)
            {
                StartCoroutine(dmgdealcooldown());
            }
        }
        else if (Physics2D.OverlapCircle(transform.position, 0.4f, opponentSoldier) != null)
        {
            //tady bude dávat attack ranger této jednotce
        }
        else if(Physics2D.OverlapCircle(transform.position, 0.4f, opponentTank) != null)
        {
            //tady bude dávat attack tank této jednotce
        }
    }

    IEnumerator dmgdealcooldown()
    {
        canGetdmg = false;
        hp = hp - dmg;
        Debug.Log("Player " + hp);
        yield return new WaitForSecondsRealtime(2);
        canGetdmg = true;
    }
}
