using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
	//tento script funguje na pohyblivost obrazu, takze ho vlastni kamera a taky slunce
	public Rigidbody2D rb;                                      //funkce pro gravitaci
	private float activeX;                                      //promenna pro ulozeni zda se hybe ci ne
	public float movespeed = 3;                                 //rychlost pohybu objektu

    private int touchField = 10;                                //velikost pole pro mys, po najeti do pole se bude tam pohybovat kamera

    public MoveType moveType;

	//promenne pro border kamery
    float borderLposX;
    public GameObject playerBase;
    public GameObject borderL;
    float borderRposX;
    public GameObject enemyBase;
    public GameObject borderR;

    public GameObject sun;
    public GameObject widthFromSun;

    // Start is called before the first frame update
    void Start()
	{

        //definice hranic pro kameru
        borderLposX = playerBase.transform.position.x + 7;
        borderRposX = enemyBase.transform.position.x - 7;

        borderL.transform.position = new Vector2(borderLposX, transform.position.y);
        borderR.transform.position = new Vector2(borderRposX, transform.position.y);
	}

	// Update is called once per frame
	void Update()
	{
        //Debug.Log(moveType);

        if (!LogScript.isGameOver)
        {
            //pohyb pomoci myse
            if (Input.mousePosition.x < Screen.width / touchField && Input.mousePosition.y < (Screen.height * 3) / 4 && Input.mousePosition.y > Screen.height/ touchField && (moveType == MoveType.Mouse || moveType == MoveType.MouseKeyboard))                        //pokud je mys left
            {
                activeX = -1;
            }
            else if (Input.mousePosition.x > Screen.width - Screen.width / touchField && Input.mousePosition.y < (Screen.height * 3) / 4 && Input.mousePosition.y > Screen.height / touchField && (moveType == MoveType.Mouse || moveType == MoveType.MouseKeyboard))   //pokud je mys right
            {
                activeX = 1;
            }
            //
            //pohyb pomoci klavesnice (definice pohybu(ten pohyb je urcen pro A, D a taky pro levou sipku a pravou sipku))
            else if (moveType == MoveType.Keyboard || moveType == MoveType.MouseKeyboard)
            {
                activeX = Input.GetAxisRaw("Horizontal");
            }
            //
            else
            {
                activeX = 0;
            }
        }
        rb.velocity = new Vector2((movespeed * activeX), rb.velocity.y);                                                          //pohyb kamery
        sun.transform.position = new Vector3(widthFromSun.transform.position.x + transform.position.x, sun.transform.position.y, sun.transform.position.z);     //nastaveni pozice pro slunce
    }
}

public enum MoveType
{
    MouseKeyboard, Keyboard, Mouse
}