using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.AI;

public class SimulationController : MonoBehaviour
{
    public static int healthy, recovered, sick, dead;
    public Text labelHealthy, labelRecovered, labelSick, labelTime, labelDead;
    public InputField agentsField, speedField, timeField;
    public static GameObject[] points;
    public GameObject[] agents;
    public static JSONNode items;
    public static bool start = false;
    float totalTime = 0.0f;
    
    void Awake()
    {
        points = GameObject.FindGameObjectsWithTag("Point");
        ReadJson();
    }

    public void StartSimulation(){
        healthy = int.Parse(agentsField.text);
        sick = 1;
        dead = 0;
        recovered = 0;
        CreateAgents(healthy, "Healthy");
        start = true;
        //camera.enabled = true;
       // time = float.Parse(timeField.text) * 60.0f;
    }

    void CreateAgents(int cant, string state)
    {
        for(int i=0; i<cant; i++){
            GameObject prefab = agents[Random.Range(0, agents.Length)];
            int value = Random.Range(0, points.Length);
            GameObject newAgent = Instantiate(prefab, points[value].transform.position, Quaternion.identity);
            var data = newAgent.GetComponent<AgentController>().data;
            data.state = state;
            data.age = Random.Range(0, 100);
            data.r0 = Random.Range(2, 4);
            newAgent.GetComponent<NavMeshAgent>().speed = float.Parse(speedField.text);
        }
    }

    void Update()
    {
        if(start){
            Timer();
        }
        labelHealthy.text = healthy.ToString();
        labelRecovered.text = recovered.ToString();
        labelSick.text = sick.ToString();
        labelDead.text = dead.ToString();
    }

    void Timer()
    {
        totalTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(totalTime / 60f);
        int seconds = Mathf.FloorToInt(totalTime - (minutes * 60));

        string niceTime = string.Format("{00:00}:{01:00}", minutes, seconds);
        labelTime.text = niceTime;
    }

    void ReadJson(){
        string data = Resources.Load<TextAsset>("mortality").text;
        items = JSONNode.Parse(data)["items"];
    }
}
