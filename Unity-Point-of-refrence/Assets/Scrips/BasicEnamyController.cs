using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnamyController : MonoBehaviour
{
    public Transform Enamy;

    public GameObject healthPickup;
   
    public int health = 3;
    public int maxHealth = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            GameObject p = Instantiate(healthPickup, Enamy.position, Enamy.rotation);
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
        if (collision.gameObject.tag == "Health Pickup")
        {
            collision.gameObject.transform.position = Enamy.position;

            collision.gameObject.transform.SetParent(Enamy);
        }
    }
}