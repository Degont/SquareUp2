using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D trigger)
    {
        if(trigger.gameObject.tag == "Cube")
        {
            Destroy(trigger.gameObject);
            Destroy(gameObject);
        }
        if(trigger.gameObject.tag == "Heart")
        {
            Destroy(trigger.gameObject);      
            Destroy(gameObject);
        }
    }
    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if(collision.gameObject.tag == "Cube")
    //     {
    //         Destroy(collision.gameObject);
    //         Destroy(gameObject);
    //     }
    // }

}
