using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmySpawn : MonoBehaviour
{
    public GameObject Soldier;          //co spawne
    public GameObject PlayerSpawner;    //kde to spawne

    public bool canClick = true;

    /*// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }*/
    public void SoldierSpawn()  // tato funkce na kliknuti spawne jednoho vojaka
    {
        if (canClick == true)
        {
            StartCoroutine(ClickCooldown());
        }
    }
    IEnumerator ClickCooldown()      //nastaveni na prestavku at nemuze to spamovat to klikani a spawnovani
    {
        canClick = false;
        Instantiate(Soldier, PlayerSpawner.transform.position, PlayerSpawner.transform.rotation);
        yield return new WaitForSecondsRealtime(5);
        canClick = true;
    }
}
