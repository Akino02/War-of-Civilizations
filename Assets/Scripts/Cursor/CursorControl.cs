using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControl : MonoBehaviour
{
    public Texture2D defaultCursor, clicableCursor;
    
    /*// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/

    public void DefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }
    public void ClicableCursor()
    {
        Cursor.SetCursor(clicableCursor, Vector2.zero, CursorMode.Auto);
    }
}
