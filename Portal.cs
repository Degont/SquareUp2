
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    System.Random random = new System.Random();
    private GameObject cubePrefab;
    private List<Vector3> grid = new List<Vector3>();
    private List<Vector3> bounds = new List<Vector3>();
    private float scale = 0.05f;
    private int scaleBounds = 1;
    private Vector3 target;

    
    void Start()
    {

        // Cube Soawn stuff
        cubePrefab = Resources.Load("Cube") as GameObject;
        StartCoroutine(WaitAndCreateCube());
        
        // Animation stuff
        target = transform.position;
        StartCoroutine(WaitAndDoSomething());

        for (int i = -1; i != 2; i++)
        {
            for (int j = -1; j != 2; j++)
            {
                Vector3 temp_pos = new Vector3(i*scale,j*scale,0);
                Vector3 temp_bounds = transform.position + new Vector3(i*scale*scaleBounds,j*scale*scaleBounds,0);

                grid.Add(temp_pos);
                bounds.Add(temp_bounds);
            };
        };

    }

    // Update is called once per frame
    void Update()
    {  
        // This function is weird becuase when i wasnt cleaning this it was getting cubes attached to it at the critical location
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "Cube")
            {
                Destroy(child.gameObject);
            }
        }

        // Bounds for animation stuff
        if (target.normalized.x * scale * Time.deltaTime + transform.position.x > bounds[0].x && target.normalized.x * scale * Time.deltaTime + transform.position.x < bounds[8].x && target.normalized.y * scale * Time.deltaTime + transform.position.y > bounds[0].y && target.normalized.y * scale * Time.deltaTime + transform.position.y < bounds[8].y)
        {
            transform.position += target.normalized * scale * Time.deltaTime;
        }
    }   

    /// <summary>
    /// This fxn is for the jiggling motion of the portal
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitAndDoSomething()
    {   
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            target = grid[random.Next(0,grid.Count)];
        }
    }

    IEnumerator WaitAndCreateCube()
    {   
        yield return new WaitForSeconds(random.Next(0,5));
        while (true)
        {   
            yield return new WaitForSeconds(5.0f);
            CreateCube();
            
        }
    }

    void CreateCube()
    {
        // Calculate the spawn position based on the current rotation and position of the original cube
            Vector3 spawnPosition = transform.position;

            // Calculate the spawn rotation based on the current rotation of the original cube
            Quaternion spawnRotation = transform.parent.rotation;

            // Spawn the new cube
            GameObject newCube = Instantiate(cubePrefab, spawnPosition, spawnRotation);
            newCube.transform.parent = transform.parent;
    }

    Vector3 RandVect()
    {
        int x = random.Next((int)transform.parent.localScale[0]);
        int y = random.Next((int)transform.parent.localScale[0]);
        return new Vector3(x-(int)transform.parent.localScale[0]/2,y-(int)transform.parent.localScale[0]/2,0);
    }
}
