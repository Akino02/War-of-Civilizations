using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHpScript : MonoBehaviour
{
    //importovani dat jednotky
    UnitScript unitData;

    //prevedeni currhp na procenta
    private float hpInPercentage;

    //tohle je pevne nastavena hodnota, aby currhp byli videt jako prvni v layer
    private int sortLayerToShowCurrHP = -3;

    //vizualni objekt, do ktereho se budou vpisovat procenta
    public GameObject hpBar;

    //ziskani teamu jednotky
    private int teamInt => (int)unitData.team;

    //orientace kam se budou currhp blizit k nule (hrac ma bod 0 vlevo, enemy to ma naopak)
    private int[] hpOrientation = { -1, 1 };

    private void Awake()
    {
        unitData = GetComponent<UnitScript>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHP();
    }

    private void UpdateHP()
    {
        //vypocet procent
        hpInPercentage = ((100 * unitData.currhp) / (UnityConfiguration.maxhp[unitData.armyTypeNum] * (unitData.lvl + 1))) / 100;
        //nastaveni velikosti hp v scale
        hpBar.transform.localScale = new Vector2(hpInPercentage, hpBar.transform.localScale.y);
        //prehozeni a nastaveni orientace hpbaru (curr)
        hpBar.transform.localPosition = new Vector3(((1-hpInPercentage)/2)* hpOrientation[teamInt], hpBar.transform.localPosition.y, sortLayerToShowCurrHP);
        if (unitData.currhp <= 0)
        {
            unitData.currhp = 0;
        }
    }
}
