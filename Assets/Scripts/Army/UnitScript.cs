using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UnitScript : MonoBehaviour
{
    //importovane scripty (hrace)
    [Header("Importing scripts")]
    EvolutionPlayerScript evolutionPlayerS;

    //importovane scripty (nepritele)
    EvolutionEnemyScript evolutionEnemyS;


    //pro import animatoru
    //animace
    public Animator animator;

	//sound
	public AudioSource attackSound;



	//
	[Header("Opponent layer")]
    //layer nepratelskych jednotek typu soldier
    public LayerMask opponent;
    //layer nepratelske zakladny
    public LayerMask opponentBase;

    //rychlost pohybu objektu
    public float movespeed;

	//typ jednotky
	[Header("Unit type")]
	public LayerMask armyType;

	[Header("Unit types")]
	public LayerMask[] armyTypeLayer;
    //Layer pratelskych vojacku
    public LayerMask allies;
    //toto definuje jaky je to typ vojaka
    public int armyTypeNum = 0;

	[Header("Attributes")]
	public float currhp;
	public float unitRange;

    //uchovani urovne vojacka
    public int lvl = 0;


	//change from private to public
	public int[] moveDir = { 1, -1, 0 };
	public UnitTeam team;
	private int teamInt => (int)team;

    //zda vidi nepritele, zda vidi zakladnu nepritele, zda vidi spojence
    public bool[] checkCollision = { false, false, false };

	public Vector3 distanceFromAllie;

    //zda muze bojovat
    public bool canGiveDmg = false;
    //zda nasel nepritele
    public bool foundEnemy = false;


	public float chargeAttack = 0f;

    //public bool isDead = false;

    // Start is called before the first frame update
    private void Awake()
    {
        //toto najde zakladnu hrace pomoci tagu ktery ma
        GameObject script1 = GameObject.FindWithTag("baseP");
        evolutionPlayerS = script1.GetComponent<EvolutionPlayerScript>();

        //toto najde zakladnu nepritele pomoci tagu ktery ma
        GameObject script2 = GameObject.FindWithTag("baseE");
        evolutionEnemyS = script2.GetComponent<EvolutionEnemyScript>();


    }
    void Start()
	{
		//Sound ziskani hodnot z menu
		attackSound.volume = ButtonsMenu.volumeSFX;

		if (team == UnitTeam.Left)
		{
            //urceni levelu, ktery bude mit jednotky hrace
            lvl = evolutionPlayerS.level;
		}
		else
		{
            //urceni levelu, ktery bude mit jednotky nepritele
            lvl = evolutionEnemyS.level;
		}

		int index;
		for (index = 0; index < armyTypeLayer.Length; index++)
		{
			if(armyType == armyTypeLayer[index])
            {
                break;
            }
        }

        armyTypeNum = index;
        currhp = UnityConfiguration.maxhp[index] * (lvl+1);
        canGiveDmg = true;
        //kdyz layer je roven range, tak dostane vetsi pole dostrelu, jinak melee
        if (index == 1)
        {
            unitRange = UnityConfiguration.ranges[1];
        }
        else
        {
            unitRange = UnityConfiguration.ranges[0];
        }
		//nastaveni animace, tim se nastavi vzhled aktualniho levelu a tridy dane jednotky
        animator.SetInteger("Class", index + 1);
        animator.SetInteger("Level", lvl);

    }
	// Update is called once per frame
	void Update()
	{
		Animation();
	}
	
	private void Animation()
	{
		if (checkCollision[0] || checkCollision[1])
		{
			animator.SetBool("EnemyNear", true);
			return;
		}
		animator.SetBool("EnemyNear", false);
		return;
	}
}

public enum UnitTeam { 
	Left , Right
}