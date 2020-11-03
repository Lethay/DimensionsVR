using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TextPopulator : MonoBehaviour
{
    TMPro.TextMeshProUGUI TMProObject;
    private Subtitler subtitler;
    bool initialised = false;

    // Start is called before the first frame update
    void Start()
    {
        subtitler = GetComponentInParent<Canvas>().GetComponentInParent<Subtitler>();
        return;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialised && subtitler.initialised){
            Initialise();
            initialised = true;
        }
        return;
    }

    int Initialise()
    {
        if (subtitler == null)
        {
            Debug.LogWarning("Subtitle canvas: subtitle script not found. It should be the parent of the canvas that this object is attached to.");
            return 1;
        }

        //Iterate over all children and set the text of the TMProGui elements to text we get from the subtitler script.
        int i = 0;
        string s = "\0";
        foreach (TMPro.TextMeshProUGUI tmpgui in this.transform.parent.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
        {
            //Get the text out of the Subtitler script
            s = subtitler.SubtitleList[i][0].text;
            tmpgui.text = s;
            tmpgui.transform.gameObject.SetActive(false);
            i++;
        }

        return 0;
    }
}
