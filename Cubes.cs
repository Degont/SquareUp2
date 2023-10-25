using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;


public class Cubes : MonoBehaviour
{
    private System.Random random = new System.Random();
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;
    public bool connected = false;
    private int speed;
    private Vector3 direction;
    private Map map;
    // Start is called before the first frame update
    void Start()
    {
        
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        //map = transform.parent.gameObject.GetComponent<Map>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Note we only do things if its connected which means its attached to a player 
    /// </summary>
    /// <param name="collision"></param> <summary>
    /// This is the object colliding with this current cube
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D(Collision2D collision)
    {   
        // lOCAL VARIABLES
        Vector3 offset;
        
        // If the cube is connected to a player
        if(transform.parent.gameObject.tag == "Player")
        {
            
            // Colliding with self
            if(collision.transform == transform.parent || collision.transform.parent == transform.parent)
            {
                return;
            }

            else // Colliding with something else
            {
                if(collision.gameObject.tag == "Heart" || collision.gameObject.tag == "Player")
                {
                    Destroy(collision.gameObject);
                }

                if(collision.gameObject.tag == "Cube")
                {
                    Debug.Log("Land Ho");
                    if(collision.transform.parent.gameObject.tag == "Map")
                    {
                        offset = WhichSide(collision);

                        collision.transform.rotation = transform.parent.rotation;
                        // collision.transform.parent = transform;
                        // collision.transform.localPosition = offset;
                        collision.transform.parent = transform.parent;
                        collision.transform.localPosition = offset + transform.localPosition;
                        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
                        rb.bodyType = RigidbodyType2D.Kinematic;

                    }
                    else
                    {
                        Destroy(collision.gameObject);
                        Destroy(gameObject);
                    }
                }

            }
        }
        else
        {

        }
        
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        
    }
    
    void OnTriggerStay2D(Collider2D trigger)
    {
       
    }

    void OnTriggerExit2D(Collider2D trigger)
    {

    }

 Vector3 WhichSide(Collision2D cube)
    {
        string face = null;
        
        float champion = 6.9f;

        Vector3 offset;

        // dict with absolute positions of each side,
        Dictionary<string,Vector3> sides = new Dictionary<string, Vector3>() 
        {
            {"Up",transform.position + transform.up},
            {"Down",transform.position + (-1*transform.up)},
            {"Right",transform.position + transform.right},
            {"Left",transform.position + (-1*transform.right)}
        };

        foreach(KeyValuePair<string,Vector3> side in sides)
        {
            float challenger = Vector3.Distance(cube.transform.position,side.Value); // calculates the distance between 2 vectors
            
            if(champion == 6.9f)
            {
                champion = challenger;
                face = side.Key;
            }
            else
            {
                if(challenger<champion) // checks if new distance is less than previous one
                {
                    champion = challenger; // if new one is small then change holder to current one
                    face = side.Key; // update vector3 position
                }
                else if(challenger == champion) // deals with if its the same distance
                {
                    int rN = random.Next(0,2);
                    if(rN == 1)
                    {
                        face = side.Key; // 50/50 chance to flip it to the new one
                    }
                }
            }
        };

        switch(face)
        {
            case "Up":
                offset = new Vector3(0,1,0);
                return offset;
            case "Down":
                offset = new Vector3(0,-1,0);
                return offset;
            case "Left":
                offset = new Vector3(-1,0,0);
                return offset;
            case "Right":
                offset = new Vector3(1,0,0);
                return offset;
            default:
                offset = new Vector3(0,0,0);
                return offset;
        }
    }
}
