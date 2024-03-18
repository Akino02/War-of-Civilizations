using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //public CameraMove move;
    public AudioSource song;
    public static float songInGame = 0.15f;

    public Transform target;                                    //objekt ktery bude pronasledovan

    // Start is called before the first frame update
    void Start()
    {
        song.volume = ButtonsMenu.volumeSong;
        songInGame = song.volume;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x,target.position.y + 6, -10);                                     //bude pronasledovat dany objekt aby se hybal a jeho pozice je rovna targetu
    }
}
