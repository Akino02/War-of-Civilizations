using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierE : MonoBehaviour
{
	SoldierP soldierPscript;                       //import scriptu protivnika
	[SerializeField] GameObject soldierP;          //import objektu

    public Rigidbody2D rb;              //funkce pro gravitaci
    public LayerMask[] armyTypes = new LayerMask[3];
	//public LayerMask[] armyTypesE = new LayerMask[3];
	public float[] ranges = { 0.5f, 1.4f, 0.5f };
    /*public LayerMask opponentSoldier;       //layer hracovych jednotek typu soldier
    public float rangeS = 0.5f;
    public LayerMask opponentRanger;       //layer hracovych jednotek typu ranger
    public float rangeR = 1.4f;
    public LayerMask opponentTank;       //layer hracovych jednotek typu tank
    public float rangeT = 0.5f;*/
    public float movespeed;             //rychlost pohybu objektu
    public LayerMask armyType;
    public int armyTypeNum;

    //Ohledne HPbaru
    public GameObject hpBar;

    public float[] maxhp = { 100, 60, 300 };
    public float currhp;
    //public float[] hptype = { 100, 60, 300 };
    private float hpinprocents = 1f;

    //Ohledne utoku
    /*public float dmgS = 40; //sila teto postavy
    public float dmgR = 60; //sila teto postavy
    public float dmgT = 40; //sila teto postavy*/
    public int[] dmg = { 40, 60, 40 };
    public bool canGetdmgM = true;      //na blizko
    public bool canGetdmgR = true;      //na dalku

    // Start is called before the first frame update
    void Start()
	{
		soldierPscript = soldierP.GetComponent<SoldierP>();  //import protivnika a jeho promìnných
        for (int i = 0; i < 3; i++)
        {
            if (armyType == armyTypes[i])
            {
                //maxhp = hptype[i];
                currhp = maxhp[i];
                armyTypeNum = i;
            }
            //Debug.Log(i);
        }
    }
	// Update is called once per frame
	void Update()
	{
        hpinprocents = ((100 * currhp) / maxhp[armyTypeNum]) / 100;
		rb.velocity = new Vector2((movespeed * -1), rb.velocity.y);   //bude se hybyt do leva zatim je to testovaci
		if (Physics2D.OverlapCircle(transform.position, soldierPscript.ranges[0], soldierPscript.armyTypes[0]) != null || Physics2D.OverlapCircle(transform.position, soldierPscript.ranges[2], soldierPscript.armyTypes[2]) != null)	//je tam if, aby to poznaval hned
		{
			if(currhp <= 0)
			{
				Destroy(gameObject);
			}
			else if(currhp > 0 && canGetdmgM == true)
			{
				StartCoroutine(DmgdealcooldownMelee());
			}
		}
		if(Physics2D.OverlapCircle(transform.position, soldierPscript.ranges[1], soldierPscript.armyTypes[1]) != null)			//je tam if, aby to poznaval hned
		{
			if (currhp <= 0)
			{
				Destroy(gameObject);
			}
			else if (currhp > 0 && canGetdmgR == true)
			{
				StartCoroutine(DmgdealcooldownRange());
			}
		}
		hpBar.transform.localScale = new Vector2(hpinprocents, hpBar.transform.localScale.y);
	}

	IEnumerator DmgdealcooldownMelee()
	{
		canGetdmgM = false;
		if (Physics2D.OverlapCircle(transform.position, soldierPscript.ranges[0], soldierPscript.armyTypes[0]) != null)
		{
			currhp -= soldierPscript.dmg[0];
		}
		else if (Physics2D.OverlapCircle(transform.position, soldierPscript.ranges[2], soldierPscript.armyTypes[2]) != null)
		{
			currhp -= soldierPscript.dmg[2];
		}
		Debug.Log("Enemy " + currhp);
		yield return new WaitForSecondsRealtime(3);
		canGetdmgM = true;
	}
	IEnumerator DmgdealcooldownRange()
	{
		canGetdmgR = false;
		currhp -= soldierPscript.dmg[1];
		Debug.Log("Enemy " + currhp);
		yield return new WaitForSecondsRealtime(2);
		canGetdmgR = true;
	}
}
