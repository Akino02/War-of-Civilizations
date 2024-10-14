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
    /*public Text showPlayButtonTextSong;
    public Text showPlayButtonTextSFX;*/

    public float[] stopAfterTest = {0f, 0f};
    public int testSoundLength = 2;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);                           //nastaveni ze toto menu bude videt pri startu
        settingMenu.SetActive(false);                       //toto menu bude pri startu vypnute
        volumeBarSong.value = volumeSong;      //nastaveni zvuku do hry
        volumeBarSFX.value = volumeSFX;              //nastaveni zvuku do hry

        /*volumeSong = volumeBarSong.value;                       //ziskani hodnoty z posouvadla
        volumeSFX = volumeBarSFX.value;                         //ziskani hodnoty z posouvadla*/
        /*showPlayButtonTextSong.text = "Play";
        showPlayButtonTextSFX.text = "Play";*/

        //testSound = GetComponent<AudioSource>();
        //changeCameraName.text = UnityConfiguration.cameraTypeName[UnityConfiguration.cameraMoveType];
        ChangeCameraImage();
    }

    // Update is called once per frame
    void Update()
    {
        //test
        PlaySound();

        if (isPlayingSong || isPlayingSFX)
        {
            StopTimerSound();
        }

        volumeSong = volumeBarSong.value;                       //ziskani hodnoty z posouvadla
        volumeSFX = volumeBarSFX.value;                         //ziskani hodnoty z posouvadla
        showVolumeValueSong.text = "Volume: " + math.round(volumeSong * 100) + "% Song";     //dosazeni textu pri meneni hodnoty zvuku
        showVolumeValueSFX.text = "Volume: " + math.round(volumeSFX * 100) + "% SFX";     //dosazeni textu pri meneni hodnoty zvuku
        menuSongSoung.volume = volumeSong;
    }

    //MainMenu buttons
    public void Play()          //Funkce pro vstoupeni do hry
    {
        SceneManager.LoadScene("GameScene");
        GameScript.isGameOver = false;                           //Zajistuje ze kdyz jde do hry tak hra bude nova
    }
    public void Setting()              //Funkce pro zmenu nastaveni
    {
        mainMenu.SetActive(false);
        settingMenu.SetActive(true);
        isPlayingSong = false;
        isPlayingSFX = false;

        //nastavise hodnota textu na play a jeste bool
        /*showPlayButtonTextSong.text = "Play";
        showPlayButtonTextSFX.text = "Play";*/
    }
    public void QuitGame()          //Funkce pro vypnuti hry
    {
        Application.Quit();
    }

    public void SongAuthorButtonLink()      //odkaz na autora hudby ve hre
    {
        Application.OpenURL("https://www.youtube.com/@WaterflameMusic/videos");
    }
    public void ToggleKeyboard()
    {
        //MoveType[] controllPresets = {MoveType.Keyboard, MoveType.Mouse, MoveType.Keyboard | MoveType.Mouse};
        /*if (UnityConfiguration.cameraMoveType < )
        {
            UnityConfiguration.cameraMoveType++;
        }
        else
        {
            UnityConfiguration.cameraMoveType = 0;
        }
        ChangeCameraImage();*/
        //bool isMouse = (UnityConfiguration.cameraMoveType.HasFlag(MoveType.Mouse));
        bool isKeyboard = (UnityConfiguration.cameraMoveType.HasFlag(MoveType.Keyboard));
        if (isKeyboard)
        {
            UnityConfiguration.cameraMoveType = MoveType.Mouse;
        }
        else
        {
            UnityConfiguration.cameraMoveType = UnityConfiguration.cameraMoveType | MoveType.Keyboard;
        }
        Debug.Log(isKeyboard + " Keyboard");
        ChangeCameraImage();
    }
    public void ToggleMouse()
    {
        /*if (UnityConfiguration.cameraMoveType > 0)
        {
            UnityConfiguration.cameraMoveType--;
        }
        else
        {
            UnityConfiguration.cameraMoveType = 2;
        }
        ChangeCameraImage();*/
        bool isMouse = (UnityConfiguration.cameraMoveType.HasFlag(MoveType.Mouse));
        if (isMouse)
        {
            UnityConfiguration.cameraMoveType = MoveType.Keyboard;
        }
        else
        {
            UnityConfiguration.cameraMoveType = UnityConfiguration.cameraMoveType | MoveType.Mouse;
        }
        Debug.Log(isMouse);
        ChangeCameraImage();
    }
    private void ChangeCameraImage()
    {
        //podminka ? true : false
        Color showImage = new Color(1, 1, 1, 1);
        Color dimImage = new Color(1, 1, 1, dimPercentage);
        bool isKeyboard = (UnityConfiguration.cameraMoveType.HasFlag(MoveType.Keyboard));
        bool isMouse = (UnityConfiguration.cameraMoveType.HasFlag(MoveType.Mouse));
        changeCameraKeyboard.color = isKeyboard ? showImage : dimImage;
        changeCameraMouse.color = isMouse ? showImage : dimImage;
    }

    //SettingMenu buttons
    public void Back()              //Funkce pro navrat z vedlejsiho menu do hlavniho (pokud bude vice menu tak se to da do array)
    {
        mainMenu.SetActive(true);
        settingMenu.SetActive(false);
        testSongSound.Stop();           //vypne se hudba, jakmile jde mimo nastaveni
        testSFXSound.Stop();
        isPlayingSong = false;
        isPlayingSFX = false;
    }
    //test
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
            //Debug.Log("Start timer");
        }

        if (volumeSFX != volumeBarSFX.value)
        {
            if (isPlayingSFX != true)
            {
                testSFXSound.Play();
            }
            stopAfterTest[1] = 0f;
            isPlayingSFX = true;
            //Debug.Log("Start timer");
        }

        testSongSound.volume = volumeSong;
        testSFXSound.volume = volumeSFX;
    }
    private void StopTimerSound()
    {
        for (int i = 0; i < stopAfterTest.Length; i++)
        {
            stopAfterTest[i] = Mathf.Lerp(stopAfterTest[i], stopAfterTest[i] + 1f, Time.deltaTime / testSoundLength);
            //Debug.Log("Somethingggg");
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
                //Debug.Log("StopMusic");
            }
        }
    }
    /*public void PlaySoundSong()                     //metoda pro button na test zvuku (Hudba)                                   !!udelat to lepsi fr
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
    }*/
}
