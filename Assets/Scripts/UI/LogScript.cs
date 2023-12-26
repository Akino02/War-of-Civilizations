using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LogScript : MonoBehaviour
{
    public GameObject canvas;
    public Text monolog;
    private string[] texts = { "Welcome", "You don't have enough money", "You have a full queue" };

    // Start is called before the first frame update
    void Start()
    {
        //monolog.text = texts[0];

        /*static Text CreateText(Transform parent)                  //vytvorit text, ktery se presune do canvasu a do neho se vypise "texts[0]"
        {
            var go = new GameObject();
            go.transform.parent = parent;
            var text = go.AddComponent<Text>();
            return text;
        }
        var mytext = CreateText(canvas.transform);
        mytext.text = texts[0];*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
