using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierE : MonoBehaviour
{
	SoldierP soldierPscript;                       //import scriptu protivnika
	[SerializeField] GameObject soldierP;          //import objektu

	public Rigidbody2D rb;              //funkce pro gravitaci
	public LayerMask opponentSoldier;       //layer hracovych jednotek typu soldier
	public float rangeS = 0.5f;
	public LayerMask opponentRanger;       //layer hracovych jednotek typu ranger
	public float rangeR = 1.4f;
	public LayerMask opponentTank;       //layer hracovych jednotek typu tank
	public float rangeT = 0.5f;
	public float movespeed;             //rychlost pohybu objektu

	//Ohledne HPbaru
	public GameObject hpBar;

	public float maxhp = 100;
	public float currhp = 100;
	private float hpinprocents = 1f;

	//Ohledne utoku
	public float dmgS = 30;
	public float dmgR = 15;
	public float dmgT;
	public bool canGetdmgM = true;
	public bool canGetdmgR = true;

	// Start is called before the first frame update
	void Start()
	{
		soldierPscript = soldierP.GetComponent<SoldierP>();  //import protivnika a jeho promìnných
	}
	// Update is called once per frame
	void Update()
	{
		hpinprocents = ((100 * currhp) / maxhp) / 100;
		rb.velocity = new Vector2((movespeed * -1), rb.velocity.y);   //bude se hybyt do leva zatim je to testovaci
		if (Physics2D.OverlapCircle(transform.position, rangeS, opponentSoldier) != null)		//je tam if, aby to poznaval hned
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
		if(Physics2D.OverlapCircle(transform.position, rangeR, opponentRanger) != null)			//je tam if, aby to poznaval hned
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
		currhp -= soldierPscript.dmgS;
		Debug.Log("Enemy " + currhp);
		yield return new WaitForSecondsRealtime(3);
		canGetdmgM = true;
	}
	IEnumerator DmgdealcooldownRange()
	{
		canGetdmgR = false;
		currhp -= soldierPscript.dmgR;
		Debug.Log("Enemy " + currhp);
		yield return new WaitForSecondsRealtime(2);
		canGetdmgR = true;
	}
}
