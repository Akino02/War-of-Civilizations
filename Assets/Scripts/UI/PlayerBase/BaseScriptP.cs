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
    public GameObject soldierP;          //co spawne
    public GameObject rangerP;          //co spawne
    public GameObject tankP;          //co spawne
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
    public int order = 0;             //kolik jich vyrabime   //udìlat poudìji jako array, protoze bude vyrabet vice jednotek
    public int[] orderv2 = {0, 0, 0, 0, 0};             //poradi jednotek                               //zatim mimo provoz neni hodne jednotek *******************************
    //

    //vyrovnik v procentech graficky                //zatim nefunguje nevim jak udelat ten casovac
    public Image progBar;
    private float waitSoldier = 5;
    /*private float waitRanger = 8;
    private float waitTank = 10;*/
    public float progbarinprocents = 0f;            //
    public float timer = 0;
    public bool canProduce = true;      //zda muze vyrabet

    public Image order1;
    public Image order2;
    public Image order3;
    public Image order4;
    public Image order5;
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
        StartCoroutine(OrderView());        //graficke vydeni fronty
        //StartCoroutine(OrderSorter());        //radi vojaky jak se maji vyrobit                 //zatim mimo provoz neni hodne jednotek *******************************
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
    public void SoldierSpawn()  // tato funkce na kliknuti spawne jednoho vojaka            PRO SOLDIERA
    {
        if (order < 5 && currHPBase > 0)      //muze je vyrabet v rade a kazdy se bude vyrabet 5s   //jeste tam pak doplnit ze za to bude platit
        {
            //StartCoroutine(ClickCooldown());   //je to zatim nevyuzite
            order += 1;
            orderv2[order-1] = 1;
            /*for(int i = 0;i < order; i++)                                 //zatim mimo provoz neni hodne jednotek *******************************
            {
                orderv2[i] = 1;
            }
            for (int j = 0; j < order; j++)
            {
                Debug.Log(orderv2[j]);
            }*/
            Debug.Log("Prirazeno do fronty " + order);
        }
        else
        {
            Debug.Log("Fronta je plna " + order);
        }
    }
    public void RangerSpawn()  // tato funkce na kliknuti spawne jednoho vojaka            PRO RANGERA
    {
        if (order < 5 && currHPBase > 0)      //muze je vyrabet v rade a kazdy se bude vyrabet 5s   //jeste tam pak doplnit ze za to bude platit
        {
            order += 1;
            orderv2[order - 1] = 2;
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
                if (orderv2[0] == 1)
                {
                    Instantiate(soldierP, playerSpawner.transform.position, playerSpawner.transform.rotation);
                    Debug.Log("Byl vyroben Soldier");
                }
                else if (orderv2[0] == 2)
                {
                    Instantiate(rangerP, playerSpawner.transform.position, playerSpawner.transform.rotation);
                }
                //Debug.Log("Byl vyroben " + order);
                order -= 1;
                if(order > 0) 
                {
                    StartCoroutine(OrderSorter());
                }
            }
        }
        if (order == 0)
        {
            timer = 0;
            progbarinprocents = ((100 * timer) / waitSoldier) / 100;
            progBar.fillAmount = progbarinprocents;
        }
    }
    IEnumerator OrderView()             //toto zajistuje vizualni frontu vyroby jednotek
    {
        int giveOrder = order;
        order1.fillAmount = giveOrder;
        giveOrder -= 1;
        order2.fillAmount = giveOrder;
        giveOrder -= 1;
        order3.fillAmount = giveOrder;
        giveOrder -= 1;
        order4.fillAmount = giveOrder;
        giveOrder -= 1;
        order5.fillAmount = giveOrder;
        yield return order;
    }
    IEnumerator OrderSorter()         //toto serazuje array podle toho co je na rade ve vyrobe            //zatim mimo provoz neni hodne jednotek *******************************
    {
        for (int i = 0;i < 5; i++)
        {
            if(i != 4)
            {
                orderv2[i] = orderv2[i + 1];
            }
            else
            {
                orderv2[i] = 0;
            }
        }
        yield return order;
    }
    IEnumerator DamageBaseSoldier()      //base bude dostavat dmg od enemy
    {
        canGetdmg = false;
        currHPBase -= soldierEscript.dmgS;
        yield return new WaitForSecondsRealtime(2);
        canGetdmg = true;
    }
}
