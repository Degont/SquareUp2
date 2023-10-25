using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Map : MonoBehaviour
{
    // Class to essentially hold global variables and things like positions, characters, cubes, states, settings
    private System.Random random = new System.Random();

    // PreFabs
    public GameObject portalPrefab;
    public GameObject circlePrefab;
    public GameObject ammoPrefab;

    // Map Stuff
    public List<Vector3> bounds = new List<Vector3>();

    // Player Stuff
    public List<GameObject> players = new List<GameObject>();
    public List<GameObject> hearts = new List<GameObject>();
    public int movementFlag = 1;
    private TextMeshPro textMesh;

    // Spawner Stuff
    public int spawnCount = 5;
    public int cubeCount = 0;
    public int cubeLimit = 10;

    // Ammo Stuff
    public int ammoCount = 0;
    public int maxAmmo = 15;

    // Block Stuff
    public float blockSpeed = 2.5f;

    // Events
    void Start()
    {
        // Map Parameters
        createBounds();

        // Spawning Portals
        for (int i = 0; i != spawnCount; i++)
        {
            spawnPortals();
        }

    }
    void Update()
    {
        // Refreshing Players
        players.Clear();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player);
        }

        // Refreshing Hearts
        hearts.Clear();
        foreach (GameObject heart in GameObject.FindGameObjectsWithTag("Heart"))
        {
            hearts.Add(heart);
        }

        // Cleaning Map
        cleanList();

        // Events 
        if(transform.childCount > 0)
        {
            // 
            createAmmo();

            // Cubes
            cubeCount = 0;
            foreach (Transform child in transform)
            {
                if (child.gameObject.tag == "Cube")
                {
                    cubeCount++;
                    CubetoPlayer(child.gameObject);
                }
            }
                
        }
    }
    void OnTriggerExit2D(Collider2D trigger)
    {
        // Removing Bullets that fly astray
        if(trigger.gameObject.tag == "Bullet")
        {
            Destroy(trigger.gameObject);
        }
    }
    void OnTriggerStay2D(Collider2D trigger)
    {
        
    }


    // Functions
    Vector3 RandVect()
    {
        // create random vector using the scale as bounds
        int x = random.Next((int)transform.localScale[0]);
        int y = random.Next((int)transform.localScale[0]);
        return new Vector3(x-(int)transform.localScale[0]/2,y-(int)transform.localScale[0]/2,0);
    }

    /// <summary>
    /// Removes dead players and out of bounds items like cubes and portals
    /// </summary>
    void cleanList()
    {
        // Removing dead players from list
        foreach (GameObject player in players)
        {
            if (player == null)
            {
                players.Remove(player);
                Debug.Log("Removed");
            }
        }

        // Removing out of bounds cubes
        foreach (GameObject cube in GameObject.FindGameObjectsWithTag("Cube"))
        {
            if (bounds[0].x < cube.transform.position.x && cube.transform.position.x < bounds[8].x && bounds[8].y > cube.transform.position.y && cube.transform.position.y > bounds[0].y)
            {

            }
            else
            {   
                // things out of bounds
                Destroy(cube.gameObject);
            }
        }

        // Removing out of bounds portals
        foreach (GameObject portal in GameObject.FindGameObjectsWithTag("Portal"))
        {
            if (bounds[0].x < portal.transform.position.x && portal.transform.position.x < bounds[8].x && bounds[8].y > portal.transform.position.y && portal.transform.position.y > bounds[0].y)
            {

            }
            else
            {   
                // things out of bounds
                Destroy(portal.gameObject);
            }
        }
    }
    void spawnPortals()
    {   
        // Portal Spawning Parameters
        Vector3 spawnPosition = RandVect();
        Quaternion spawnRotation = transform.rotation;

        // Spawning the Portal
        GameObject newPortal = Instantiate(portalPrefab, spawnPosition, spawnRotation);

        // Modifying the portal
        newPortal.transform.parent = transform;
    }

    void createAmmo()
    {   
        ammoCount = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "Ammo")
            {
                ammoCount++;
            }
        }

        if (ammoCount < maxAmmo)
        {
            // Ammo Spawning Parameters
            Vector3 spawnPosition = RandVect();
            Quaternion spawnRotation = transform.rotation;

            // Spawning the Ammo
            GameObject newAmmo = Instantiate(ammoPrefab, spawnPosition, spawnRotation);

            // Modifying the Ammo
            newAmmo.transform.parent = transform;
        }
        
    }

    void CubetoPlayer(GameObject cube)
    {
        Vector3 direction = new Vector3(0,0,0);
        float ruler = 100f;
        // Getting the closest player
        foreach (GameObject target in hearts)
        {   
            if (ruler == 100f) // if its the first player then set it to the first player
            {
                ruler = Vector3.Distance(target.transform.position, cube.transform.position);
                direction = target.transform.position - cube.transform.position;
            }
            else
            {
                if (Vector3.Distance(target.transform.position, cube.transform.position) < ruler) // if its not the first player then check if its closer than the previous one
                {
                    ruler = Vector3.Distance(target.transform.position, cube.transform.position);
                    direction = target.transform.position - cube.transform.position;
                }
            }
        }
        direction = direction.normalized;
        cube.transform.position += direction * blockSpeed * Time.deltaTime;
    }

    void createBounds()
    {
        float boundscale = transform.localScale[0]/2 - 0.5f;
        for (int i = -1; i != 2; i++)
        {
            for (int j = -1; j != 2; j++)
            {
                Vector3 temp_pos = new Vector3(i*boundscale,j*boundscale,0);
                bounds.Add(temp_pos);
            };
        };
    }

    void createGrid()
    {
        for (int i = 0; i != transform.localScale[0]; i++)
        {
            for (int j = 0; j != transform.localScale[1]; j++)
            {
                Vector3 spawnPosition = new Vector3(j - (transform.localScale[1]/2) ,i - (transform.localScale[1]/2),0);
                // Calculate the spawn rotation based on the current rotation of the original cube
                Quaternion spawnRotation = transform.rotation;

                // Spawn the new circle
                GameObject newCircle = Instantiate(circlePrefab, spawnPosition, spawnRotation);

                // Set the new cube as a child of the original cube
                newCircle.transform.parent = transform;
            };
        }
    }
}
