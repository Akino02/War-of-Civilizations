using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmySpawn : MonoBehaviour
{
    SoldierE soldierEscript;                       //import scriptu protivnika
    [SerializeField] GameObject soldierE;          //import objektu

    public GameObject Soldier;          //co spawne
    public GameObject PlayerSpawner;    //kde to spawne

    //nepratele
    public LayerMask opponentSoldier;       //layer hracovych jednotek typu soldier
    public LayerMask opponentRanger;       //layer hracovych jednotek typu ranger
    public LayerMask opponentTank;       //layer hracovych jednotek typu tank

    public float zkusenosti = 0;        //zkusenosti
    public float penize = 0;            //penize
    public bool canProduce = true;      //zda muze vyrabet
    public float order = 0;             //kolik jich vyrabime   //udìlat poudìji jako array, protoze bude vyrabet vice jednotek

    public GameObject makeBar;

    public float maxHPBase = 1000;
    public float currHPBase = 1000;
    public float hpbaseinprocents = 1f;
    public bool canGetdmg = true;

    public Image hpBaseBarcurr;
    public GameObject basePosition;

    // Start is called before the first frame update
    void Start()
    {
        soldierEscript = soldierE.GetComponent<SoldierE>();  //import protivnika a jeho promìnných
    }

    // Update is called once per frame
    void Update()
    {
        hpbaseinprocents = ((100 * currHPBase) / maxHPBase) / 100;  //pomoc pri pocitani procent
        if (order > 0 && canProduce == true && currHPBase != 0)  //zacne se produkce jakmile bude neco v rade
        {
            StartCoroutine(orderfactory());
        }
        if (Physics2D.OverlapCircle(basePosition.transform.position, 0.8f, opponentSoldier) != null && canGetdmg == true)  //nejaky nepritel muze ubrat zivoty zakladny
        {
            StartCoroutine(damageBaseSoldier());
        }
        hpBaseBarcurr.fillAmount = hpbaseinprocents;  //urcovani zivotu v procentech
    }
    public void SoldierSpawn()  // tato funkce na kliknuti spawne jednoho vojaka
    {
        if (order < 5)      //muze je vyrabet v rade a kazdy se bude vyrabet 5s   //jeste tam pak doplnit ze za to bude platit
        {
            //StartCoroutine(ClickCooldown());   //je to zatim nevyuzite
            order += 1;
            Debug.Log("Prirazeno do fronty " + order);
        }
        else
        {
            Debug.Log("Fronta je plna " + order);
        }
    }
    /*IEnumerator ClickCooldown()      //nastaveni na prestavku at nemuze to spamovat to klikani a spawnovani     //je to zatim nevyuzite
    {
        yield return order;
        order -= 1;
    }*/
    IEnumerator orderfactory()      //bude vyrabet jednoho 5s
    {
        canProduce = false;
        yield return new WaitForSecondsRealtime(5);
        Instantiate(Soldier, PlayerSpawner.transform.position, PlayerSpawner.transform.rotation);
        Debug.Log("Byl vyroben " + order);
        order -= 1;
        canProduce = true;
    }
    IEnumerator damageBaseSoldier()      //base bude dostavat dmg od enemy
    {
        canGetdmg = false;
        currHPBase = currHPBase - soldierEscript.dmg;
        yield return new WaitForSecondsRealtime(2);
        canGetdmg = true;
    }
}
