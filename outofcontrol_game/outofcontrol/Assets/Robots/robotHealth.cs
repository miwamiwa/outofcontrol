using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotHealth : MonoBehaviour
{
    public float hitPoints = 100f;
    public GameObject beaconObject;

    AudioSource deadSFX;
    public int deadCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] aSources = GetComponents<AudioSource>();
        deadSFX = aSources[4];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hitPoints <= 0&&deadCounter==0)
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
            deadCounter = 1;
            
            // if health is 0 this big guy is dead 
            


        }

        if (deadCounter > 0)
        {
            if(deadCounter==1) deadSFX.Play();
            if (deadCounter>60) Destroy(gameObject);
            deadCounter++;
        }
    }

    
}
