using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform M_Player;
    public Transform[] waypoints;
    public Level1Manager levelManager;
    public Slider healthbar;
    public GameObject bulletPrefab;

    bool patrolling = true;
    int m_CurrentWaypointIndex;
    float originalSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Transform startPosotion = transform;
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<Level1Manager>();
        M_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        waypoints = new Transform[2];
        waypoints[0] = levelManager.waypoints[Random.Range(0, levelManager.waypoints.Length)];
        waypoints[1] = levelManager.waypoints[Random.Range(0, levelManager.waypoints.Length)];
        navMeshAgent.SetDestination(waypoints[0].position);
        healthbar.gameObject.SetActive(false);
        originalSpeed = navMeshAgent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (patrolling)
        {
            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            } //Patrullar
        }
        else
        {
            navMeshAgent.destination = M_Player.position;
            navMeshAgent.speed += Time.deltaTime;
        } //Perseguir a jugador

    }

    public void TakeDamage(float damage)
    {
        healthbar.gameObject.SetActive(true);
        patrolling = false;

        healthbar.value -= damage;
        navMeshAgent.speed = originalSpeed;

        if (healthbar.value <= 0)
        {
            levelManager.KilledEnemy();
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        } //Se Muere :(
    }

    public void ChacePlayer()
    {
        patrolling = false;
    }
}
