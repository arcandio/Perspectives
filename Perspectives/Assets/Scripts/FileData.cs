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
            fileData.name = path;
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
        if (jsonContents != null)
        {
            // populate
            UnpackGlobalData(jsonContents);
            UnpackAllElements(jsonContents);
            // Update UI
            Activate();
        }
        else
        {
            Debug.LogError("Invalid json file contents in " + path);
        }
    }
    void UnpackGlobalData(JSONObject j)
    {
        perspectivesCustom = new List<string>();
        JSONObject jcp = j.GetField("custom perspectives");
        for (int i = 0; i < jcp.Count; i++ )
        {
            perspectivesCustom.Add(jcp[i].str);
        }

        nodeTypesCustom = new List<string>();
        JSONObject jcnt = j.GetField("custom node types");
        for (int i = 0; i < jcnt.Count; i++)
        {
            nodeTypesCustom.Add(jcnt[i].str);
        }

        edgeTypesCustom = new List<string>();
        JSONObject jcet = j.GetField("custom edge types");
        for (int i = 0; i < jcet.Count; i++)
        {
            edgeTypesCustom.Add(jcet[i].str);
        }

        customFields = new List<string>();
        JSONObject jcf = j.GetField("custom fields");
        for (int i = 0; i < jcf.Count; i++)
        {
            customFields.Add(jcf[i].str);
        }
    }
    void UnpackAllElements(JSONObject j)
    {
        JSONObject je = j.GetField("elements");
        Transform parent = ElementPaneUI.elementUI.bgi.layout;
        allElements = new List<Element>();
        selectedElements = new List<Element>();
        // create instances
        for (int i = 0; i < je.Count; i++)
        {
            allElements.Add(UnpackElement(je[i]));
        }
        // now apply edge linking

    }
    Element UnpackElement(JSONObject j)
    {
        string type = j.GetField("element type").str;
        GameObject clone;
        Element e = null;
        if (type.ToLower() == "node")
        {
            clone = Instantiate(ElementPaneUI.elementUI.bgi.nodePrototype);
            Node node = clone.GetComponent<Node>();
            e = node;
        }
        else if (type.ToLower() == "edge")
        {
            clone = Instantiate(ElementPaneUI.elementUI.bgi.edgePrototype);
            Edge edge = clone.GetComponent<Edge>();
            edge.width = j.GetField("width").f;
            edge.headGuid = j.GetField("head").str;
            edge.tailGuid = j.GetField("tail").str;
            edge.directionality = (Directionality)System.Enum.Parse(typeof(Directionality), j.GetField("directionality").str);
            e = edge;
        }
        else
        {
            Debug.LogError("Did not have a correct Element Type");
        }
        if (e != null)
        {
            // hierarchy stuff
            e.transform.SetParent(ElementPaneUI.elementUI.bgi.layout, false);
            e.interaction.Setup();

            // base element values
            e.Guid = j.GetField("guid").str;
            e.Content = j.GetField("content").str;
            e.description = j.GetField("description").str;
            e.elementSubType = j.GetField("element subtype").str;
            e.startDate = new TimelineDate(j.GetField("start date").str);
            e.endDate = new TimelineDate(j.GetField("end date").str);
            e.Color = ParseColorFromString(j.GetField("color").str);
            e.transform.position = ParsePositionFromString(j.GetField("position").str);
            
            // perspectives
            JSONObject jp = j.GetField("perspectives");
            e.perspectives = new List<Perspective>();
            if (jp != null)
            {
                for (int i = 0; i < jp.Count; i++)
                {
                    Perspective p = new Perspective();
                    p.perspective = jp[i].GetField("perspective").str;
                    p.position = ParsePositionFromString(jp[i].GetField("position").str);
                    p.isDisplayed = jp[i].GetField("is displayed").b;
                    e.perspectives.Add(p);
                }
            }

            // custom fields
            Dictionary<string, string> cf = new Dictionary<string, string>();
            JSONObject jcf = j.GetField("custom fields");
            if (jcf != null)
            {
                for (int i = 0; i < jcf.Count; i++)
                {
                    cf.Add(jcf[0].str, jcf[0].str);
                }
            }
        }
        return e;
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
        j.AddField("custom perspectives", pc);

        JSONObject ntc = new JSONObject(JSONObject.Type.ARRAY);
        foreach (string s in nodeTypesCustom)
        {
            ntc.Add(s);
        }
        j.AddField("custom node types", ntc);

        JSONObject etc = new JSONObject(JSONObject.Type.ARRAY);
        foreach (string s in edgeTypesCustom)
        {
            etc.Add(s);
        }
        j.AddField("custom edge types", etc);

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
        j.AddField("guid", e.Guid);
        j.AddField("start date", e.startDate.ToString());
        j.AddField("end date", e.endDate.ToString());
        j.AddField("description", e.description);
        j.AddField("color", e.Color.ToString());
        j.AddField("position", e.transform.position.ToString());

        if (e.edge != null)
        {
            j.AddField("head", e.edge.head.Guid);
            j.AddField("tail", e.edge.tail.Guid);
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

        JSONObject c = new JSONObject(JSONObject.Type.ARRAY);
        foreach (KeyValuePair<string, string> kv in e.customFields)
        {
            JSONObject jkv = new JSONObject();
            jkv.AddField("key", kv.Value);
            jkv.AddField("value", kv.Value);
            c.Add(jkv);
        }
        j.AddField("custom fields", c);

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

    float[] ParseNumbersFromString(string s)
    {
        List<float> numbers = new List<float>();

        string[] sep = new string[] { ",", "(", ")", " ", "RGBA", "RGB" };
        string[] parts = s.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string part in parts)
        {
            numbers.Add(float.Parse(part));
        }
        return numbers.ToArray();
    }

    Color ParseColorFromString(string s)
    {
        float[] numbers = ParseNumbersFromString(s);
        if (numbers.Length < 3)
        {
            System.Array.Resize(ref numbers, 3);
        }
        Color c = new Color(numbers[0], numbers [1], numbers[2]);
        return c;
    }
    Vector3 ParsePositionFromString(string s)
    {
        float[] numbers = ParseNumbersFromString(s);
        if (numbers.Length < 3)
        {
            System.Array.Resize(ref numbers, 3);
        }
        Vector3 v = new Vector3(numbers[0], numbers[1], numbers[2]);
        return v;
    }
    public Element GetElementByName(string s)
    {
        Element e = null;
        foreach (Element a in allElements)
        {
            if (a.Guid == s)
            {
                e = a;
            }
        }
        return e;
    }
    public void Activate()
    {
        DeactivateAll();
        foreach (Element e in allElements)
        {
            e.gameObject.SetActive(true);
        }
        currentFile = this;
        ElementPaneUI.elementUI.GenerateElementList();
        ElementPaneUI.elementUI.SetupFields(false);
    }
    static void DeactivateAll()
    {
        foreach (FileData fd in FileData.files)
        {
            foreach (Element e in fd.allElements)
            {
                e.gameObject.SetActive(false);
            }
        }
    }
}
