using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionEnemyScript : MonoBehaviour
{
    EnemySpawn enemyS;
    HpScript hpEnemyS;
    EvolutionPlayerScript evolutionPlayerS;

    //aktualni level zakladny
    public int level = 0;

    //cas byl upraven a jeste podminka pro evoluce
    public int lvlTypeWait = 15;
    public bool evolving = false;

    //procenta zkusenosti od, kterych se zacne vylepsovat enemy
    private int EvolveExperiencePro = 90;

    //vzhled budov v array ohledne nove evoluce
    public GameObject[] baseAppearance = new GameObject[5];

    private void Awake()
    {
        enemyS = GetComponent<EnemySpawn>();
        hpEnemyS = GetComponent<HpScript>();

        //toto najde zakladnu hrace pomoci tagu ktery ma
        GameObject item = GameObject.FindWithTag("baseP");
        evolutionPlayerS = item.GetComponent<EvolutionPlayerScript>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        if (level != UnityConstants.maxLevelIndex && !GameScript.isGameOver)
        {
            StartCoroutine(Evolution());
        }
    }
    //toto bude primo pro enemy system pro evoluce
    IEnumerator Evolution()
    {
        if (evolving == false && level != UnityConstants.maxLevelIndex && enemyS.isCreatingUnit == false)
        {
            //urcit jinak podminku
            if (evolutionPlayerS.experience >= (UnityConfiguration.nextlevelup * EvolveExperiencePro) / UnityConstants.maxPercentage && evolutionPlayerS.level == level)
            {
                evolving = true;
                yield return new WaitForSeconds(lvlTypeWait);
                level += 1;
                evolving = false;

                for (int i = 0; i < UnityConstants.maxNumberOfBaseAppearance; i++)
                {
                    if (level == i)
                    {
                        baseAppearance[i].SetActive(true);
                    }
                    else
                    {
                        baseAppearance[i].SetActive(false);
                    }
                }
                hpEnemyS.UpgradeHp();
            }
            else if (evolutionPlayerS.level > level)
            {
                evolving = true;
                yield return new WaitForSeconds(lvlTypeWait);
                level += 1;
                evolving = false;

                for (int i = 0; i < UnityConstants.maxNumberOfBaseAppearance; i++)
                {
                    if (level == i)
                    {
                        baseAppearance[i].SetActive(true);
                    }
                    else
                    {
                        baseAppearance[i].SetActive(false);
                    }
                }
                hpEnemyS.UpgradeHp();
            }
        }
    }
}