using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FileOps : MonoBehaviour {
    public Text debug;
    public FileBrowser fileBrowser;

    public void SaveFile()
    {
        // if we already have a path, just save it without throwing a dialog.
        if (FileData.currentFile.HasValidPath())
        {
            FileData.currentFile.SaveData();
        }
        // if we DON'T have a valid path, throw a dialog to get a new one.
        else
        {
            Debug.Log("Throw SaveAs");
            fileBrowser.OpenFileBrowser(FileOperation.Save);
        }
        Debug.Log("Save");
        debug.text = "save";
    }

    public void SaveFileAs ()
    {
        fileBrowser.OpenFileBrowser(FileOperation.Save);
        Debug.Log("Save as");
        debug.text = "save as";
    }

    public void LoadFile()
    {
        fileBrowser.OpenFileBrowser(FileOperation.Load);
        Debug.Log("Load");
        debug.text = "load";
    }

    public void NewFile()
    {
        FileData.currentFile = FileData.NewFile();
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
