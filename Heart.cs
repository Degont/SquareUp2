using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
  private System.Random random = new System.Random();
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;
    public bool connected = false;
    private int speed;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {   
        Vector3 offset;
        if(connected)
        {
            if(collision.gameObject.tag =="Bullet")
            {
                // Debug.Log("BumbaCLot");
                // Destroy(collision.gameObject);
                // Destroy(gameObject);
            }
            if(collision.gameObject.tag == "Cube")
            {
                if(collision.transform.parent.gameObject.tag == "Map")
                {
                    offset = WhichSide(collision);

                    collision.transform.rotation = transform.parent.rotation;
                    collision.transform.parent = transform.parent;
                    collision.transform.localPosition = offset + transform.localPosition;

                    Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
                    rb.bodyType = RigidbodyType2D.Kinematic;
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

        // Setting the offset based on the calculations
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