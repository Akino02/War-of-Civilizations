using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierP : MonoBehaviour
{
    SoldierE soldierEscript;                       //import scriptu protivnika
    [SerializeField] GameObject soldierE;

    public Rigidbody2D rb;              //funkce pro gravitaci
    public LayerMask opponentSoldier;       //layer hracovych jednotek typu soldier
    public float rangeS = 0.5f;
    public LayerMask opponentRanger;       //layer hracovych jednotek typu ranger
    public float rangeR = 1.4f;
    public LayerMask opponentTank;       //layer hracovych jednotek typu tank
    public float rangeT = 0.5f;
    public float movespeed;             //rychlost pohybu objektu

    //Ohledne HPbaru
    public GameObject hpBar;

    public float maxhp;
    public float currhp;
    private float hpinprocents = 1f;

    //Ohledne utoku
    public float dmgS = 40; //sila teto postavy
    public float dmgR = 60; //sila teto postavy
    public float dmgT; //sila teto postavy
    public bool canGetdmgM = true;      //na blizko
    public bool canGetdmgR = true;      //na dalku

    // Start is called before the first frame update
    void Start()
    {
        soldierEscript = soldierE.GetComponent<SoldierE>();  //import protivnika a jeho promennych
    }
    // Update is called once per frame
    void Update()
    {
        hpinprocents = ((100 * currhp) / maxhp) / 100;
        rb.velocity = new Vector2((movespeed * 1), rb.velocity.y);   //bude se hybyt do leva zatim je to testovaci
        if (Physics2D.OverlapCircle(transform.position, rangeS, opponentSoldier) != null)       //je tam if, aby to poznaval hned
        {
            if (currhp <= 0)
            {
                Destroy(gameObject);
            }
            else if (currhp > 0 && canGetdmgM == true)
            {
                StartCoroutine(DmgdealcooldownMelee());
            }
        }
        if (Physics2D.OverlapCircle(transform.position, rangeR, opponentRanger) != null)        //je tam if, aby to poznaval hned
        {
            if (currhp <= 0)
            {
                Destroy(gameObject);
            }
            else if (currhp > 0 && canGetdmgR == true)
            {
                StartCoroutine(DmgdealcooldownRange());
            }
        }
        else if(Physics2D.OverlapCircle(transform.position, rangeT, opponentTank) != null)
        {
            //tady bude davat attack tank teto jednotce
        }
        hpBar.transform.localScale = new Vector2(hpinprocents, hpBar.transform.localScale.y);
    }

    IEnumerator DmgdealcooldownMelee()
    {
        canGetdmgM = false;
        currhp -= soldierEscript.dmgS;
        Debug.Log("Player " + currhp);
        yield return new WaitForSeconds(3);
        canGetdmgM = true;
    }
    IEnumerator DmgdealcooldownRange()
    {
        canGetdmgR = false;
        currhp -= soldierEscript.dmgR;
        Debug.Log("Player " + currhp);
        yield return new WaitForSecondsRealtime(2);
        canGetdmgR = true;
    }

    /*private void OnDrawGizmosSelected()   //vykreslí kruh okolo jednotky
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeR);
    }*/
}