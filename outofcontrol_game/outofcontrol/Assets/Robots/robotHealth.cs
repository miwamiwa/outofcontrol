using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotHealth : MonoBehaviour
{
    public float hitPoints = 100f;
    public GameObject beaconObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hitPoints <= 0)
        {
            


            // if this is a big robot, make minions friendly and spawn beacon.
            if (gameObject.GetComponent<bigRobotController>() != null)
            {

                GameObject newBeacon =Instantiate(beaconObject, transform.position, Quaternion.identity);
                GameObject.Find("robotSpawner").GetComponent<robotSpawn>().activeSpawns--;
                GameObject[] minions = gameObject.GetComponent<bigRobotController>().minions;
                for (int i = 0; i < minions.Length; i++)
                {
                    // if this minion isn't already destroyed
                    if (minions[i] != null)
                    {
                        minions[i].GetComponent<smallRobotController>().friendly = true;
                        minions[i].GetComponent<smallRobotController>().parentRobot = newBeacon;
                        minions[i].GetComponent<smallRobotController>().maxStrayDistance = 3f;
                        minions[i].layer = 8;
                        minions[i].GetComponent<smallRobotController>().currentState = "idle";
                    }
                }
            }


            // if health is 0 this big guy is dead 
            Destroy(gameObject);


        }
    }

    
}
