using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TutorialBehaviour : MonoBehaviour
{
    void Start()
    {
        UpdateFontSize();
    }

    private void UpdateFontSize()
    {
        Text introText = this.transform.GetChild(1).GetComponent<Text>();
        Text howToPlayText = this.transform.GetChild(3).GetComponent<Text>();
        Text contolsText = this.transform.GetChild(4).GetComponent<Text>();
        List<Text> textsToFit = new List<Text>
        {
            introText,
            howToPlayText,
            contolsText
        };


        int minTextSize = 9999999;
        foreach (Text tt in textsToFit)
        {
            int textSize = getTextSize(tt);
            if (textSize < minTextSize)
            {
                minTextSize = textSize;

            }
        }
        foreach (Text tt in textsToFit)
        {
            tt.resizeTextForBestFit = false;
            tt.fontSize = minTextSize;
        }
    }


    private int getTextSize(Text tt)
    {
        tt.cachedTextGenerator.Invalidate();
        Vector2 size = (tt.transform as RectTransform).rect.size;
        TextGenerationSettings tempSettings = tt.GetGenerationSettings(size);
        tempSettings.scaleFactor = 1;//dont know why but if I dont set it to 1 it returns a font that is to small.
        if (!tt.cachedTextGenerator.Populate(tt.text, tempSettings))
            Debug.LogError("Failed to generate fit size");
        return tt.cachedTextGenerator.fontSizeUsedForBestFit;
    }



    private void Update()
    {
        //UpdateFontSize();

    }
}
