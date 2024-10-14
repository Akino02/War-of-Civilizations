using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionPlayerScript : MonoBehaviour
{
    ProgresScript progS;
    HpScript hpPlayerS;

    //zkusenosti
    public int experience = 0;

    //zkusenosti v procentech
    public int experienceInPercentage = 0;

    //pokud dosahne tolika zkusenosti tak se evolvuje
    //public int nextlevelup = 4000;

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
        //progS = GetComponent<ProgresScript>();
        hpPlayerS = GetComponent<HpScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        experienceText.text = experienceInPercentage.ToString() + "%";

        //na zacatku se definuje co tam bude na tom buttonu
        for (int i = 0; i < UnityConstants.numberOfProductionButtons; i++)
        {
            actionButtonText[i].text = "lvl." + (level + 1);
            actionButtonText[i + (actionButtonText.Length) / 2].text = "Cost " + UnityConfiguration.moneyperunit[level, i] + " $";
        }
    }

    // Update is called once per frame
    void Update()
    {
        Evolution();
        ExperienceBar();
    }

    //docasne dokud neni button tak se to evolvuje automaticky
    void Evolution()
    {
        //pokud jeho level neni roven 4 coz je nejvysi uroven tak se muze vylepsit
        if (experience >= UnityConfiguration.nextlevelup && level != UnityConstants.maxLevelIndex)
        {
            experience -= UnityConfiguration.nextlevelup;
            level += 1;

            //pise do vsech textu, ktere jsou uchovany v poli
            for (int i = 0; i < actionButtonText.Length / UnityConstants.numberOfTextFieldsInProductionButton; i++)
            {
                actionButtonText[i].text = "lvl." + (level + 1);
                actionButtonText[i + (actionButtonText.Length) / UnityConstants.numberOfTextFieldsInProductionButton].text = "Cost " + UnityConfiguration.moneyperunit[level, i] + " $";
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
        else if (level == UnityConstants.maxLevelIndex)
        {
            experienceText.text = "Max";
        }
        else
        {
            experienceText.text = experienceInPercentage.ToString() + "%";
        }
    }
    void ExperienceBar()
    {
        //opravit MATHF.LERP
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
    }
}