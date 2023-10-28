using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{   
    // Setup Stuff
    private System.Random random = new System.Random();
    private Rigidbody2D rigidBody2D;
    private BoxCollider2D boxCollider2D;
    private Transform maptrans;
    private int maplength;
    public GameObject cubePrefab;
    public GameObject map;
    public GameObject HeartPrefab;
    public GameObject bulletPrefab;
    public GameObject ammoSymbolPrefab;
    public bool active;
    private Dictionary<Vector3,GameObject> cubes = new Dictionary<Vector3, GameObject>();
    private List<Vector3> tempcubes = new List<Vector3>();
    

    // Movement Stuff
    public int movementFlag = 0;
    private float horizontalInput;
    private float verticalInput;
    private bool clockwise = false;
    private bool counterclockwise = false;
    public float speed = 10.0f;
    public float rotation = 35.0f;
    
    // Gun Stuff
    public int shots = 5;
    public int bulletSpeed = 15;
    private List<GameObject> bullets = new List<GameObject>();
    private int ammoLvl = 1;


    // Loops
    void Start()
    {
        // Fetching all the components we need
        Map mapScript = map.GetComponent<Map>(); // Getting the script from the map
        maptrans = map.GetComponent<Transform>(); // Getting the transform from the map
        maplength = (int)maptrans.localScale[0]; // Getting the length of the map

        rigidBody2D = GetComponent<Rigidbody2D>(); // Getting the rigidbody from the player
        boxCollider2D = GetComponent<BoxCollider2D>(); // Getting the box collider from the player
        createHeart();

    }

    // Update is called once per frame
    void Update()
    {
        // Checks alive or not
        checkHearts();
        
        // State modifier for player
        if (Input.GetKeyDown(KeyCode.G))
        {
            active = !active;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) // You can change the key as needed
        {
            Gun();
        }

        // Keeping my list nice and updated
        foreach (KeyValuePair<Vector3,GameObject> cube in cubes)
        {
            if(cube.Value == null) // this means my cube was defeated and I need to remove it from the list
            {
                hangingCubes(cube.Key);
            }
        }

        cubes.Clear();
        
        foreach (Transform child in transform)
        {
            // Making sure Cubes are alligned properly
            child.localPosition = new Vector3(Mathf.Round(child.localPosition[0]),Mathf.Round(child.localPosition[1]),Mathf.Round(child.localPosition[2]));
            child.rotation = transform.rotation;

            // Recreating list of cubes
            if(child.gameObject.tag == "Cube")
            {   
                if(cubes.ContainsKey(child.localPosition))
                {
                    Destroy(child.gameObject);
                }
                cubes.Add(child.localPosition,child.gameObject);
            }
        }

        // Movement Functions
        if(active)
        {
            switch(movementFlag)
            {
                case 1:
                    Movement_Keyboardv1();
                    break;
                case 2:
                    Movement_Mouse();
                    break;
                default:
                    Movement_Keyboardv0();
                    break;

            }
        }
        
    }

    void FixedUpdate()
    {

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 offset;
        if(collision.gameObject.tag == "Cube")
        {
            if(collision.transform.parent.gameObject.tag == "Map") // Making sure the cubes parent is the map or not a player
            {
                // returns where to position the cube based on the collision
                offset = WhichSide(collision);

                collision.transform.rotation = transform.rotation;
                collision.transform.parent = transform;
                collision.transform.localPosition = offset;

                Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }
    }
    
    // FUNCTIONS
    
    // Input Management

    void Movement_Mouse()
    {
        // Inspired by snake.io movement
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical"); 

        transform.position = transform.position + new Vector3(horizontalInput*speed*Time.deltaTime,verticalInput*speed*Time.deltaTime,0);

        //Vector3 movement = new Vector3(horizontalInput,verticalInput,0);
        Vector3 TomousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position;

        TomousePosition.z = 0.0f;
        //movement.z = 0.0f;

        //rigidBody2D.velocity = movement.normalized * speed;
        transform.up = TomousePosition; 
    }
    /// <summary>
    /// Alternative Keyboard Movement system for second player
    /// </summary> 
    void Movement_Keyboardv1()
    {
        // inputs
        horizontalInput = Input.GetAxis("Horizontal_alt");
        verticalInput = Input.GetAxis("Vertical_alt"); 

        if (Input.GetKeyDown(KeyCode.A))
        {
            clockwise = true;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            clockwise = false;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            counterclockwise = true;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            counterclockwise = false;
        }

        // Movement Calculated
        transform.position += transform.up.normalized * verticalInput * speed * Time.deltaTime;

        if (clockwise)
        {
            transform.Rotate(0,0,rotation*speed*Time.deltaTime);
        }
        if (counterclockwise)
        {
            transform.Rotate(0,0,-rotation*speed*Time.deltaTime);
        }

    }
    void Movement_Keyboardv0()
    {
        // inputs
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical"); 

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            clockwise = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            clockwise = false;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            counterclockwise = true;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            counterclockwise = false;
        }

        // The actual movement happening

        //transform.position = transform.position + new Vector3(/*horizontalInput*speed*Time.deltaTime*/0,verticalInput*speed*Time.deltaTime,0);
        rigidBody2D.velocity = transform.up.normalized * verticalInput * speed;

        if (clockwise)
        {
            transform.Rotate(0,0,rotation*speed*Time.deltaTime);
        }
        if (counterclockwise)
        {
            transform.Rotate(0,0,-rotation*speed*Time.deltaTime);
        }
    }

    void Gun()
    {
        if(bullets.Count<shots)
            {
                Vector3 spawnPosition = transform.up;

                // Calculate the spawn rotation based on the current rotation of the original cube
                Quaternion spawnRotation = transform.rotation;

                // Spawn the new cube
                GameObject newBullet = Instantiate(bulletPrefab, transform.position + spawnPosition.normalized, spawnRotation);
                bullets.Add(newBullet);

                Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();
                bulletRb.velocity = spawnPosition.normalized * bulletSpeed;
            }
    }

    void createHeart()
    {
        Vector3 spawnPosition = transform.position;

        // Calculate the spawn rotation based on the current rotation of the original cube
        Quaternion spawnRotation = transform.rotation;

        // Spawn the new cube
        GameObject newHeart = Instantiate(HeartPrefab, spawnPosition, spawnRotation);
        
        // Configure the heart
        newHeart.transform.parent = transform;
        newHeart.transform.localPosition = new Vector3(0,-1,0);
        Heart heart = newHeart.GetComponent<Heart>();
        heart.connected = true;

    }

    void reflectAmmo()
    {
        // switch(ammoLvl)
        // {
        //     case 1:
                
        //         break;
        //     case 2:
            
        //         break;
        //     case 3:
            
        //         break;
        //     default:
        //         break;
        // }
        // for (int i = 0; i = ammoCount; i++)
        // {
        //     Vector3 spawnPosition = transform.position;

        //     // Calculate the spawn rotation based on the current rotation of the original cube
        //     Quaternion spawnRotation = transform.rotation;

        //     // Spawn the new cube
        //     GameObject newAmmoSymbol = Instantiate(ammoSymbolPrefab, spawnPosition, spawnRotation);
            
        //     // Configure the heart
        //     newAmmo.transform.parent = transform;
        //     newAmmo.transform.localPosition = new Vector3(0,-1,0);
        //     Ammo ammo = newAmmo.GetComponent<Ammo>();
        //     ammo.connected = true;
        // }
    }

    void checkHearts()
    {
        int hearts = 0;

        foreach (Transform child in transform)
            {
                if (child.gameObject.tag == "Heart")
                {
                    hearts++;
                }
            }
            if (hearts == 0)
            {
                Destroy(gameObject);
            }   
    }
    Vector3 WhichSide(Collision2D cube)
    {
        string face = null;
        
        float champion = 100f;

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
            if(champion == 100f)
            {
                champion = challenger;
                face = side.Key;

                Debug.Log(champion+"This here is a rodeo"+face);
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

        Debug.Log(face);
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
    // Takes in a vector of a cube, returns a list of vectors for the 4 faces of the cube
    List<Vector3> sideScan(Vector3 dyingcube)
    {
        List<Vector3> fourfaces = new List<Vector3>()
        {
            new Vector3(0,1,0) + dyingcube,
            new Vector3(0,-1,0) + dyingcube,
            new Vector3(1,0,0) + dyingcube,
            new Vector3(-1,0,0) + dyingcube
        };

        List<Vector3> sortedList = fourfaces.OrderBy(v => v.magnitude).ToList(); // sorts the list by magnitude

        return sortedList;
    }

    void hangingCubes()
    {
        
    }
}
