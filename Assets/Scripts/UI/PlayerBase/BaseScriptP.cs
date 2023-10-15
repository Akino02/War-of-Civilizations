using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;       //import teto funkce, abych mohl pracovat s UI vecmi v unity enginu

public class BaseScriptP : MonoBehaviour
{
    //import enemy scriptu pro damage jaky davaji
    SoldierE soldierEscript;                       //import scriptu protivnika
    [SerializeField] GameObject soldierE;          //import objektu
    //

    //co a kde to bude spawnovat
    public GameObject soldier;          //co spawne
    public GameObject playerSpawner;    //kde to spawne
    //

    //nepratele (layers)
    public LayerMask opponentSoldier;       //layer hracovych jednotek typu soldier
    public LayerMask opponentRanger;       //layer hracovych jednotek typu ranger
    public LayerMask opponentTank;       //layer hracovych jednotek typu tank
    //

    //zatím 2/3 nevyuzite  funkce nasi basky
    public float zkusenosti = 0;        //zkusenosti
    public float penize = 0;            //penize
    public float order = 0;             //kolik jich vyrabime   //udìlat poudìji jako array, protoze bude vyrabet vice jednotek
    //

    //vyrovnik v procentech graficky                //zatim nefunguje nevim jak udelat ten casovac
    public Image progBar;
    private float waitSoldier = 5;
    /*private float waitRanger = 8;
    private float waitTank = 10;*/
    public float progbarinprocents = 0f;            //
    public float timer = 0;
    public bool canProduce = true;      //zda muze vyrabet
    //

    //hp a ubirani base
    public float maxHPBase = 1000;
    public float currHPBase = 1000;
    public float hpbaseinprocents = 1f;

    public Image hpBaseBarcurr;
    public GameObject basePosition;

    public bool canGetdmg = true;
    //

    // Start is called before the first frame update
    void Start()
    {
        soldierEscript = soldierE.GetComponent<SoldierE>();  //import protivnika a jeho promìnných
    }

    // Update is called once per frame
    void Update()
    {
        hpbaseinprocents = ((100 * currHPBase) / maxHPBase) / 100;  //pomoc pri pocitani procent
        if (order > 0 && currHPBase != 0)  //zacne se produkce jakmile bude neco v rade a taky se zacne hybat progbar
        {
            StartCoroutine(Orderfactory());
        }
        if (Physics2D.OverlapCircle(basePosition.transform.position, 0.8f, opponentSoldier) != null && canGetdmg == true && currHPBase > 0)  //nejaky nepritel muze ubrat zivoty zakladny
        {
            StartCoroutine(DamageBaseSoldier());
        }
        hpBaseBarcurr.fillAmount = hpbaseinprocents;  //urcovani zivotu v procentech
    }
    public void SoldierSpawn()  // tato funkce na kliknuti spawne jednoho vojaka
    {
        if (order < 5 && currHPBase > 0)      //muze je vyrabet v rade a kazdy se bude vyrabet 5s   //jeste tam pak doplnit ze za to bude platit
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
    //funkce pro progressBar
    IEnumerator Orderfactory()   //bude vyrabet jednoho 5s           //pak udelat na if (aby se menil ten vyrobni cas)              // pozdeji udelat smooth
    {
        if (order > 0 && progbarinprocents != 1f && canProduce == true)
        {
            canProduce = false;
            timer += 1;
            yield return new WaitForSecondsRealtime(1);
            progbarinprocents = ((100 * timer) / waitSoldier) / 100;        //zatím urèeno jen pro Soldiera
            progBar.fillAmount = progbarinprocents;
            canProduce = true;
            if(progbarinprocents == 1f)
            {
                timer = 0;
                progbarinprocents = 0f;
                Instantiate(soldier, playerSpawner.transform.position, playerSpawner.transform.rotation);
                Debug.Log("Byl vyroben " + order);
                order -= 1;
            }
        }
        if (order == 0)
        {
            timer = 0;
            progbarinprocents = ((100 * timer) / waitSoldier) / 100;
            progBar.fillAmount = progbarinprocents;
        }
    }
    IEnumerator DamageBaseSoldier()      //base bude dostavat dmg od enemy
    {
        canGetdmg = false;
        currHPBase -= soldierEscript.dmg;
        yield return new WaitForSecondsRealtime(2);
        canGetdmg = true;
    }
}
