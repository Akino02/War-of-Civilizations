using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HpScript : MonoBehaviour
{
    //propojeni zakladnich scriptu pro funkci UI
    EvolutionPlayerScript evolutionPlayerS;
    EvolutionEnemyScript evolutionEnemyS;
    public float currHPBase;
    public float hpbaseinprocents = 1f;

    //ziskani levelu zakladny
    public int currLevelBase;

    //aktualni zivoty do textu viditelneho
    public Text hpTextShow;
    //vizualni ukazatel zivotu
    public Image hpBaseBarcurr;

    public bool upgradingHp = false;

    public Team teamHP;

    //
    public void Awake()
    {
        //propojeni zakladnich scriptu pro funkci UI
        evolutionPlayerS = GetComponent<EvolutionPlayerScript>();

        //toto najde zakladnu nepritele pomoci tagu ktery ma
        GameObject objectOfScript = GameObject.FindWithTag("baseE");
        evolutionEnemyS = objectOfScript.GetComponent<EvolutionEnemyScript>();
    }
    // Start is called before the first frame update
    public void Start()
    {
        GetCurrBaseLevel();
        currHPBase = UnityConfiguration.maxHPBase * currLevelBase;
    }

    // Update is called once per frame
    public void Update()
    {
        hpTextShow.text = Mathf.Round(currHPBase).ToString();


        //kolik mame aktualne, kolik budeme mit, rychlost jak se to bude posouvat nasobeno synchronizovany cas
        hpBaseBarcurr.fillAmount = Mathf.Lerp(hpBaseBarcurr.fillAmount, currHPBase / (UnityConfiguration.maxHPBase * currLevelBase), 3f * Time.deltaTime);

        //nastaveni barev pro hpBar, pokud minHP tak red a pokud maxHP tak green a je to gradian
        Color healthColor = Color.Lerp(Color.red, Color.green, (currHPBase / (UnityConfiguration.maxHPBase * currLevelBase)));

        //zde se aplikuje barva gradianu, podle toho kolik ma hpBar zivotu
        hpBaseBarcurr.color = healthColor;
        if (currHPBase <= 0)
        {
            currHPBase = 0;
        }
    }
    public void GetCurrBaseLevel()
    {
        if (teamHP == Team.Player)
        {
            //predani hodnoty z maximalnich zivotu do aktualnich zivotu
            currLevelBase = evolutionPlayerS.level + 1;
        }
        else if (teamHP == Team.Enemy)
        {
            currLevelBase = evolutionEnemyS.level + 1;
        }
    }

    //zachova procentuelne hp pri upgradu
    public void UpgradeHp()
    {
        GetCurrBaseLevel();
        if (currLevelBase > 0)
        {
            Debug.Log(currHPBase);
            Debug.Log(UnityConfiguration.maxHPBase * (currLevelBase - 1));

            //pomoc pri pocitani procent(zde se zjistuje rozdil aktualnich hp a maximalnich, aby se to pak podle procent upravilo v dalsi fazi)
            hpbaseinprocents = currHPBase / (UnityConfiguration.maxHPBase * (currLevelBase - 1));

            //vypocita aktualniho poctu hp v novych zivotech
            currHPBase = hpbaseinprocents * (UnityConfiguration.maxHPBase * currLevelBase);
        }
    }
}

public enum Team
{
    Player = 1,
    Enemy = 2
}