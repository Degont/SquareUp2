using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class Gun : MonoBehaviour
{
    public int shots = 10;
    public int bulletSpeed = 5;
    public GameObject bulletPrefab;
    private List<GameObject> bullets = new List<GameObject>();   // Start is called before    the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(bullets.Count<shots)
            {
                Vector3 spawnPosition = transform.right*-1;

                // Calculate the spawn rotation based on the current rotation of the original cube
                Quaternion spawnRotation = transform.parent.rotation;

                // Spawn the new cube
                GameObject newBullet = Instantiate(bulletPrefab, transform.position, spawnRotation);
                bullets.Add(newBullet);

                Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();
                bulletRb.velocity = spawnPosition.normalized * bulletSpeed;
            }
        }
    }

    
}
