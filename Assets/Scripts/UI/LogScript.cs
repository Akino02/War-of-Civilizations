using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LogScript : MonoBehaviour
{
    public GameObject warning;
    public GameObject reportScore;
    public Text placeText;
    private string[] possibleText = {"You don't have enough money", "You have a full queue", "You Won", "You Lost"};

    // Start is called before the first frame update
    void Start()
    {
        warning.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowText()
    {
        placeText.text = possibleText[0];
        warning.SetActive(true);
    }
    public void ShowReport()
    {
    }
}
