using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level1Manager : MonoBehaviour
{
    [Header("Mission")]
    public GameObject[] pistolaEscondida;
    public Animator missionIndicatorAnimator;
    public TMP_Text missionIndicatorText;
    [SerializeField] string[] missions;
    private int currentMission = 0;

    [Header("TIMER")]
    public float finishMissionTimer = 4; 
    private bool timerIsRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        pistolaEscondida[Random.Range(0, pistolaEscondida.Length)].SetActive(true);
        StartCoroutine(TypeEffect(missions[currentMission]));
        finishMissionTimer = finishMissionTimer * 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (finishMissionTimer > 0)
            {
                finishMissionTimer -= Time.deltaTime;
                DisplayTime(finishMissionTimer);
            }
            else
            {
                Debug.Log("Time has run out!");
                finishMissionTimer = 0;
                timerIsRunning = false;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        missionIndicatorText.text = missions[currentMission] + ": " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator TypeEffect(string text)
    {
        missionIndicatorAnimator.SetTrigger("Sleep");
        missionIndicatorText.text = "";

        foreach (char chacarcter in text.ToCharArray())
        {
            missionIndicatorText.text += chacarcter;
            yield return new WaitForSecondsRealtime(0.08f);
        }

        missionIndicatorAnimator.SetTrigger("Awake");
        timerIsRunning = true;
    }
}
