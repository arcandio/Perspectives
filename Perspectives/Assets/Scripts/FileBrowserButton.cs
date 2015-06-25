using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FileBrowserButton : MonoBehaviour, IPointerClickHandler {
    public FileBrowser fileBrowser;
    public Text buttonText;
    public Image icon;
    public string directoryPath;
    public string filePath;
    public string fileName;
    public bool isDirectory = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        fileBrowser.ReceiveClick(this);
        if (eventData.clickCount == 2)
        {
            fileBrowser.ExecuteFileBrowser();
        }
    }
}
