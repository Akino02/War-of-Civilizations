using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Timers;

public class ButtonsMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingMenu;


    public static float volume;
    public Slider volumeBar;
    public Text showVolumeValue;

    public AudioSource testSound;
    bool isPlaying = false;
    public Text showPlayButtonText;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);                       //nastaveni ze toto menu bude videt pri startu
        settingMenu.SetActive(false);                   //toto menu bude pri startu vypnute
        volumeBar.value = CameraFollow.songInGame;      //nastaveni zvuku do hry
        showPlayButtonText.text = "Play";               //nastaveni nazvu buttonu

        //testSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        volume = volumeBar.value;                       //ziskani hodnoty z posouvadla
        showVolumeValue.text = "Volume: " + math.round(volume*100) + "%";     //dosazeni textu pri meneni hodnoty zvuku
    }

    //MainMenu buttons
    public void Play()          //Funkce pro vstoupeni do hry
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Setting()              //Funkce pro zmenu nastaveni
    {
        mainMenu.SetActive(false);
        settingMenu.SetActive(true);

        //nastavise hodnota textu na play a jeste bool
        showPlayButtonText.text = "Play";
        isPlaying = false;
    }
    public void QuitGame()          //Funkce pro vypnuti hry
    {
        Application.Quit();
    }

    public void SongAuthorButtonLink()      //odkaz na autora hudby ve hre
    {
        Application.OpenURL("https://www.youtube.com/@WaterflameMusic/videos");
    }

    //SettingMenu buttons
    public void Back()              //Funkce pro navrat z vedlejsiho menu do hlavniho (pokud bude vice menu tak se to da do array)
    {
        mainMenu.SetActive(true);
        settingMenu.SetActive(false);
        testSound.Stop();           //vypne se hudba, jakmile jde mimo nastaveni
    }
    /*public IEnumerator PlaySoundTimer()          //pokud se zmeni hodnota tak se pusti testovaci zvuk                   !!udelat lepsi mby
    {
        testSound.Play();
        testSound.volume = volume;
        yield return new WaitForSeconds(timeForTest);
        testSound.Stop();
    }*/
    public void PlaySound()                     //metoda pro button na test zvuku (Hudba)                                   !!udelat to lepsi fr
    {
        if (!isPlaying)
        {
            testSound.volume = volume;
            isPlaying = true;
            testSound.Play();
            showPlayButtonText.text = "Stop";
        }
        else
        {
            isPlaying = false;
            testSound.Stop();
            showPlayButtonText.text = "Play";
        }
    }
    public void OnChangeVolume()                //pri zmene se nastavi hlasitost hudby
    {
        testSound.volume = volume;
    }
}
