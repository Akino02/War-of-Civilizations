using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    UnitScript armyScriptE;

    //private GameObject hitBox;

    [Header("Attributes")]
    private float damage;
    private bool hit = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DestroyObject();     //pokud propadne nebo se dotkne nepritele
    }
    private void OnTriggerEnter2D(Collider2D hitBox)
    {
        if (hitBox.gameObject.CompareTag("Enemy"))
        {
            var SoldierArmyScript = hitBox.GetComponent<UnitScript>();
            armyScriptE = SoldierArmyScript;                    //dosazeni scriptu za objekt
            damage = (UnityConfiguration.maxhp[armyScriptE.lvl, 2] / 3) / 2;
            if (hit == false)
            {
                armyScriptE.currhp -= damage;                      //nastaveni poskozeni fireballu podle toho kolik dana postavicka ma hp
            }
            hit = true;
        }
    }

    private void DestroyObject()
    {
        if(transform.position.y < -20 || hit == true)
        {
            Destroy(gameObject);
            return;
        }
    }
}
