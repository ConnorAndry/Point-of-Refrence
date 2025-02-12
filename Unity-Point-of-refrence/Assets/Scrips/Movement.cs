using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    public NavMeshAgent agent;

    public BasicEnamyController Bec;

    
    public GameObject Children;
    public GameObject Children1;

    public bool child = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Children");
        agent = GetComponent<NavMeshAgent>();
        agent.destination = Children.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, Children.transform.position) < 5)
        {
            agent.destination = Children1.transform.position;
        }

        if (Vector3.Distance(transform.position, Children1.transform.position) < 5)
        {
            agent.destination = Children.transform.position;
        }
    }

    
}


