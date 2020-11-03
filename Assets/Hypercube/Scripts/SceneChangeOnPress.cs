using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeOnPress : MonoBehaviour
{
    public Animator fadeToBlackAnimator;
    // public int targetScene = 0;

    private void _tryFindAnimator()
    {
        if (fadeToBlackAnimator != null) return;
        fadeToBlackAnimator = GameObject.Find("FadeToBlack").GetComponent<Animator>();
    }
    public void sceneChange(int scene)
    {
        if ((SceneManager.GetActiveScene().buildIndex != scene) && (0 <= scene) && (scene <= SceneManager.sceneCountInBuildSettings))
        {
            if (fadeToBlackAnimator == null) _tryFindAnimator();

            if (fadeToBlackAnimator != null)
            {
                fadeToBlackAnimator.SetInteger("TargetScene", scene);
                fadeToBlackAnimator.Play("FadeToScene"); //Could wait for fadeToBlackAnimator to finish playing and use SceneManager.LoadScene-- not sure which is cleaner
            }

            else
            {
                SceneManager.LoadScene(scene);
            }
        }
    }
}