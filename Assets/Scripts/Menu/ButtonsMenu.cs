using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()          //Funkce pro vstoupeni do hry
    {
        SceneManager.LoadScene("GameScene");
    }
    void Setting()              //Funkce pro zmenu nastaveni
    {

    }
    public void QuitGame()          //Funkce pro vypnuti hry
    {
        Application.Quit();
    }
}
