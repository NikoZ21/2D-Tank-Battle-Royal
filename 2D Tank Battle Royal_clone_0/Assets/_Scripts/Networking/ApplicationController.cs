using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ApplicationController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        LauncherInMode(SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null);
    }

    private void LauncherInMode(bool isDedicated)
    {
        if (isDedicated) return;

        //create host 
        //create client
        //go to mainMenu
    }
}
