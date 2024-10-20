using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public AudioSource song;

    //objekt ktery bude pronasledovan
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        song.volume = ButtonsMenu.volumeSong;
    }

    // Update is called once per frame
    void Update()
    {
        //bude pronasledovat dany objekt aby se hybal a jeho pozice je rovna targetu
        transform.position = new Vector3(target.position.x,target.position.y + 6, -10);
    }
}
