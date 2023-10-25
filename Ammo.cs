using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
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
            if(trigger.gameObject.GetComponent<Cubes>().connected == true)
            {
                trigger.transform.parent.gameObject.GetComponent<Player>().shots += 1;  
            }
            Destroy(gameObject);
        }

        if(trigger.gameObject.tag == "Heart")
        {
            trigger.transform.parent.gameObject.GetComponent<Player>().shots += 1;  
            Destroy(gameObject);
        }

        if(trigger.gameObject.tag == "Player")
        {
            trigger.gameObject.GetComponent<Player>().shots += 1;  
            Destroy(gameObject);
        }
    }
}
