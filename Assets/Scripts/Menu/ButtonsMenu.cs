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


    public static float volumeSong;
    public static float volumeSFX;
    public Slider volumeBarSong;
    public Slider volumeBarSFX;
    public Text showVolumeValueSong;
    public Text showVolumeValueSFX;

    public AudioSource testSongSound;
    public AudioSource testSFXSound;

    bool isPlayingSong = false;
    bool isPlayingSFX = false;
    public Text showPlayButtonTextSong;
    public Text showPlayButtonTextSFX;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);                       //nastaveni ze toto menu bude videt pri startu
        settingMenu.SetActive(false);                   //toto menu bude pri startu vypnute
        volumeBarSong.value = CameraFollow.songInGame;      //nastaveni zvuku do hry
        volumeBarSFX.value = UniArmy.sfxSound;      //nastaveni zvuku do hry
        showPlayButtonTextSong.text = "Play";
        showPlayButtonTextSFX.text = "Play";

        //testSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        volumeSong = volumeBarSong.value;                       //ziskani hodnoty z posouvadla
        volumeSFX = volumeBarSFX.value;                       //ziskani hodnoty z posouvadla
        showVolumeValueSong.text = "Volume: " + math.round(volumeSong * 100) + "% Song";     //dosazeni textu pri meneni hodnoty zvuku
        showVolumeValueSFX.text = "Volume: " + math.round(volumeSFX * 100) + "% SFX";     //dosazeni textu pri meneni hodnoty zvuku
    }

    //MainMenu buttons
    public void Play()          //Funkce pro vstoupeni do hry
    {
        SceneManager.LoadScene("GameScene");
        LogScript.isGameOver = false;                           //Zajistuje ze kdyz jde do hry tak hra bude nova
    }
    public void Setting()              //Funkce pro zmenu nastaveni
    {
        mainMenu.SetActive(false);
        settingMenu.SetActive(true);

        //nastavise hodnota textu na play a jeste bool
        showPlayButtonTextSong.text = "Play";
        showPlayButtonTextSFX.text = "Play";
        isPlayingSong = false;
        isPlayingSFX = false;
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
        testSongSound.Stop();           //vypne se hudba, jakmile jde mimo nastaveni
        testSFXSound.Stop();
    }
    /*public IEnumerator PlaySoundTimer()          //pokud se zmeni hodnota tak se pusti testovaci zvuk                   !!udelat lepsi mby
    {
        testSound.Play();
        testSound.volume = volume;
        yield return new WaitForSeconds(timeForTest);
        testSound.Stop();
    }*/
    public void PlaySoundSong()                     //metoda pro button na test zvuku (Hudba)                                   !!udelat to lepsi fr
    {
        if (!isPlayingSong)
        {
            testSongSound.volume = volumeSong;
            isPlayingSong = true;
            testSongSound.Play();
            showPlayButtonTextSong.text = "Stop";
        }
        else
        {
            isPlayingSong = false;
            testSongSound.Stop();
            showPlayButtonTextSong.text = "Play";
        }
    }
    public void PlaySoundSFX()                     //metoda pro button na test zvuku (Hudba)                                   !!udelat to lepsi fr
    {
        if (!isPlayingSFX)
        {
            testSFXSound.volume = volumeSFX;
            isPlayingSFX = true;
            testSFXSound.Play();
            showPlayButtonTextSFX.text = "Stop";
        }
        else
        {
            isPlayingSFX = false;
            testSFXSound.Stop();
            showPlayButtonTextSFX.text = "Play";
        }
    }
    public void OnChangeVolume()                //pri zmene se nastavi hlasitost hudby
    {
        testSongSound.volume = volumeSong;
        testSFXSound.volume = volumeSFX;
    }
}
