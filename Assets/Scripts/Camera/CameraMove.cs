using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //tento script funguje na pohyblivost obrazu, takze ho vlastni kamera a taky slunce
    //funkce pro gravitaci
    public Rigidbody2D rb;

    //importovani scriptu s kamerou
    CameraFollow cameraScript;
    public GameObject referenceToCamera;

    //promenna pro ulozeni zda se hybe ci ne
    public float activeX;

    //rychlost pohybu objektu
    public float movespeed = 3;

    //velikost pole pro mys, po najeti do pole se bude tam pohybovat kamera
    private int touchField = 10;

    //Maximalni okraje hry
    public GameObject gameBorderL;
    public GameObject gameBorderR;

	//promenne pro border kamery
    public GameObject borderL;
    public GameObject borderR;

    public GameObject sun;
    public GameObject widthFromSun;

    private void Awake()
    {
        GameObject giveCamera = GameObject.FindWithTag("MainCamera");
        cameraScript = giveCamera.GetComponent<CameraFollow>();
    }
    // Start is called before the first frame update
    void Start()
	{
        ResBorderSize(cameraScript.cam.orthographicSize, cameraScript.cam.pixelHeight);
    }

	// Update is called once per frame
	void Update()
	{
        if (!GameScript.isGameOver)
        {
            //pohyb pomoci myse
            bool isMouse = (UnityConfiguration.cameraMoveType.HasFlag(MoveType.Mouse));
            bool isKeyboard = (UnityConfiguration.cameraMoveType.HasFlag(MoveType.Keyboard));
            if (
                Input.mousePosition.x < Screen.width / touchField &&
                Input.mousePosition.y < (Screen.height * 3) / 4 &&
                Input.mousePosition.y > Screen.height/ touchField &&
                isMouse)
                //pokud je mys left
            {
                activeX = -1;
            }
            else if (Input.mousePosition.x > Screen.width - Screen.width / touchField &&
                Input.mousePosition.y < (Screen.height * 3) / 4 &&
                Input.mousePosition.y > Screen.height / touchField &&
                isMouse)
                //pokud je mys right
            {
                activeX = 1;
            }
            else if (isKeyboard)
            {
                //pohyb pomoci klavesnice (definice pohybu(ten pohyb je urcen pro A, D a taky pro levou sipku a pravou sipku))
                activeX = Input.GetAxisRaw("Horizontal");
            }
            else
            {
                activeX = 0;
            }
        }
        else
        {
            activeX = 0;
        }
        //pohyb kamery
        rb.velocity = new Vector2((movespeed * activeX), rb.velocity.y);

        //nastaveni pozice pro slunce
        //sun.transform.position = new Vector3(widthFromSun.transform.position.x + transform.position.x, sun.transform.position.y, sun.transform.position.z);
    }
    private void ResBorderSize(float ortho, float pixelH)
    {
        float halfUserScreen = ((Camera.main.pixelWidth / 2) * ortho * 2) / pixelH;

        borderL.transform.position = new Vector2(gameBorderL.transform.position.x + halfUserScreen, borderL.transform.position.y);
        borderR.transform.position = new Vector2(gameBorderR.transform.position.x - halfUserScreen, borderR.transform.position.y);
        transform.position = new Vector2(borderL.transform.position.x + borderL.transform.localScale.x, transform.position.y);
    }
}