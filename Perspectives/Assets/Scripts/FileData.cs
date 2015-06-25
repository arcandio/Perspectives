using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class FileData : MonoBehaviour {
    static public List<FileData> files = new List<FileData>();
    static public FileData currentFile;
    static public List<string> perspectivesDefault = new List<string>()
    {
        "Default",
        "Places",
        "People",
        "Events"
    };
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
    static public List<string> edgeTypesDefault = new List<string>()
    {
        "No Type",
        "Action",
        "Membership",
        "Relationship",
        "Path",
        "Dependency"
    };

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
    public List<string> perspectivesCustom;
    public List<string> nodeTypesCustom;
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
        PackGlobalData(jsonContents);
        PackAllElementData(jsonContents);

        // json object to string
        rawContents = jsonContents.ToString(true);
        Debug.Log(jsonContents.ToString(true));
        // write string to file
        File.WriteAllText(path, rawContents);
    }

    void PackGlobalData(JSONObject j)
    {
        JSONObject pc = new JSONObject(JSONObject.Type.ARRAY);
        foreach (string s in perspectivesCustom)
        {
            pc.Add(s);
        }
        j.AddField("custom fields", pc);

        JSONObject ntc = new JSONObject(JSONObject.Type.ARRAY);
        foreach (string s in nodeTypesCustom)
        {
            ntc.Add(s);
        }
        j.AddField("custom fields", ntc);

        JSONObject etc = new JSONObject(JSONObject.Type.ARRAY);
        foreach (string s in edgeTypesCustom)
        {
            etc.Add(s);
        }
        j.AddField("custom fields", etc);

        JSONObject cf = new JSONObject(JSONObject.Type.ARRAY);
        foreach (string s in customFields)
        {
            cf.Add(s);
        }
        j.AddField("custom fields", cf);
    }

    void PackAllElementData(JSONObject j)
    {
        JSONObject a = new JSONObject(JSONObject.Type.ARRAY);
        foreach (Element e in allElements)
        {
            a.AddField(e.Content, PackElementData(e));
        }
        j.AddField("elements", a);
    }

    JSONObject PackElementData(Element e)
    {
        JSONObject j = new JSONObject();
        j.AddField("content", e.Content);
        j.AddField("element type", e.elementType.ToString());
        j.AddField("element subtype", e.elementSubType);
        j.AddField("guid", e.guid);
        j.AddField("start date", e.startDate.ToString());
        j.AddField("end date", e.endDate.ToString());
        j.AddField("description", e.description);
        j.AddField("color", e.Color.ToString());
        j.AddField("position", e.transform.position.ToString());

        if (e.edge != null)
        {
            j.AddField("head", e.edge.head.guid);
            j.AddField("tail", e.edge.tail.guid);
            j.AddField("directionality", e.edge.directionality.ToString());
            j.AddField("width", e.edge.width);
        }

        JSONObject p = new JSONObject(JSONObject.Type.ARRAY);
        foreach (Perspective persp in e.perspectives)
        {
            JSONObject jp = new JSONObject();
            jp.AddField("perspective", persp.perspective);
            jp.AddField("is displayed", persp.isDisplayed);
            jp.AddField("position", persp.position.ToString());
            p.Add(jp);
        }
        j.AddField("perspectives", p);

        return j;
    }

    public bool HasValidPath()
    {
        // Did not have a path at all, probably a New file
        if (string.IsNullOrEmpty(path))
        {
            Debug.Log("null or empty" + path);
            return false;
        }
        // The directory of the file exists, so we can save there, regardless of whether the file actuall exists
        else if (Directory.Exists(Path.GetDirectoryName(path)))
        {
            Debug.Log("path exists" + path);
            return true;
        }
        // The directory doesn't exist, so we need a new path.
        else
        {
            Debug.Log("dir missing" + path);
            return false;
        }
    }

}
