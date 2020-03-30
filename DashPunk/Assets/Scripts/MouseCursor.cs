using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    private Vector2 cursorPos;
    void Start()
    {
        Cursor.visible = false;
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void LateUpdate()
    {        
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;   
    }    
}
