using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //import teto funkce, abych mohl pracovat s UI vecmi v unity enginu

public class EnemySpawn : MonoBehaviour
{
    //import enemy scriptu pro damage jaky davaji
    SoldierP soldierPscript;                       //import scriptu protivnika
    [SerializeField] GameObject soldierP;          //import objektu
    //
    //import layeru nepratel(hracovych)
    public LayerMask opponentSoldier;       //layer hracovych jednotek typu soldier
    public LayerMask opponentRanger;       //layer hracovych jednotek typu ranger
    public LayerMask opponentTank;       //layer hracovych jednotek typu tank
    //
    //
    //co bude spawnovat a kde
    public GameObject soldier;          //co spawne
    public GameObject ranger;          //co spawne
    public GameObject tank;          //co spawne
    public GameObject enemySpawner;    //kde to spawne
    //
    //veci ohledne baseHP ci damage pro base
    public float maxHPBase = 1000;
    public float currHPBase = 1000;
    public float hpbaseinprocents = 1f;

    public Image hpBaseBarcurr;
    public GameObject basePosition;

    public bool canGetdmg = true;
    //
    //spawnovani jednotek
    public bool canSpawn = true;
    public int nahoda = 0;
    //
    // Start is called before the first frame update
    void Start()
    {
        soldierPscript = soldierP.GetComponent<SoldierP>();  //import protivnika a jeho promìnných
    }

    // Update is called once per frame
    void Update()
    {
        hpbaseinprocents = ((100 * currHPBase) / maxHPBase) / 100;  //pomoc pri pocitani procent
        if (canSpawn == true && nahoda >= 1 && nahoda <= 3 && currHPBase > 0)
        {
            StartCoroutine(CooldownSoldier());
        }
        else if(nahoda == 0 || nahoda >= 4)
        {
            nahoda = Random.Range(1, 5);
        }
        //Debug.Log(nahoda);
        if (Physics2D.OverlapCircle(basePosition.transform.position, 0.8f, opponentSoldier) != null && canGetdmg == true && currHPBase > 0)  //nejaky nepritel muze ubrat zivoty zakladny
        {
            StartCoroutine(DamageBaseSoldier());
        }
        hpBaseBarcurr.fillAmount = hpbaseinprocents;  //urcovani zivotu v procentech
    }

    IEnumerator CooldownSoldier()      //nastaveni na prestavku at nemuze to spamovat to klikani a spawnovani
    {
        canSpawn = false;
        Instantiate(soldier, enemySpawner.transform.position, enemySpawner.transform.rotation);
        nahoda = Random.Range(1, 5);
        yield return new WaitForSecondsRealtime(5);
        canSpawn = true;
    }
    //pak tady bude i ranger i tank
    //
    IEnumerator DamageBaseSoldier()      //base bude dostavat dmg od enemy
    {
        canGetdmg = false;
        currHPBase -= soldierPscript.dmg;
        yield return new WaitForSecondsRealtime(2);
        canGetdmg = true;
    }
}
