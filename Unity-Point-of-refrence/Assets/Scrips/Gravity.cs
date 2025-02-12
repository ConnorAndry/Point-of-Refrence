using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Gravity : MonoBehaviour
{
    public Transform Gravityfall;

    public GameObject Gravitydoor;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Gravity fall")
        {
            Destroy(Gravitydoor);
            
        }
    }

   
}
