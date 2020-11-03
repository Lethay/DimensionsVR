using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class Subtitler : MonoBehaviour
{
    public TextAsset[] SubtitleFiles;     //RawText files containing the transcript of each audio file
    public List<Subtitle[]> SubtitleList; //List of arrays of subtitles - one list for each file, each w/ unknown number of subtitles
    public GameObject SubtitlePrefab;     //The text prefab we use to render subttiles
    private Transform Canvas;             //Where we draw subtitles
    private AudioSource AudioSource;      //The object playing audio files for these subtitles
    private Vector2 InitialAnchor;        //Initial position for subtitles
    private float AnchorOffset;           //Where subtitles get pushed, in case of long sentences
    private float TransformOffset;        //Same thing but in pixel coordinates instead of fractional
    bool disabled = false;
    public bool initialised = false;
    public bool AudioControlledByTimeLine = false;

    private void Start()
    {
        Canvas = this.transform.Find("Canvas");
        if (Canvas == null)
        {
            Debug.LogWarning("Subtitler: Canvas not found.");
            disabled = true;
            return;
        }
        AudioSource = this.gameObject.GetComponent<AudioSource>();
        SubtitleList = new List<Subtitle[]>();
        InitialAnchor = new Vector2(0.5f, 0.05f);
        AnchorOffset = 0.05f;
        TransformOffset = AnchorOffset * Canvas.GetComponent<RectTransform>().rect.height;

        for (int i = 0; i < SubtitleFiles.Length; i++)
        {
            SubtitleList.Add(load(SubtitleFiles[i].text));
        }

        initialised = true;
    }
    private void Update()
    {
    }

    public int play(int clip, float ClipLength){
        if (disabled) return 1;
        if (SubtitleList == null){
            Debug.LogWarningFormat("SubtitleList is null: {0}. Subtitle files: {1}. Perhaps you tried to play a subtitle on start?", SubtitleFiles, SubtitleList);
            return 1;
        }
        if (clip < 0)
        {
            //Debug.LogWarningFormat("AudioManager: Clip number {0} is less than zero. No file will be played.", clip);
            return 1;
        }

        if (clip > SubtitleList.Count)
        {
            Debug.LogWarningFormat(
                "Subtitler: Clip number {0} is greater than the number of subtitle files available, {1}. No text will be shown.",
                clip, SubtitleList.Count
            );
            return 1;
        }


        Subtitle[] subs = SubtitleList[clip]; //File number clip, first line. The rest will be updated iteratively.
        AddToScreen(subs, ClipLength);

        return 0;
    }
    private GameObject AddToScreen(Subtitle[] subs, float ClipLength){
        GameObject SubObj = Instantiate(SubtitlePrefab, Canvas);

        //Set transform for object containing sub + background
        RectTransform RT = SubObj.transform.GetComponent<RectTransform>();
        RT.anchorMin = InitialAnchor;
        RT.anchorMax = new Vector2(InitialAnchor[0], InitialAnchor[1] + AnchorOffset);
        RT.offsetMin = new Vector2(RT.offsetMin.x, 0); //Set bottom
        RT.offsetMax = new Vector2(RT.offsetMax.x, 0); //Set top

        //Set text
        TMPro.TextMeshProUGUI TextComp = SubObj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        TextComp.text = subs[0].text;

        //Watch the subtitle to update or destroy it
        StartCoroutine(UpdateSubtitle_Keep1(subs, SubObj, Time.timeSinceLevelLoad, ClipLength));
        return SubObj;
    }

    IEnumerator UpdateSubtitle_Keep1(Subtitle[] subs, GameObject SubObj, float timeStarted, float maxLength)
    {
        float nextTime = 0;
        GameObject[] SubObjs = new GameObject[2]; //max two lines
        SubObjs[0] = SubObj;
        TMPro.TextMeshProUGUI TextComp = SubObj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        int childCount = 0;

        //Debug.LogFormat("1\n");
        for (int s = 0; s < subs.Length; s++){
            //End if audio source has stopped playing
            if (!AudioControlledByTimeLine && !AudioSource.isPlaying) break;
            //Debug.LogFormat("2+{0}\n", s);

            //Keep yielding until parent subtitle (start of sentence!) ready to be added
            nextTime = subs[s].timestamp;
            while (Time.timeSinceLevelLoad < timeStarted + nextTime) yield return 1;

            // Change subttitle
            TextComp.text = subs[s].text; // + (subs[s].HasChildren() ? "..." : "");

            //If the line is long enough that we have child lines, iterate between those.
            childCount = subs[s].children.Count;
            for (int l = 0; l < childCount; l++)
            {
                //End if audio source has stopped playing
                if (!AudioControlledByTimeLine && !AudioSource.isPlaying) break;

                //Keep yielding until we are ready to add a new subtitle
                nextTime = subs[s].children[l].timestamp;
                while (Time.timeSinceLevelLoad < timeStarted + nextTime) yield return 1;

                //Change subttitle
                TextComp.text = subs[s].children[l].text; // + (l == childCount-1 ? "..." : "");
            }
        }


        //Show last subtitle until we stop playing or we're past the length of time we were told to show it
        while ((AudioControlledByTimeLine || AudioSource.isPlaying) && Time.timeSinceLevelLoad < timeStarted + maxLength) yield return 1;
        //Debug.LogFormat("3\n");

        //Destroy the object holding the subtitle text and end
        Destroy(SubObj);
        yield return 0;
    }

    // IEnumerator UpdateSubtitle_Keep2(Subtitle sub, GameObject SubObj, float timeStarted){
    //     float nextTime = 0;
    //     GameObject[] SubObjs = new GameObject[2]; //max two lines
    //     SubObjs[0] = SubObj;
    //     int ind;
    // 
    //     for (int s = 0; s < sub.children.Count; s++) {
    //         //End if audio source has stopped playing
    //         if (!AudioSource.isPlaying) break;
    // 
    //         //Keep yielding until we are ready to add a new subtitle
    //         nextTime = sub.children[s].timestamp;
    //         while (Time.timeSinceLevelLoad < timeStarted + nextTime) { yield return 1; }
    // 
    //         //Destroy unneeded subtitles
    //         if (s >= 1)
    //         {
    //             ind = s - 1; //If on the first child, we don't destroy any. If on the second child, we want to destroy the parent, which is index 0.
    //             Destroy(SubObjs[ind]);
    //         }
    // 
    //         //Push previous subtitle upwards
    //         ind = s; //If on first child, we push the parent object. If on second child, we push index 1, and so on.
    //         Vector3 OriginalPos = SubObjs[ind].transform.localPosition;
    //         Vector3 target = SubObjs[ind].transform.localPosition;
    //         target[1] += TransformOffset;
    //         while(SubObjs[ind].transform.localPosition[1] < target[1])
    //             SubObjs[ind].transform.localPosition = Vector3.MoveTowards(SubObjs[ind].transform.localPosition, target, 20f * Time.deltaTime);
    // 
    //         //Create new subtitle
    //         AddToScreen(sub.children[s]);
    //         yield return 2;
    //     }
    // 
    //     while (AudioSource.isPlaying) yield return 3;
    // 
    //     for (int s=0; s<2; s++){
    //         if (SubObjs[s] != null) Destroy(SubObjs[s]);
    //     }
    //     
    //     yield return 0;
    // }

    private Subtitle[] load(string fileContent)
    {
        StringReader stream = new StringReader(fileContent);
        string line;
        string[] split = new string[2];
        bool inSentence = false;
        float time;
        string sentence;
        Subtitle[] subtitles = new Subtitle[1000];
        int numSubtitles = 0;

        while ((line = stream.ReadLine()) != null)
        {
            //Skip line if it's empty, but mark this as a new sentence
            if (line.Length == 0)
            {
                inSentence = false;
                continue;
            }

            //Extra both the timestamp and the text, e.g. "00	In front of you, you can see a square."
            split = line.Split('\t');
            time = float.Parse(split[0]);
            sentence = split[1];

            //Either make a new subtitle (if previous line was a blank line)
            //, or append to the children of the previous subtitle (if this is a continuation)
            if (inSentence == false) subtitles[numSubtitles++] = new Subtitle(time, sentence);
            else subtitles[numSubtitles - 1].children.Add(new Subtitle(time, sentence));
            inSentence = true;
        }

        Array.Resize(ref subtitles, numSubtitles);
        return subtitles;
    }
}
