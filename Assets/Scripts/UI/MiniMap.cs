using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    CameraMove cameraMove;
    public GameObject cameraObjectMove;

    public float actualPosition;
    public float positionProcent = 0f;

    public Scrollbar bar;

    // Start is called before the first frame update
    void Start()
    {
        bar = gameObject.GetComponent<Scrollbar>();
        cameraMove = cameraObjectMove.GetComponent<CameraMove>();
    }

    // Update is called once per frame
    void Update()
    {
        positionProcent = (cameraMove.transform.position.x / 20);
        bar.value = positionProcent;
    }
}
