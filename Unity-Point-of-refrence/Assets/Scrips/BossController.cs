using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform Boss;

    public GameObject healthBuff;


    public int health = 10;
    public int maxHealth = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            Destroy(gameObject);
            GameObject p = Instantiate(healthBuff, Boss.position, Boss.rotation);
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
