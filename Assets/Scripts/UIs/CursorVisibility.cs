using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorVisibility : MonoBehaviour
{
    public bool visibleOnStart = true;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = visibleOnStart;
        Cursor.lockState = visibleOnStart ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
