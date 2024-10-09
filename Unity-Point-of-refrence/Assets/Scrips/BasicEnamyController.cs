using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnamyController : MonoBehaviour
{
    public Transform Enemy1;

    public ScripPlayerController player;
    public NavMeshAgent agent;

    public GameObject Health;

    public GameObject Ammo;
   
    public int maxHealth = 15;
    public int health = 15;
    public int damageGivin = 1;
    public int damageRecieved = 1;
    public float pushBackForce = 10000;
    
    

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
            GameObject p = Instantiate(Health, Enemy1.position, Enemy1.rotation);
            GameObject a = Instantiate(Ammo, Enemy1.position, Enemy1.rotation);
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
            collision.gameObject.transform.position = Enemy1.position;

            collision.gameObject.transform.SetParent(Enemy1);
        }

        if (collision.gameObject.tag == "Ammo Pickup")
        {
            collision.gameObject.transform.position = Enemy1.position;

            collision.gameObject.transform.SetParent(Enemy1);
        }

        
    }



   
}
