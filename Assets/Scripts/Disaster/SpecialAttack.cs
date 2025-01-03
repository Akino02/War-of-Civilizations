using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    UnitScript armyScriptE;

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
        //pokud propadne nebo se dotkne nepritele
        DestroyObject();
    }
    private void OnTriggerEnter2D(Collider2D hitBox)
    {
        if (hitBox.gameObject.CompareTag("Enemy"))
        {
            armyScriptE = hitBox.GetComponent<UnitScript>();
            //dosazeni scriptu za objekt
            damage = Random.Range(UnityConfiguration.dmgMin[0] * (ButtonScript.specialAttackLevel+1), UnityConfiguration.dmgMax[2]) * (ButtonScript.specialAttackLevel + 1);
            //Debug.Log(ButtonScript.specialAttackLevel);
            if (hit == false)
            {
                //nastaveni poskozeni fireballu podle toho kolik dana postavicka ma hp
                armyScriptE.currhp -= damage;
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
