using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //public CameraFollow follow;

    public Rigidbody2D rb;          //funkce pro gravitaci
    private float activeX;          //promenna pro ulozeni zda se hybe ci ne
    public float movespeed = 5;     //rychlost pohybu objektu

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        activeX = Input.GetAxis("Horizontal");  //definice pohybu
        rb.velocity = new Vector2((movespeed * activeX), rb.velocity.y); //pohyb
    }
}
