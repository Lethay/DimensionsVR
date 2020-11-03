using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MengerPauseScript : MonoBehaviour
{
    [HideInInspector] public bool isPaused = false;
    private GameObject pauseMenuObject;
    private GameObject pauseCanvasObject;
    private PlayableDirector TimelineDirector;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuObject = this.gameObject;
        pauseCanvasObject = this.transform.Find("PauseCanvas").gameObject;
        pauseCanvasObject.SetActive(false);
        TimelineDirector = this.GetComponentInParent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void pauseScene()
    {
        isPaused = true;

        //Pause timeline
        TimelineDirector.Pause();

        //Show pause menu
        pauseCanvasObject.SetActive(true);
    }

    public void unpauseScene()
    {
        isPaused = false;

        //Pause timeline
        TimelineDirector.Play();

        //Hide pause menu
        pauseCanvasObject.SetActive(false);
    }
}
