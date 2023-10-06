using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSoldierE : MonoBehaviour
{
    [SerializeField] MoveSoldierE player;         //import scriptu protivnika     //špatnì funguje

    public Rigidbody2D rb;              //funkce pro gravitaci
    public LayerMask opponent;       //layer hracovych jednotek
    public float movespeed;             //rychlost pohybu objektu

    public int hp = 100;
    public int dmg = 75;
    public bool canGetdmg = true;

    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2((movespeed * -1), rb.velocity.y);   //bude se hybyt do leva zatim je to testovaci
        if (Physics2D.OverlapCircle(transform.position, 0.4f, opponent) != null)
        {
            if(hp <= 0)
            {
                Destroy(gameObject);
            }
            else if(hp > 0 && canGetdmg == true)
            {
                StartCoroutine(dmgdealcooldown());
            }
        }
    }

    IEnumerator dmgdealcooldown()
    {
        canGetdmg = false;
        hp = hp - dmg;
        Debug.Log("Enemy " + hp);
        yield return new WaitForSecondsRealtime(2);
        canGetdmg = true;
    }
}
