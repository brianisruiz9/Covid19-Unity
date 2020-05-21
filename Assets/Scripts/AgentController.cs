using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    NavMeshAgent myNavMeshAgent;
    public GameObject[] points;
    int value = 0;
    void Start()
    {
        points = GameObject.FindGameObjectsWithTag("Point");
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        value = Random.Range(0, points.Length);
    }

    void Update()
    {
        myNavMeshAgent.SetDestination(points[value].transform.position);
        float distanceToTarget = Vector3.Distance(transform.position, points[value].transform.position);
        //Debug.Log(distanceToTarget);
        if(distanceToTarget <= 0.6f){
            value = Random.Range(0, points.Length);
        }
    }
}
