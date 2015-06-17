﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FileOps : MonoBehaviour {
    public Text debug;

    public void SaveFile()
    {
        Debug.Log("Save");
        debug.text = "save";
    }

    public void SaveFileAs ()
    {
        Debug.Log("Save as");
        debug.text = "save as";
    }

    public void LoadFile()
    {
        Debug.Log("Load");
        debug.text = "load";
    }

    public void NewFile()
    {
        Debug.Log("New");
        debug.text = "new";
    }

    void Update ()
    {
        if (Input.GetButton("Control"))
        {
            if (Input.GetButtonUp("Save"))
            {
                if (Input.GetButton("Shift"))
                {
                    SaveFileAs();
                }
                else
                {
                    SaveFile();
                }
            }
            if (Input.GetButtonUp("Load"))
            {
                LoadFile();
            }
            if (Input.GetButtonUp("New"))
            {
                NewFile();
            }
        }
    }

}