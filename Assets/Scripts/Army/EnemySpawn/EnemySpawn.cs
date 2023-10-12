using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject soldier;          //co spawne
    public GameObject ranger;          //co spawne
    public GameObject tank;          //co spawne
    public GameObject enemySpawner;    //kde to spawne

    public bool canSpawn = true;
    public int nahoda = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn == true && nahoda >= 1 && nahoda <= 3)
        {
            StartCoroutine(CooldownSoldier());
        }
        else if(nahoda == 0 || nahoda >= 4)
        {
            nahoda = Random.Range(1, 5);
        }
        //Debug.Log(nahoda);
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
}
