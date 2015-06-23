using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CustomFieldEntry : MonoBehaviour {
    public Text key;
    public InputField value;
    public CustomFieldList cfl;

    public void Entered()
    {
        cfl.Entered(key.text, value.text);
    }
}
