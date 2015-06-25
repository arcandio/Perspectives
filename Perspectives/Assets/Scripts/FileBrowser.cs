using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.IO;

public class FileBrowser : MonoBehaviour {
    public FileTypeAssoc[] fileTypes;
    public FileOperation operation = FileOperation.None;
    public InputField addressBarField;
    public InputField fileNameField;
    public Text windowTitle;
    public Text acceptButtonText;
    public Image backgroundPanel;
    public Canvas popupCanvas;
    public List<FileBrowserButton> fileButtons;
    public Sprite folderIcon;
    public Sprite fileIcon;

    void Start ()
    {
        popupCanvas.gameObject.SetActive(false);
    }

    public void OpenFileBrowser(FileOperation op)
    {
        popupCanvas.gameObject.SetActive(true);
        operation = op;
        gameObject.SetActive(true);
        backgroundPanel.gameObject.SetActive(true);
        acceptButtonText.text = operation.ToString() + " File";
        windowTitle.text = acceptButtonText.text;
        addressBarField.text = GetCurrentDirectory();
        GetDirectoryContents();
    }
    public void CloseFileBrowser()
    {
        popupCanvas.gameObject.SetActive(false);
    }
    public void ExecuteFileBrowser()
    {
        string fullDir = addressBarField.text + Path.DirectorySeparatorChar + fileNameField.text;
        // if we have an existing directory, open it up
        if (Directory.Exists(fullDir))
        {
            addressBarField.text += Path.DirectorySeparatorChar + fileNameField.text;
            fileNameField.text = "";
            GetDirectoryContents();
        }

        // It wasn't a Directory, so it must be a file. Switch on the operation.
        else
        {
            string completePath = fullDir.EndsWith(".json") ? fullDir : fullDir + ".json";
            bool fileExists = File.Exists(completePath);
            switch (operation)
            {
                case FileOperation.Save:
                    FileData.currentFile.InitializeAtPath(completePath);
                    FileData.currentFile.SaveData();
                    SetCurrentDirectory(addressBarField.text);
                    CloseFileBrowser();
                    break;
                case FileOperation.Load:
                    if (fileExists)
                    {
                        FileData.currentFile = FileData.GetFile(completePath);
                        SetCurrentDirectory(addressBarField.text);
                        CloseFileBrowser();
                    }
                    else
                    {
                        Debug.LogWarning("Tried to load a file that doesn't exist: "+completePath);
                    }
                    break;
            }
        }
    }
    public void ReceiveClick(FileBrowserButton b)
    {
        fileNameField.text = b.fileName;
    }
    void GetDirectoryContents()
    {
        List<string> items = new List<string>();
        items.AddRange(Directory.GetDirectories(addressBarField.text));
        int directoryCount = items.Count;
        items.AddRange(Directory.GetFiles(addressBarField.text, "*.json"));
        Debug.Log(string.Join(" - ", items.ToArray()));
        CheckMinimumButtons(items.Count);
        for (int i = 0; i < fileButtons.Count; i++)
        {
            FileBrowserButton cfbb = fileButtons[i];
            if (i < items.Count)
            {
                cfbb.gameObject.SetActive(true);
                cfbb.fileName = Path.GetFileName(items[i]);
                cfbb.buttonText.text = cfbb.fileName;
                cfbb.filePath = items[i];
                if (i < directoryCount)
                {
                    cfbb.icon.sprite = folderIcon;
                    cfbb.isDirectory = true;
                }
                else
                {
                    cfbb.icon.sprite = fileIcon;
                    cfbb.isDirectory = false;
                }
            }
            else
            {
                cfbb.gameObject.SetActive(false);
            }
        }
    }
    string GetCurrentDirectory()
    {
        if (PlayerPrefs.HasKey("currentDirectory"))
        {
            return PlayerPrefs.GetString("currentDirectory");
        }
        else
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }
    }
    void SetCurrentDirectory(string dir)
    {
        PlayerPrefs.SetString("currentDirectory", dir);
    }
    void CheckMinimumButtons(int count)
    {
        if (fileButtons.Count < count)
        {
            FileBrowserButton clone = Instantiate(fileButtons[0]);
            clone.transform.SetParent(fileButtons[0].transform.parent,false);
            fileButtons.Add(clone);
            CheckMinimumButtons(count);
        }
    }
    public void GetParentDirectory()
    {
        DirectoryInfo newDir = Directory.GetParent(addressBarField.text);
        addressBarField.text = newDir != null ? newDir.ToString() : addressBarField.text;
        GetDirectoryContents();
    }
}
public enum FileOperation
{
    None,
    Save,
    Load
}
[System.Serializable]
public struct FileTypeAssoc {
    public string extension;
    public string fileType;
}