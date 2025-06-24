using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseMenu;

    public bool isGamePause = false;

    //sound settings
    public Slider volumeBarSong;
    public Slider volumeBarSFX;
    public Text showVolumeValueSong;
    public Text showVolumeValueSFX;

    // Start is called before the first frame update
    void Start()
    {
        //Pro jistotu
        //pauseMenu.SetActive(false);

        volumeBarSong.value = ButtonsMenu.volumeSong;
        volumeBarSFX.value = ButtonsMenu.volumeSFX;
    }

    // Update is called once per frame
    void Update()
    {
        showVolumeValueSong.text = "Volume: " + math.round(volumeBarSong.value * 100) + "% Song";
        showVolumeValueSFX.text = "Volume: " + math.round(volumeBarSFX.value * 100) + "% SFX";
    }
    public void PauseGameMenu()
    {
        isGamePause = !isGamePause;
        Time.timeScale = isGamePause ? 0f : 1f;
        pauseMenu.SetActive(isGamePause);

        volumeBarSong.value = ButtonsMenu.volumeSong;
        volumeBarSFX.value = ButtonsMenu.volumeSFX;
    }

    public void ChangeVolume()
    {
        if (isGamePause && (volumeBarSong.value != ButtonsMenu.volumeSong || volumeBarSFX.value != ButtonsMenu.volumeSFX))
        {
            ButtonsMenu.volumeSong = volumeBarSong.value;
            ButtonsMenu.volumeSFX = volumeBarSFX.value;
        }
    }
}
