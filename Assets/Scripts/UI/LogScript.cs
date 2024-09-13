using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LogScript : MonoBehaviour
{
    EnemySpawn enemyS;
    HpScript hpS;

    public GameObject warning;
    //public GameObject reportScore;
    public Text placeText;
    public string[] possibleText = {"You don't have enough money", "You have a full queue", "You Won", "You Lost"};
    public bool canShow = true;

    public Text winnerText;
    public GameObject endGameMenu;
    public static bool isGameOver = false;

    public GameObject escapeButtonBack;

    // Start is called before the first frame update
    void Start()
    {
        GameObject objectOfScript = GameObject.FindWithTag("baseE");   //toto najde zakladnu nepritele pomoci tagu ktery ma
        enemyS = objectOfScript.GetComponent<EnemySpawn>();

        hpS = GetComponent<HpScript>();							//propojeni zakladnich scriptu pro funkci UI


        warning.SetActive(false);
        endGameMenu.SetActive(false);
        escapeButtonBack.SetActive(true);
    }

    // Update is called once per frame
    void Update()                               //kontrola zda se furt hraje
    {
        if (HpScript.currHPBase <= 0 || EnemySpawn.currHPBase <= 0)
        {
            isGameOver = true;
            endGameMenu.SetActive(true);
            escapeButtonBack.SetActive(false);
            Debug.Log("Game is Over but why");
            if(HpScript.currHPBase <= 0)
            {
                winnerText.text = "You Lost!";
            }
            else
            {
                winnerText.text = "You Won!";
            }
        }
    }
    public IEnumerator ShowText()               //Funkce ukazuje uzivateli ze nemuze neco provest
    {
        canShow = false;
        warning.SetActive(true);
        yield return new WaitForSeconds(2);
        warning.SetActive(false);
        canShow = true;
    }
}
