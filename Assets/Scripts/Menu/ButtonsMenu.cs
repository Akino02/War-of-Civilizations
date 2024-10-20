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


    public static float volumeSong = 0.35f;
    public static float volumeSFX = 0.35f;
    public Slider volumeBarSong;
    public Slider volumeBarSFX;
    public Text showVolumeValueSong;
    public Text showVolumeValueSFX;

    public Image changeCameraKeyboard;
    public Image changeCameraMouse;

    public AudioSource testSongSound;
    public AudioSource testSFXSound;
    public AudioSource menuSongSoung;

    [Range(0f, 1f)]
    public float dimPercentage = 0.35f;

    bool isPlayingSong = false;
    bool isPlayingSFX = false;

    public float[] stopAfterTest = {0f, 0f};
    public int testSoundLength = 2;

    // Start is called before the first frame update
    void Start()
    {
        //nastaveni ze toto menu bude videt pri startu
        mainMenu.SetActive(true);
        //toto menu bude pri startu vypnute
        settingMenu.SetActive(false);

        //nastaveni zvuku do hry
        volumeBarSong.value = volumeSong;
        volumeBarSFX.value = volumeSFX;

        ChangeCameraImage();
    }

    // Update is called once per frame
    void Update()
    {
        PlaySound();

        if (isPlayingSong || isPlayingSFX)
        {
            StopTimerSound();
        }

        //ziskani hodnoty z posouvadla
        volumeSong = volumeBarSong.value;
        volumeSFX = volumeBarSFX.value;

        //dosazeni textu pri meneni hodnoty zvuku
        showVolumeValueSong.text = "Volume: " + math.round(volumeSong * 100) + "% Song";
        showVolumeValueSFX.text = "Volume: " + math.round(volumeSFX * 100) + "% SFX";

        menuSongSoung.volume = volumeSong;
    }

    //MainMenu buttons
        //Funkce pro vstoupeni do hry
    public void Play()
    {
        SceneManager.LoadScene("GameScene");

        //Zajistuje ze kdyz jde do hry tak hra bude nova
        GameScript.isGameOver = false;
    }

        //Funkce pro zmenu nastaveni
    public void Setting()
    {
        mainMenu.SetActive(false);
        settingMenu.SetActive(true);
        isPlayingSong = false;
        isPlayingSFX = false;
    }
        //Funkce pro vypnuti hry
    public void QuitGame()
    {
        Application.Quit();
    }

        //odkaz na autora hudby ve hre
    public void SongAuthorButtonLink()
    {
        Application.OpenURL("https://www.youtube.com/@WaterflameMusic/videos");
    }

    public void ToggleKeyboard()
    {
        bool isKeyboard = (UnityConfiguration.cameraMoveType.HasFlag(MoveType.Keyboard));
        if (isKeyboard)
        {
            UnityConfiguration.cameraMoveType = MoveType.Mouse;
        }
        else
        {
            UnityConfiguration.cameraMoveType = UnityConfiguration.cameraMoveType | MoveType.Keyboard;
        }
        //Debug.Log(isKeyboard + " Keyboard");
        ChangeCameraImage();
    }
    public void ToggleMouse()
    {
        bool isMouse = (UnityConfiguration.cameraMoveType.HasFlag(MoveType.Mouse));
        if (isMouse)
        {
            UnityConfiguration.cameraMoveType = MoveType.Keyboard;
        }
        else
        {
            UnityConfiguration.cameraMoveType = UnityConfiguration.cameraMoveType | MoveType.Mouse;
        }
        //Debug.Log(isMouse);
        ChangeCameraImage();
    }
    private void ChangeCameraImage()
    {
        Color showImage = new Color(1, 1, 1, 1);
        Color dimImage = new Color(1, 1, 1, dimPercentage);
        bool isKeyboard = (UnityConfiguration.cameraMoveType.HasFlag(MoveType.Keyboard));
        bool isMouse = (UnityConfiguration.cameraMoveType.HasFlag(MoveType.Mouse));

        //podminka ? true : false
        //jedna se o ternarni operator
        changeCameraKeyboard.color = isKeyboard ? showImage : dimImage;
        changeCameraMouse.color = isMouse ? showImage : dimImage;
    }

    //SettingMenu buttons
    //Funkce pro navrat z vedlejsiho menu do hlavniho (pokud bude vice menu tak se to da do array)
    public void Back()
    {
        mainMenu.SetActive(true);
        settingMenu.SetActive(false);

        //vypne se hudba, jakmile jde mimo nastaveni (pro jistotu aby se zabranilo lehke chybe)
        testSongSound.Stop();
        testSFXSound.Stop();
        isPlayingSong = false;
        isPlayingSFX = false;
    }

    //prehravani hudby a nasledne i dosazeni hodnot, aby to melo odpovidajici hlasitost
    private void PlaySound()
    {
        if (volumeSong != volumeBarSong.value)
        {
            if (isPlayingSong != true)
            {
                testSongSound.Play();
            }
            stopAfterTest[0] = 0f;
            isPlayingSong = true;
        }

        if (volumeSFX != volumeBarSFX.value)
        {
            if (isPlayingSFX != true)
            {
                testSFXSound.Play();
            }
            stopAfterTest[1] = 0f;
            isPlayingSFX = true;
        }

        testSongSound.volume = volumeSong;
        testSFXSound.volume = volumeSFX;
    }

    //po dosazeni casoveho limitu se vypne hudba
    private void StopTimerSound()
    {
        for (int i = 0; i < stopAfterTest.Length; i++)
        {
            stopAfterTest[i] = Mathf.Lerp(stopAfterTest[i], stopAfterTest[i] + 1f, Time.deltaTime / testSoundLength);
            if (stopAfterTest[i] >= 1f)
            {
                if(i == 0)
                {
                    isPlayingSong = false;
                    testSongSound.Stop();
                }
                else if(i == 1)
                {
                    isPlayingSFX = false;
                    testSFXSound.Stop();
                }
                stopAfterTest[i] = 0f;
            }
        }
    }
}
