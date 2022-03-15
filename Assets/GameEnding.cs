using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public GameObject player;

    public CanvasGroup exitBackgroundImageCanvasGroup;
    public CanvasGroup caughtBackgroundImageCanvasGroup;

    public AudioSource exitAudio;
    public AudioSource caughtAudio;

    public float displayImageDuration = 1f;

    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;
    bool m_HasAudioPlayed;


    float m_Timer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }

    private void Update()
    {
        if (m_IsPlayerAtExit)
        {
            EndLevel(exitBackgroundImageCanvasGroup, false, exitAudio);
        }

        else if (m_IsPlayerCaught)
        {
            EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }
    }

    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
    {
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }

        m_Timer += Time.deltaTime;

        imageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                if(SceneManager.GetActiveScene().buildIndex == 0)
                {
                    SceneManager.LoadScene(1);
                }
                else
                {
                    Application.Quit();
                }
            }
        }
    }

    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }
}
