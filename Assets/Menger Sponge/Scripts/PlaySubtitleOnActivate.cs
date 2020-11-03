using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySubtitleOnActivate : MonoBehaviour
{
    private Subtitler subtitler;
    private bool initialised = false;
    private bool playing = false;
    public int index=-1;
    public AudioManager am;

    // Start is called before the first frame update
    void Start()
    {
    }


    private void OnEnable()
    {
        playing = false;
        initialised = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (subtitler == null)
        {
            subtitler = this.transform.parent.GetComponentInParent<Subtitler>();
        }
        if (!initialised && subtitler.initialised)
        {
            initialised = true;
            return;
        }

        if (!playing && index!=-1 && am!=null)
        {
            subtitler.play(index, am.DialogueFiles[index].length);
            playing = true;
        }    
        return;
    }
}
