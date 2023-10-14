using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierE : MonoBehaviour
{
    SoldierP soldierPscript;                       //import scriptu protivnika
    [SerializeField] GameObject soldierP;          //import objektu

    public Rigidbody2D rb;              //funkce pro gravitaci
    public LayerMask opponent;       //layer hracovych jednotek
    public float movespeed;             //rychlost pohybu objektu

    //Ohledne HPbaru
    public GameObject hpBar;

    private float hp = 100;
    public float currhp = 100;
    private float hpinprocents = 1f;

    //Ohledne utoku
    public float dmg = 25;
    public bool canGetdmg = true;

    // Start is called before the first frame update
    void Start()
    {
        soldierPscript = soldierP.GetComponent<SoldierP>();  //import protivnika a jeho promìnných
    }
    // Update is called once per frame
    void Update()
    {
        hpinprocents = ((100 * currhp) / hp) / 100;
        rb.velocity = new Vector2((movespeed * -1), rb.velocity.y);   //bude se hybyt do leva zatim je to testovaci
        if (Physics2D.OverlapCircle(transform.position, 0.4f, opponent) != null)
        {
            if(currhp <= 0)
            {
                Destroy(gameObject);
            }
            else if(currhp > 0 && canGetdmg == true)
            {
                StartCoroutine(Dmgdealcooldown());
            }
        }
        hpBar.transform.localScale = new Vector2(hpinprocents, hpBar.transform.localScale.y);
    }

    IEnumerator Dmgdealcooldown()
    {
        canGetdmg = false;
        currhp = currhp - soldierPscript.dmg;
        Debug.Log("Enemy " + currhp);
        yield return new WaitForSecondsRealtime(2);
        canGetdmg = true;
    }
}
