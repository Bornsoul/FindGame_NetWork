﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAInit_Scene : MonoBehaviour
{
    
    // Use this for initialization
    void Start()
    {
        Screen.SetResolution(1280, 720, false);

        Application.LoadLevel("Login");
    }

    // Update is called once per frame
    void Update()
    {

    }
    
}
