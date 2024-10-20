using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameScript : MonoBehaviour
{
    HpScript hpPlayerS;
    HpScript hpEnemyS;

    public GameObject warning;
    public Text placeText;

    public bool canShow = true;
    public float showTextTimer = 2f;

    public Text winnerText;
    public GameObject endGameMenu;
    public static bool isGameOver = false;

    public GameObject escapeButtonBack;

    private void Awake()
    {
        //toto najde zakladnu nepritele pomoci tagu ktery ma
        GameObject objectOfScript = GameObject.FindWithTag("baseE");
        hpEnemyS = objectOfScript.GetComponent<HpScript>();

        //propojeni scriptu s zivoty hrace
        hpPlayerS = GetComponent<HpScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        warning.SetActive(false);
        endGameMenu.SetActive(false);
        escapeButtonBack.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //kontrola zda se furt hraje
        if (hpPlayerS.currHPBase <= 0 || hpEnemyS.currHPBase <= 0)
        {
            isGameOver = true;
            endGameMenu.SetActive(true);
            escapeButtonBack.SetActive(false);
            //Debug.Log("Game is Over but why");
            Debug.Log(hpPlayerS.currHPBase);
            if(hpPlayerS.currHPBase <= 0)
            {
                winnerText.text = "You Lost!";
            }
            else
            {
                winnerText.text = "You Won!";
            }
        }
    }
    //Funkce ukazuje uzivateli ze nemuze neco provest
    public IEnumerator ShowText()
    {
        canShow = false;
        warning.SetActive(true);
        yield return new WaitForSeconds(showTextTimer);
        warning.SetActive(false);
        canShow = true;
    }
}
