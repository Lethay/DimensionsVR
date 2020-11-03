using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] DialogueFiles; //All MP3 (or otherwise) audio files to play
    private AudioSource AudioSource;  //The object that will actually play our files
    private Subtitler subtitler;      //Script that makes subttiles

    // Start is called before the first frame update
    void Start()
    {
        AudioSource = this.gameObject.GetComponent<AudioSource>();
        subtitler = this.GetComponent<Subtitler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int play(string filename){
        for (int i=0; i<DialogueFiles.Length; i++){
            if (filename == DialogueFiles[i].name){
                return play(i);
            }
        }

        Debug.LogWarningFormat("AudioManager: filename {0} was not found in the DialogueFiles array. No file will be played.", filename);
        return 1;
    }

    public int play(int clip){
        if (clip < 0){
            //Debug.LogWarningFormat("AudioManager: Clip number {0} is less than zero. No file will be played.", clip);
            return 1;
        }

        if (clip > DialogueFiles.Length){
            Debug.LogWarningFormat(
                "AudioManager: Clip number {0} is greater than the number of dialogue files available, {1}. No file will be played.", 
                clip, DialogueFiles.Length
            );
            return 1;
        }

        AudioSource.clip = DialogueFiles[clip];
        AudioSource.Play();

        //If we have subtitles, play that too
        if (clip < subtitler.SubtitleList.Count) {
            subtitler.play(clip, DialogueFiles[clip].length);
        }
        return 0;
    }

    public void stop()      { AudioSource.Stop();           }
    public void pause()     { AudioSource.Pause();          }
    public bool isPlaying() { return AudioSource.isPlaying; }
}
