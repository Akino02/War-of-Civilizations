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

	public bool canGetdmgM = true;
	public bool canGetdmgR = true;
	//
	//spawnovani jednotek
	public bool canSpawn = true;
	private int nahoda = 0;
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
		//toto slouzi pro spawn vojaku
		if (canSpawn == true && nahoda >= 1 && nahoda <= 3 && currHPBase > 0)
		{
			StartCoroutine(CoolDownArmySpawn());
		}
		else if(nahoda == 0 || nahoda >= 4)
		{
			nahoda = Random.Range(1, 5);
		}

		//damage pro base
		if ((Physics2D.OverlapCircle(basePosition.transform.position, 0.7f, opponentSoldier) != null || Physics2D.OverlapCircle(basePosition.transform.position, 0.7f, opponentTank) != null) && canGetdmgM == true && currHPBase > 0)  //nejaky nepritel muze ubrat zivoty zakladny
		{
			StartCoroutine(DmgdealcooldownMelee());
		}
		if (Physics2D.OverlapCircle(basePosition.transform.position, 1.4f, opponentRanger) != null && canGetdmgR == true && currHPBase > 0)
		{
			StartCoroutine(DmgdealcooldownRange());
		}
		hpBaseBarcurr.fillAmount = hpbaseinprocents;  //urcovani zivotu v procentech
	}

	IEnumerator CoolDownArmySpawn()      //nastaveni na prestavku at nemuze to spamovat to klikani a spawnovani
	{
		canSpawn = false;
		if(nahoda == 1)
		{
			yield return new WaitForSecondsRealtime(5);
			Instantiate(soldier, enemySpawner.transform.position, enemySpawner.transform.rotation);
		}
		else if(nahoda == 2)
		{
			yield return new WaitForSecondsRealtime(8);
			Instantiate(ranger, enemySpawner.transform.position, enemySpawner.transform.rotation);
		}
		else if(nahoda == 3)
		{
			yield return new WaitForSecondsRealtime(10);
			Instantiate(tank, enemySpawner.transform.position, enemySpawner.transform.rotation);
		}
		nahoda = Random.Range(1, 5);
		canSpawn = true;
	}
	//base bude dostavat dmg od enemy
	IEnumerator DmgdealcooldownMelee()
	{
		canGetdmgM = false;
		if (Physics2D.OverlapCircle(basePosition.transform.position, 0.7f, opponentSoldier) != null)
		{
			currHPBase -= soldierPscript.dmg[soldierPscript.level, 0];                                                      //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
        }
		else if (Physics2D.OverlapCircle(basePosition.transform.position, 0.7f, opponentTank) != null)
		{
			currHPBase -= soldierPscript.dmg[soldierPscript.level, 2];                                                      //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
        }
		Debug.Log("Player " + currHPBase);
		yield return new WaitForSeconds(3);
		canGetdmgM = true;
	}
	IEnumerator DmgdealcooldownRange()
	{
		canGetdmgR = false;
		currHPBase -= soldierPscript.dmg[soldierPscript.level, 1];                                                          //potrebuje sledovani !!!!!!!!!!!!!!!!!!!!!!!!*******
        Debug.Log("Player " + currHPBase);
		yield return new WaitForSecondsRealtime(2);
		canGetdmgR = true;
	}
}
