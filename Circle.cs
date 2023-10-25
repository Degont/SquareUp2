using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public GameObject map;
    public GameObject circlePrefab;
    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i != map.transform.localScale[0]; i++)
        {
            for (int j = 0; j != map.transform.localScale[1]; j++)
            {
                Vector3 spawnPosition = new Vector3(j - (map.transform.localScale[1]/2) ,i - (map.transform.localScale[1]/2),0);
            

                // Calculate the spawn rotation based on the current rotation of the original cube
                Quaternion spawnRotation = transform.rotation;

                // Spawn the new circle
                GameObject newCircle = Instantiate(circlePrefab, spawnPosition, spawnRotation);

                // Set the new cube as a child of the original cube
                newCircle.transform.parent = transform;
            };
        };


            

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
