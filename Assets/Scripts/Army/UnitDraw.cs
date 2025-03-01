using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDraw : MonoBehaviour
{
    UnitScript unitData;

    private void Awake()
    {
        unitData = GetComponent<UnitScript>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()     //vykreslí kruh okolo jednotky
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(unitData.distanceFromAllie, UnityConfiguration.ranges[2]);                    //odstup od spojencu

        Gizmos.DrawWireSphere(gameObject.transform.position, unitData.unitRange);
    }
}
