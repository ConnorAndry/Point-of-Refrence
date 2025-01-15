using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Transform Target;

    public GameObject Platform1;

    public int health = 1;
    public int maxHealth = 1;
    public int damageRecieved = 1;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject); GameObject p = Instantiate(Platform1, Target.position, Target.rotation);
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
        if (collision.gameObject.tag == "Platform")
        {
            collision.gameObject.transform.position = Target.position;

            collision.gameObject.transform.SetParent(Target);
        }
    }
}
