using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnamyController : MonoBehaviour
{
    public Transform Enamy;

    public ScripPlayerController player;
    public NavMeshAgent agent;

    public GameObject healthPickup;

    public GameObject AmmoPickup;
   
    public int maxHealth = 5;
    public int health = 3;
    public int damageGivin = 1;
    public int damageRecieved = 1;
    public float pushBackForce = 10000;
    public int playerDistance = 30;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<ScripPlayerController>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        if (health <= 2)
            agent.destination = player.transform.position;


        if (health <= 0)
        {
            Destroy(gameObject);
            GameObject p = Instantiate(healthPickup, Enamy.position, Enamy.rotation);
            GameObject a = Instantiate(AmmoPickup, Enamy.position, Enamy.rotation);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Shot")
        {
            health -= damageRecieved;
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Health Pickup")
        {
            collision.gameObject.transform.position = Enamy.position;

            collision.gameObject.transform.SetParent(Enamy);
        }

        if (collision.gameObject.tag == "Ammo Pickup")
        {
            collision.gameObject.transform.position = Enamy.position;

            collision.gameObject.transform.SetParent(Enamy);
        }
    }

   
}
