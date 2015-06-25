using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class FileData : MonoBehaviour {
    static public List<FileData> files = new List<FileData>();
    static public FileData currentFile;

    public string path;
    public string fileDirectory;
    public string fileName;
    public string fileExtension;
    public bool isDirty;
    public string rawContents;
    public JSONObject jsonContents;
    public List<Element> allElements;
    public List<Element> selectedElements;
    public int index;

    static public List<string> perspectivesDefault = new List<string>()
    {
        "Default",
        "Places",
        "People",
        "Events"
    };
    public List<string> perspectivesCustom;

    static public List<string> nodeTypesDefault = new List<string>()
    {
        "No Type",
        "Person",
        "Group",
        "Place",
        "Thing",
        "Event",
        "Note"
    };
    public List<string> nodeTypesCustom;

    static public List<string> edgeTypesDefault = new List<string>()
    {
        "No Type",
        "Action",
        "Membership",
        "Relationship",
        "Path",
        "Dependency"
    };
    public List<string> edgeTypesCustom;

    public List<string> customFields;

    static public FileData GetFile(string path)
    {
        int index = FindInList(path);
        if (index == -1)
        {
            // new script
            FileData fileData = NewFile();
            // apply vars
            fileData.InitializeAtPath(path);
            // Load the data
            fileData.LoadData();
            // emit the new FileData
            return fileData;
        }
        else if (index > -1 && index < files.Count)
        {
            //files[index].LoadData();
            return files[index];
        }
        else
        {
            Debug.LogError("Error initializing a FileData object at path: " + path);
            return null;
        }
    }
    static public FileData NewFile()
    {
        // Create new object
        GameObject obj = new GameObject();
        // new script
        FileData fileData = obj.AddComponent<FileData>();
        fileData.index = files.Count;
        files.Add(fileData);
        return fileData;
    }
    public void InitializeAtPath(string path)
    {
        this.path = path;
        fileDirectory = Path.GetDirectoryName(path);
        fileName = Path.GetFileNameWithoutExtension(path);
        fileExtension = Path.GetExtension(path);
    }
    static int FindInList(string path)
    {
        int index = -1;
        if (!string.IsNullOrEmpty(path))
        {
            for (int i = 0; i < files.Count; i++)
            {
                FileData f = files[i];
                if (f.path == path)
                {
                    index = i;
                }
            }
        }
        return index;
    }

    void LoadData()
    {
        // raw first
        rawContents = File.ReadAllText(path);
        // json
        jsonContents = new JSONObject(rawContents);
        // populate
        
    }

    public void SaveData()
    {
        // collect all data
        jsonContents = new JSONObject();

        // json object to string
        rawContents = jsonContents.ToString(true);
        Debug.Log(jsonContents.ToString(true));
        // write string to file
        File.WriteAllText(path, rawContents);
    }

    public bool HasValidPath()
    {
        // Did not have a path at all, probably a New file
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }
        // The directory of the file exists, so we can save there, regardless of whether the file actuall exists
        else if (Directory.Exists(Path.GetDirectoryName(path)))
        {
            return true;
        }
        // The directory doesn't exist, so we need a new path.
        else
        {
            return false;
        }
    }

}
