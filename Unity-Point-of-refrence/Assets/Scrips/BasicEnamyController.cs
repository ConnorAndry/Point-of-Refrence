using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnamyController : MonoBehaviour
{
    public int health = 30;
    public int maxHealth = 30;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            Destroy(gameObject);
            GameObject h = Instantiate


    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Shot")
        {
            Destroy(collision.gameObject);
            health -= 5;
        }
    }

   
}
