﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotSpawn : MonoBehaviour
{

    public int activeSpawns = 0;

    float spawnInterval = 10f;
    int maxSpawns = 3;
    float nextSpawnTime = 0f;
    float spawnRadius = 20f;

    public GameObject bigrobot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        

         if (Time.time > nextSpawnTime && activeSpawns < maxSpawns)
        {
            // increase difficulty as time goes on
            if (Time.time > 60f) maxSpawns = 4;
            if (Time.time > 120f) maxSpawns = 5;


            Vector2 randompos = Random.insideUnitCircle * spawnRadius;
            Vector3 pos = new Vector3(randompos.x, 0.53f, randompos.y);
            Instantiate(bigrobot, pos, Quaternion.identity);

            activeSpawns++;
            nextSpawnTime = Time.time + spawnInterval;
        }
    }
}
