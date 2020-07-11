﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealth : MonoBehaviour
{

    public float hitPoints = 100f;
    float bigRobotDamagePerFrame = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      

        if (GetComponent<playerController>().oxygen <= 0) hitPoints--;

        hitPoints = Mathf.Clamp(hitPoints, 0f, 100f);

        if (hitPoints <= 0)
        {
            Debug.Log("player dead!");
            GetComponent<playerController>().pendingSpawn = true;
            hitPoints = 100;

        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log("collision");
        if (collision.gameObject.tag == "BigRobot")
        {
            hitPoints -= bigRobotDamagePerFrame;
            //Debug.Log(hitPoints);
        }
    }
}
