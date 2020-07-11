using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotHealth : MonoBehaviour
{
    public float hitPoints = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hitPoints <= 0)
        {
            // if health is 0 this big guy is dead 
            Destroy(gameObject);


            // minions are now friendly 
            if (gameObject.GetComponent<bigRobotController>() != null)
            {
                GameObject[] minions = gameObject.GetComponent<bigRobotController>().minions;
                for (int i = 0; i < minions.Length; i++)
                {
                    // if this minion isn't already destroyed
                    if (minions[i] != null)
                    {
                        minions[i].GetComponent<smallRobotController>().friendly = true;
                    }
                }
            }
            
            
        }
    }

    
}
