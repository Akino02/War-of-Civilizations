using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierP : MonoBehaviour
{
    SoldierE soldierEscript;                       //import scriptu protivnika
    [SerializeField] GameObject soldierE;

    public Rigidbody2D rb;              //funkce pro gravitaci
    public LayerMask opponentSoldier;       //layer hracovych jednotek typu soldier
    public LayerMask opponentRanger;       //layer hracovych jednotek typu ranger
    public LayerMask opponentTank;       //layer hracovych jednotek typu tank
    public float movespeed;             //rychlost pohybu objektu

    //Ohledne HPbaru
    public GameObject hpBar;

    private float hp = 100;
    public float currhp = 100;
    private float hpinprocents = 1f;

    //Ohledne utoku
    public float dmg = 50; //sila teto postavy
    public bool canGetdmg = true;

    // Start is called before the first frame update
    void Start()
    {
        soldierEscript = soldierE.GetComponent<SoldierE>();  //import protivnika a jeho promìnných
    }
    // Update is called once per frame
    void Update()
    {
        hpinprocents = ((100 * currhp) / hp) / 100;
        rb.velocity = new Vector2((movespeed * 1), rb.velocity.y);   //bude se hybyt do leva zatim je to testovaci
        if (Physics2D.OverlapCircle(transform.position, 0.4f, opponentSoldier) != null)
        {
            if (currhp <= 0)
            {
                Destroy(gameObject);
            }
            else if (currhp > 0 && canGetdmg == true)
            {
                StartCoroutine(Dmgdealcooldown());
            }
        }
        else if (Physics2D.OverlapCircle(transform.position, 0.4f, opponentRanger) != null)
        {
            //tady bude dávat attack ranger této jednotce
        }
        else if(Physics2D.OverlapCircle(transform.position, 0.4f, opponentTank) != null)
        {
            //tady bude dávat attack tank této jednotce
        }
        hpBar.transform.localScale = new Vector2(hpinprocents, hpBar.transform.localScale.y);
    }

    IEnumerator Dmgdealcooldown()
    {
        canGetdmg = false;
        currhp = currhp - soldierEscript.dmg;
        Debug.Log("Player " + currhp);
        yield return new WaitForSecondsRealtime(2);
        canGetdmg = true;
    }
}
