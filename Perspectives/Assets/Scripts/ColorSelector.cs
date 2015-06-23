using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ColorSelector : MonoBehaviour {
    public ElementFieldEditor efe;
    public ElementPaneUI elementUi;
    public List<Button> colorButtons;
    public int hues;
    public int tones;
    public string current= "";
    public Sprite currentSprite;
    public Sprite notCurrentSprite;

    public void SetupColorButtons()
    {
        CheckMinimumButtons();
        for (int i = 0; i < colorButtons.Count; i++ )
        {
            Button b = colorButtons[i];
            int hue = Mathf.FloorToInt((float)i / (float)tones);
            int tone = i % tones;
            Color c = ColorFromHueAndTone(hue, tone);
            b.GetComponent<Image>().color = c;
            b.name = c.ToString();
        }
    }

    public void Clicked(Button b)
    {
        Color c = b.image.color;
        efe.RecieveColor(c);
    }

    public void HighlightCurrentColor(Color c)
    {
        current = c.ToString();
        foreach (Button b in colorButtons)
        {
            if (current == b.name)
            {
                b.image.sprite = currentSprite;
            }
            else if (b.image.sprite == currentSprite)
            {
                b.image.sprite = notCurrentSprite;
            }
        }
    }

    void CheckMinimumButtons()
    {
        if (colorButtons.Count < (hues * tones))
        {
            GameObject clone = Instantiate(colorButtons[0].gameObject);
            clone.transform.SetParent(colorButtons[0].transform.parent, false);
            colorButtons.Add(clone.GetComponent<Button>());
            CheckMinimumButtons();
        }
    }

    Color ColorFromHueAndTone (int hue, int tone)
    {
        float h = (float)hue / (float)(hues - 1) * 360;
        float l = (float)tone / (float)(tones - 1);
        float s = 1;
        Color newColor = Utilities.ColorFromHSL(h, s, l);
        return newColor;
    }
}
