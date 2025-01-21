using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    UnitScript armyScriptE;

    [Header("Attributes")]
    public float damageMin = 50;
    public float damageMax = 150;
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
        float damage;
        if (hitBox.gameObject.CompareTag("Enemy"))
        {
            armyScriptE = hitBox.GetComponent<UnitScript>();
            //dosazeni scriptu za objekt
            damage = Random.Range(damageMin * (ButtonScript.specialAttackLevel+1), damageMax * (ButtonScript.specialAttackLevel + 1));
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
