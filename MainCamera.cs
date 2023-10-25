using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public bool state = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (state)
        {
            FollowPlayer();
        }
        else
        {
            transform.position = new Vector3(0, 0, -10);
        }
        
    }

    void FollowPlayer()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            Player playerScript = player.GetComponent<Player>();
            if(playerScript.active)
            {
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
            }
        }
    }
}
