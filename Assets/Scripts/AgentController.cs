using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[System.Serializable]
public class AgentData
{
    public string state;
    public int r0 = 3;
    public float time = 120f;
    public int age = 20;
    public bool isDead = false;
}

public class AgentController : MonoBehaviour
{
    public AgentData data;
    NavMeshAgent myNavMeshAgent;
    int value = 0;
    private Image circle;

    void Start()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        value = Random.Range(0, SimulationController.points.Length);

        foreach(Transform child in this.gameObject.transform){
            if(child.name == "Canvas"){
                circle = child.GetChild(0).GetComponent<Image>();
            }
        }
    }

    void Update()
    {
        if(data.state == "Sick"){
            data.time -= Time.deltaTime;
            if(data.time <= 0.0f){
                data.state = "Recovered";
                circle.color = GetColorRGBA("#0AFF06");
                SimulationController.recovered += 1;
                SimulationController.sick -= 1;
            }
        }

        myNavMeshAgent.SetDestination(SimulationController.points[value].transform.position);
        float distanceToTarget = Vector3.Distance(transform.position, SimulationController.points[value].transform.position);
        if(distanceToTarget <= 0.6f){
            value = Random.Range(0, SimulationController.points.Length);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Agent"){
            var agent = other.GetComponent<AgentController>().data;
            var state = agent.state;
            if((state == "Sick") && (data.state == "Healthy") && (agent.r0 > 0)){
                data.state = "Sick";
                circle.color = GetColorRGBA("#FF0000");
                agent.r0 -= 1;
                SimulationController.sick += 1;
                SimulationController.healthy -= 1;
                ProbabilityDeath();
            }
        }
    }

    Color GetColorRGBA(string hex_color)
    {
        Color new_color;
        ColorUtility.TryParseHtmlString(hex_color, out new_color);
        return new_color;
    }

    void ProbabilityDeath()
    {
        float percentage = 0.0f;
        for(int i=0; i<= 6; i++){
            if((data.age >= SimulationController.items[i]["min"]) && (data.age <= SimulationController.items[i]["max"])){
                percentage = SimulationController.items[i]["percentage"];
            }
        }
        float r = Random.Range(0.0f, 100.0f);

        if(r <= percentage){
            data.isDead = true;
            data.state = "Death";
            Animator m_Animator = GetComponent<Animator>();
            m_Animator.SetBool("IsDead", true);
            myNavMeshAgent.speed = 0.0f;
            SimulationController.sick -= 1;
            SimulationController.dead += 1;
        }
    }
}
