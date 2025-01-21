using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisasterScript : MonoBehaviour
{
    EvolutionPlayerScript evolutionS;

    public GameObject fireBall;                     //pozdeji array vice objektu (ball, arrow, ...)

    //public GameObject[] disasterObjects;          //tam budou ulozene objekty (fireball, arrow, wall)

    public GameObject baseBorderL;
    public GameObject baseBorderR;

    public GameObject disasterZone;

    private float waitDisasterFill = 0f;
    private int waitingTimeForDisaster = 60;

    public Image waitDisasterFillBox;

    public bool canDoDisaster = true;

    private void Awake()
    {
        evolutionS = GetComponent<EvolutionPlayerScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!canDoDisaster && !GameScript.isGameOver)
        {
            WaitDisaster();
        }
    }
    public IEnumerator SpawnFireBall()        //GameObject objects a zmenit jmeno na SpawnObject //tam se bude dosazovat aktualni object (fireball, arrows, wall, ...)
    {
        waitDisasterFill = 1f;
        waitDisasterFillBox.fillAmount = waitDisasterFill;
        WaitDisaster();
        int i = 0;
        while (i <= 30)
        {
            float randomPosX = Random.Range(baseBorderL.transform.position.x, baseBorderR.transform.position.x);
            float randomRotZ = Random.Range(0, 360);
            Quaternion changedRotationZ = Quaternion.Euler(disasterZone.transform.rotation.x, 0, randomRotZ);
            Vector3 disasterZonePos = new Vector3(randomPosX, disasterZone.transform.position.y, fireBall.transform.position.z);
            if (evolutionS.level == 0)
            {
                Instantiate(fireBall, disasterZonePos, changedRotationZ);
            }
            else
            {
                Instantiate(fireBall, disasterZonePos, disasterZone.transform.rotation);
            }
            yield return new WaitForSeconds(0.4f);
            i += 1;
        }
        yield return null;
    }

    /*public void SetDisasterObject()
    {

    }*/

    public void WaitDisaster()
    {
        if (waitDisasterFillBox.fillAmount > 0)
        {
            waitDisasterFill = (Time.deltaTime / waitingTimeForDisaster);
            waitDisasterFillBox.fillAmount = Mathf.Lerp(waitDisasterFillBox.fillAmount, waitDisasterFillBox.fillAmount - 1f, waitDisasterFill);		//min, max, speed
        }
        else//pokud je to rovno nule ci mensi 
        {
            waitDisasterFill = 0;
            waitDisasterFillBox.fillAmount = waitDisasterFill;
            canDoDisaster = true;
        }
        return;
    }
}
