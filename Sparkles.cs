using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sparkles : MonoBehaviour
{   
    System.Random random = new System.Random();
    private List<Vector3> grid = new List<Vector3>();
    private float scale = 0.1f;
    private Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
        StartCoroutine(WaitAndDoSomething());
        StartCoroutine(WaitAndTwinkle());

        for (int i = -1; i != 2; i++)
        {
            for (int j = -1; j != 2; j++)
            {
                Vector3 temp_pos = transform.position + new Vector3(i*scale,j*scale,0);
                grid.Add(temp_pos);
            };
        };

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target;
    }

    IEnumerator WaitAndDoSomething()
    {   
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            target = grid[random.Next(0,grid.Count)];
        }
    }

    IEnumerator WaitAndTwinkle()
    {   
        while (true)
        {
            switch(random.Next(0,4))
            {
                case 0:
                    GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                case 1:
                    GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
                case 2:
                    GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                case 3:
                    GetComponent<SpriteRenderer>().color = Color.yellow;
                    break;
            }

            switch(random.Next(0,4))
            {
                case 1:
                    yield return new WaitForSeconds(0.1f);
                    GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
                    GetComponent<SpriteRenderer>().sortingOrder = 1;
                    break;
                case 2:
                    yield return new WaitForSeconds(0.2f);
                    GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
                    GetComponent<SpriteRenderer>().sortingOrder = 2;
                    break;
                case 3:
                    yield return new WaitForSeconds(0.3f);
                    GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
                    GetComponent<SpriteRenderer>().sortingOrder = 2;
                    break;
                default:
                    yield return new WaitForSeconds(0.1f);
                    GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
                    GetComponent<SpriteRenderer>().sortingOrder = 2;
                    break;
            }
        }
    }
}

