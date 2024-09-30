using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    public Transform Boss;

    public ScripPlayerController player;
    public NavMeshAgent agent;

    public GameObject Healthbuff;


    public int health = 50;
    public int maxHealth = 50;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<ScripPlayerController>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        if (health < maxHealth)
            agent.destination = player.transform.position;

        if (health <= 0)
        {
            Destroy(gameObject);
            GameObject h = Instantiate(Healthbuff, Boss.position, Boss.rotation);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Shot")
        {
            Destroy(collision.gameObject);
            health --;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Health buff")
        {
            collision.gameObject.transform.position = Boss.position;

            collision.gameObject.transform.SetParent(Boss);
        }
    }
}
