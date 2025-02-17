using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionPlayerScript : MonoBehaviour
{
    //ProgresScript progS;
    HpScript hpPlayerS;
    GameScript logS;

    //zkusenosti
    public int experience = 0;

    //zkusenosti v procentech
    public int experienceInPercentage = 0;

    //toto ukazuje level evoluce v zakladu je to 0
    public int level = 0;

    public Image xpBar;

    //vzhled budov v array ohledne nove evoluce
    public GameObject[] baseAppearance = new GameObject[UnityConstants.maxNumberOfBaseAppearance];
    //Prehled ohledne dalsi evoluce v %
    public Text experienceText;

    //upraveni textu u buttonu soldier, ranger, tank
    //pozdeji budou jestev na dobrovolny update evoluce
    //format    lvl,lvl,lvl,cost,cost,cost
    public Text[] actionButtonText = new Text[UnityConstants.numberOfProductionButtons * 2];

    public float speedOfFill = 3f;

    private void Awake()
    {
        hpPlayerS = GetComponent<HpScript>();
        logS = GetComponent<GameScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        experienceText.text = experienceInPercentage.ToString() + "%";

        //na zacatku se definuje co tam bude na tom buttonu
        for (int i = 0; i < UnityConstants.numberOfProductionButtons; i++)
        {
            actionButtonText[i].text = "lvl." + (level + 1);
            actionButtonText[i + (actionButtonText.Length) / 2].text = "Cost " + (UnityConfiguration.moneyperunit[i] * (level+1)) + " $";
        }
    }

    // Update is called once per frame
    void Update()
    {
        ExperienceBar();
    }

    void ExperienceBar()
    {
        if (level != UnityConstants.maxLevelIndex)
        {
            //vytvori proceznta ze zkusenosti
            experienceInPercentage = ((UnityConstants.maxPercentage * experience) / UnityConfiguration.nextlevelup);
            xpBar.fillAmount = Mathf.Lerp(xpBar.fillAmount, (float)experienceInPercentage / UnityConstants.maxPercentage, speedOfFill * Time.deltaTime);
        }
        else
        {
            xpBar.fillAmount = 1f;
        }

        
        //vkladani procent do textu, ukazatel UI
        if(experience >= UnityConfiguration.nextlevelup || level == UnityConstants.maxLevelIndex)
        {
            experienceText.text = "100%";
        }
        else
        {
            experienceText.text = experienceInPercentage.ToString() + "%";
        }
    }

    //funkce pro button, ktery bude evolvovat hracovy jednotky a zakladnu
    public void EvolutionUpgrade()
    {
        if (!GameScript.isGameOver)
        {
            if (experience >= UnityConfiguration.nextlevelup && level != UnityConstants.maxLevelIndex)
            {
                experience -= UnityConfiguration.nextlevelup;
                level += 1;

                //pise do vsech textu, ktere jsou uchovany v poli
                for (int i = 0; i < actionButtonText.Length / UnityConstants.numberOfTextFieldsInProductionButton; i++)
                {
                    actionButtonText[i].text = "lvl." + (level + 1);
                    actionButtonText[i + (actionButtonText.Length) / UnityConstants.numberOfTextFieldsInProductionButton].text = "Cost " + (UnityConfiguration.moneyperunit[i] * (level + 1)) + " $";
                }

                //zde se zmeni vzhled zakladny
                for (int i = 0; i < baseAppearance.Length; i++)
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

                //pro vylepseni zivotu s tim, ze se zachova %
                hpPlayerS.UpgradeHp();
            }
            else
            {
                if (level != UnityConstants.maxLevelIndex)
                {
                    //Debug.Log("You can't evolve yet");
                    logS.placeText.text = UnityConfiguration.possibleText[4];
                    StartCoroutine(logS.ShowText());
                }
                else
                {
                    //Debug.Log("You have reached maximum level");
                    logS.placeText.text = UnityConfiguration.possibleText[5];
                    StartCoroutine(logS.ShowText());
                }
            }
        }

    }
}