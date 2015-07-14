using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FileOps : MonoBehaviour {
    public Text fileName;
    public FileBrowser fileBrowser;
    public PerspectiveDropdown perspectiveDropdown;
    public GameObject helpPanel;

    void Start()
    {
        // https://github.com/arcandio/Perspectives/issues/36
    }

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
    }

    public void SaveFileAs ()
    {
        fileBrowser.OpenFileBrowser(FileOperation.Save);
        Debug.Log("Save as");
    }

    public void LoadFile()
    {
        fileBrowser.OpenFileBrowser(FileOperation.Load);
        Debug.Log("Load");
    }

    public void NewFile()
    {
        FileData.currentFile = FileData.NewFile();
        Debug.Log("New");
    }

    public void OpenSite()
    {
        Application.OpenURL("https://github.com/arcandio/Perspectives");
    }

    public void OpenHelpSite()
    {
        Application.OpenURL("https://github.com/arcandio/Perspectives/wiki");
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
        fileName.text = FileData.currentFile.fileDirectory+ System.IO.Path.DirectorySeparatorChar +FileData.currentFile.fileName + FileData.currentFile.fileExtension;
        if (FileData.currentFile.isDirty)
        {
            fileName.text += "*";
        }
        
    }
}
