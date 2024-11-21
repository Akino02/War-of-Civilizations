using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDead : MonoBehaviour
{
    UnitScript unitData;

    ProgresScript progresS;
    EvolutionPlayerScript evolutionPlayerS;

    private void Awake()
    {
        unitData = GetComponent<UnitScript>();

        //toto najde zakladnu hrace pomoci tagu ktery ma
        GameObject script1 = GameObject.FindWithTag("baseP");
        progresS = script1.GetComponent<ProgresScript>();
        evolutionPlayerS = script1.GetComponent<EvolutionPlayerScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckDead();
    }
    private void CheckDead()
    {
        //Pokud jednotka nebude mit zivoty
        if (unitData.currhp <= 0 || transform.position.y < UnityConfiguration.deadZone)
        {
            //Pokud se jedna o nepritele
            if (unitData.team != 0 && transform.position.y > UnityConfiguration.deadZone)
            {
                //isDead = true;
                Reward();
            }
            unitData.attackSound.Stop();
            if (!unitData.attackSound.isPlaying)
            {
                Destroy(gameObject);
            }
            return;
        }
    }

    //tato funkce predava odmeny za zabiti nepritele (zkusenosti a penize)
    private void Reward()
    {
        progresS.money += UnityConfiguration.moneykill[unitData.armyTypeNum] * (unitData.lvl + 1);
        evolutionPlayerS.experience += UnityConfiguration.expperkill[unitData.armyTypeNum];
    }
}
