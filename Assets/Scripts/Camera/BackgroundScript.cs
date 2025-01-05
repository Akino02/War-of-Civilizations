using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    //array s obrazky biomu
    public Sprite[] backgroundImages = new Sprite[2];

    //image, ktery se meni
    public SpriteRenderer backgroundObject;

    private void Awake()
    {
        //nahodny vyber pozadi pri spusteni sceny
        backgroundObject.sprite = backgroundImages[Random.Range(0, UnityConstants.numberOfBackgroundImages)];
    }

    /*// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
