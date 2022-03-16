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

    [Header("Waypoints & Enemies")]
    public GameObject enemyPrefab;
    public int maxEnemies;
    public Transform[] waypoints;
    private int remainingenemies;

    [Header("TIMER")]
    public float finishMissionTimer = 4; 
    private bool timerIsRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        pistolaEscondida[Random.Range(0, pistolaEscondida.Length)].SetActive(true);
        StartCoroutine(TypeEffect(missions[currentMission], true));
        finishMissionTimer = finishMissionTimer * 60;
        EventManagerLvl1.current.pickedUpGun += StopTimer;
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

    IEnumerator TypeEffect(string text, bool runTime)
    {
        missionIndicatorText.text = "";
        missionIndicatorAnimator.SetTrigger("Sleep");

        foreach (char chacarcter in text.ToCharArray())
        {
            missionIndicatorText.text += chacarcter;
            yield return new WaitForSecondsRealtime(0.08f);
        }

        missionIndicatorAnimator.SetTrigger("Awake");
        if (runTime)
        {
            timerIsRunning = runTime;
        }

        else
        {
            RemainingEnemiesText();
        }
    }

    void StopTimer()
    {
        timerIsRunning = false;
        currentMission++;
        remainingenemies = maxEnemies;

        for(int i = 0; i < maxEnemies; i++)
        {
            Instantiate(enemyPrefab, waypoints[Random.Range(0, waypoints.Length)].gameObject.transform.position, Quaternion.identity);
        }

        StartCoroutine(TypeEffect(missions[currentMission], false));
    }

    void RemainingEnemiesText()
    {
        missionIndicatorText.text = missions[currentMission] + " " + remainingenemies.ToString() + "/" + maxEnemies.ToString();
    }

    public void KilledEnemy()
    {
        remainingenemies--;
        RemainingEnemiesText();
    }
}
