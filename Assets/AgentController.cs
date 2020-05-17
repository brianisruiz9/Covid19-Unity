using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    NavMeshAgent myNavMeshAgent;
    public Transform target;
    void Start()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        myNavMeshAgent.SetDestination(target.position);
    }
}
