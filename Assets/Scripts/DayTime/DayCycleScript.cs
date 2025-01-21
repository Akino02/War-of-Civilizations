using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayCycleScript : MonoBehaviour
{
    public GameObject objectToBeMoved;

    [Range(-18, 18)] public float positionOfObjectX;

    private int minPositionX;
    private int maxPositionX = 18;
    private int maxPositionY = 3;

    public bool isDayTime = true;

    /*public int TimeToFinish = 10;
    public int objectSpeed = 80;*/

    public SpriteRenderer skyEnvironment;                   //max: 255 min: 150
    public SpriteRenderer groundEnvironment;                //max: 221 min: 130

    private float skyBrightness = 1;
    private float groundBrightness = 1;

    private float skyMinBright = 0.58f;
    private float groundMaxBright = 0.86f;
    private float groundMinBright = 0.5f;

    public int sunSet = 14;
    public int sunRise = 14;

    public Animator animatorTimeObject;

    //from 6 to 45

    // Start is called before the first frame update
    void Start()
    {
        minPositionX = -maxPositionX;
        positionOfObjectX = minPositionX;
    }

    // Update is called once per frame
    void Update()
    {
        TimeCycle();
        ChangeBrightness(isDayTime, positionOfObjectX, maxPositionX, sunSet, sunRise);
        ChangeObjectAnimator(isDayTime);
    }

    private void TimeCycle()
    {
        if (positionOfObjectX >= maxPositionX)
        {
            positionOfObjectX = minPositionX;
            isDayTime = !isDayTime;
            //change sprite function
            //change brightness of environment
        }
        //slouzi to k osetreni hodnoty takze musi byt v tomto rozsahu
        positionOfObjectX = Mathf.Clamp(positionOfObjectX + Time.deltaTime/2, minPositionX, maxPositionX);        //nebo 0.008f a Time.deltaTime  
        objectToBeMoved.transform.localPosition = new Vector2(positionOfObjectX, ObjectPositionY(positionOfObjectX, maxPositionX, maxPositionY));
    }

    private float ObjectPositionY(float positionX, int maxPosX, int maxPosY)
    {
        return maxPosY * Mathf.Sqrt(1 - (Mathf.Pow(positionX, 2) / Mathf.Pow(maxPosX, 2)));     //elipsa v kladne casti pro y
    }

    private void ChangeObjectAnimator(bool isDay)
    {
        animatorTimeObject.SetBool("IsDay", isDay);
    }

    private void ChangeBrightness(bool isDay, float positionOfObject, int maxPosX, int sunDown, int moonDown)
    {
        if (positionOfObject>= 14)
        {
            skyBrightness = isDay ? Mathf.Clamp(1 - (positionOfObject - sunDown) / (maxPosX - sunDown), skyMinBright, 1) : Mathf.Clamp((positionOfObject - moonDown) / (maxPosX - moonDown), skyMinBright, 1);
            groundBrightness = isDay ? Mathf.Clamp(1- (positionOfObject - sunDown) / (maxPosX- sunDown), groundMinBright, groundMaxBright) : Mathf.Clamp((positionOfObject - moonDown) / (maxPosX - moonDown), groundMinBright, groundMaxBright);
            //Debug.Log(positionOfObject/ maxPosX);
        }
        Color dimOfSky = new Color(skyBrightness, skyBrightness, skyBrightness, 1);
        Color dimOfGround = new Color(groundBrightness, groundBrightness, groundBrightness, 1);

        skyEnvironment.color = dimOfSky;
        groundEnvironment.color = dimOfGround;
    }
}
