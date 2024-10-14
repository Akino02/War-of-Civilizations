using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MiniMap : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    CameraMove cameraMove;
    public GameObject cameraObjectMove;

    public float actualPosition;
    public float positionProcent = 0f;

    public Scrollbar bar;
    public bool isInteractableBar = false;
    public MoveType moveTypeTMP;

    public GameObject borderL;
    public GameObject borderR;
    public float mapwidth;

    // Start is called before the first frame update
    void Start()
    {
        bar = gameObject.GetComponent<Scrollbar>();
        cameraMove = cameraObjectMove.GetComponent<CameraMove>();
        //Debug.Log(UnityConfiguration.cameraMoveType);
    }
    // Update is called once per frame
    void Update()
    {
        //pokud hrac se nepohybuje klavosou ci mysi, pokud ne tak muze pracovat s barems
        if (!isInteractableBar)
        {
            //zjisti aktualni pozici kameroveho bodu
            actualPosition = cameraMove.transform.position.x;

            //vypocita procenta mezi bodem borderL a bodem borderR
            positionProcent = (actualPosition - (borderL.transform.position.x + 1)) / ((borderR.transform.position.x - 1) - (borderL.transform.position.x + 1));

            //vlozi procenta do scrollbaru
            bar.value = positionProcent;
        }
        else
        {
            float range = (borderR.transform.position.x - 1) - (borderL.transform.position.x + 1);
            float findPositionOfBar;
            findPositionOfBar = (borderL.transform.position.x + 1) + (range * bar.value);
            //Debug.Log(findPositionOfBar);
            cameraObjectMove.transform.position = new Vector2(findPositionOfBar, cameraObjectMove.transform.position.y);
        }
    }
    //kontroluje zda muze hrac pohybovat s barem na zaklade vstupu klavesnice ci myse

    public void OnPointerEnter(PointerEventData eventData)
    {
        isInteractableBar = true;
        moveTypeTMP = UnityConfiguration.cameraMoveType;
        UnityConfiguration.cameraMoveType = MoveType.None;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isInteractableBar = false;
        UnityConfiguration.cameraMoveType = moveTypeTMP;
        moveTypeTMP = MoveType.None;
    }
}
