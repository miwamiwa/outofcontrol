﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    public Animator anim;
    public GameObject pivot;
    public GameObject stick;
    float lastPlayerRotation = 0f;
    public float playerVelocity = 0.04f;

    float angle1 = -1f;
    float angle2 = -1f;
    float playerRotation = 0f;

    float stickRestAngle = 0f;
    float stickAttackAngle = 90f;
    float initialSwingAngle = 30f;
    float swingSpeed = 10f;

    float distanceToTreeThreshold = 2.2f;

    public float oxygen = 100f;


    float airLossRate = 0.9f;

    float pickupRange = 2f;
    float setSpawnPointRange = 3f;
    bool itemPickedUp = false;
    Vector3 stickAttackPos = new Vector3(0.3f, -0.4f, 0f);

    Rigidbody rigidbody;

    public bool pendingSpawn = true;

    public bool playerAttacking = false;
    int attackCounter = 0;
    int attackLength = 9; // frames

    bool lastMoving = false;
    bool lastPlayerSafe = true;
    float lastOxygen = 100f;

    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigidbody = gameObject.GetComponent<Rigidbody>();

        anim.SetLayerWeight(1, 0f);
    }

    private void Update()
    {
        checkMotionInput();
        checkStickInput();
        checkPickup();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log("update");
        handleSpawning();
        updateMovement();
        updateStick();
        updateOxygen();
    }

    void handleSpawning()
    {
        if (pendingSpawn)
        {
            GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
            for(int i=0; i<checkpoints.Length; i++)
            {
                if (checkpoints[i].GetComponent<checkpointHandler>().isSpawnPoint)
                {
                    Vector3 pos = checkpoints[i].transform.position;
                    transform.position = new Vector3(pos.x, transform.position.y, pos.z);
                    pendingSpawn = false;
                    GetComponent<playerSFX>().triggerSpawn();
                }
            }
        }
    }

    void updateOxygen()
    {
        
        bool playerSafe = false; //  = can breathe

        // if near trees, we can breathe
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        for (int i = 0; i < trees.Length; i++)
        {
            Vector3 distance = trees[i].transform.position - transform.position;
            if (distance.magnitude < distanceToTreeThreshold)
            {
                playerSafe = true;
            }
        }

        bool playerWon = true;

        // if near checkpoint, we can also breathe
        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        for (int i = 0; i < checkpoints.Length; i++)
        {
            Vector3 distance = checkpoints[i].transform.position - transform.position;
            if (distance.magnitude < distanceToTreeThreshold)
            {
                playerSafe = true;

                // also, if base not conquered, conquer it 

                if (!checkpoints[i].GetComponent<checkpointHandler>().isCaptured) {
                    checkpoints[i].GetComponent<checkpointHandler>().isCaptured = true;
                    GetComponent<playerSFX>().captureSFX();
                    checkpoints[i].GetComponent<Renderer>().material = checkpoints[i].GetComponent<checkpointHandler>().conqueredColor;
                }
            }


            // while we're here, check if game is over

            if (!checkpoints[i].GetComponent<checkpointHandler>().isCaptured) playerWon = false;
            
        }

        if(playerWon) Application.LoadLevel(2); 

        if (!playerSafe)
        {
            oxygen= Mathf.Clamp(oxygen-airLossRate,0f,100f);

            if (lastPlayerSafe) GetComponent<playerSFX>().oxygenStart();
            if (oxygen == 0 && lastOxygen != 0) GetComponent<playerSFX>().oxygenRanOut();
        }
        else if (oxygen < 100)
        {
            if (!lastPlayerSafe) GetComponent<playerSFX>().stopOxygenSound();
            oxygen = Mathf.Clamp(oxygen + 2, 0f, 100f);
        }

        lastPlayerSafe = playerSafe;
        lastOxygen = oxygen;
    }

    void checkMotionInput()
    {
        angle1 = -1f;
        angle2 = -1f;
        bool moving = false;

        if (Input.GetKey(KeyCode.W))
        {
            angle1 = 0f;
            moving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            angle1 = 180f;
            moving = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            angle2 = 90f;
            moving = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            angle2 = 270f;
            moving = true;
        }

        if (moving) GetComponent<playerSFX>().triggerFootSteps();
        
    }
    void checkStickInput()
    {
        if (Input.GetMouseButtonDown(0) && !playerAttacking)
        {
            //Debug.Log("start swing");
            playerAttacking = true;
            attackCounter = 0;
           // stick.transform.Translate(stickAttackPos);
            stick.transform.localEulerAngles = new Vector3(0f, initialSwingAngle, 0f);
            //anim.Play("hit");
            anim.SetLayerWeight(1, 1f);
            GetComponent<playerSFX>().swishStick();
        }
    }

    void checkPickup()
    {
        GameObject[] beacons = GameObject.FindGameObjectsWithTag("Beacon");

        if (Input.GetKeyDown("space"))
        {

            if (!itemPickedUp)
            {
                for (int i = 0; i < beacons.Length; i++)
                {
                    Vector3 distance = beacons[i].transform.position - new Vector3(transform.position.x,0.5f, transform.position.z);
                    if (distance.magnitude < pickupRange)
                    {
                        beacons[i].transform.SetParent(transform);
                        GetComponent<playerSFX>().pickupSFX();
                    }
                }

            }
            else
            {
                GetComponent<playerSFX>().pickupSFX();
                for (int i = 0; i < beacons.Length; i++)
                {
                   
                        beacons[i].transform.SetParent(null);
                    
                }
            }

            itemPickedUp = !itemPickedUp;

            if (!itemPickedUp)
            {
                int newSpawn = -1;
                GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
                for(int i=0; i<checkpoints.Length; i++)
                {
                    Vector3 distance = checkpoints[i].transform.position - transform.position;
                    if (distance.magnitude < setSpawnPointRange)
                    {
                        checkpoints[i].GetComponent<checkpointHandler>().isSpawnPoint = true;
                        checkpoints[i].GetComponent<Renderer>().material = checkpoints[i].GetComponent<checkpointHandler>().spawnerColor;
                        newSpawn = i;
                    }
                }

                if (newSpawn >= 0)
                {
                    for (int i = 0; i < checkpoints.Length; i++)
                    {
                        //Vector3 distance = checkpoints[i].transform.position - transform.position;
                        if (i!=newSpawn)
                        {
                            checkpoints[i].GetComponent<checkpointHandler>().isSpawnPoint = false;

                            if(checkpoints[i].GetComponent<checkpointHandler>().isCaptured)
                                checkpoints[i].GetComponent<Renderer>().material = checkpoints[i].GetComponent<checkpointHandler>().conqueredColor;
                            else checkpoints[i].GetComponent<Renderer>().material = checkpoints[i].GetComponent<checkpointHandler>().defaultColor;
                        }
                    }
                }
            }
            
        }
    }


    void updateStick()
    {
        
        if (playerAttacking)
        {
            
           // stick.transform.localEulerAngles = new Vector3(0f, initialSwingAngle - attackCounter*swingSpeed, stickAttackAngle);
            attackCounter++;
            if (attackCounter > attackLength)
            {
                playerAttacking = false;
                anim.SetLayerWeight(1, 0f);
                stick.transform.localEulerAngles = new Vector3(0f, 0f, stickRestAngle);
                //  stick.transform.Translate(-stickAttackPos);//(new Vector3(1f,0.4f,0.2f));
                stick.transform.localPosition = new Vector3(-2.83f, 0.87f, 0.62f);
            }
            else stick.transform.RotateAround(pivot.transform.position, Vector3.up, 4f*swingSpeed);
        }
    }


    // updatemovement()
    //
    // update player movement and rotation 

    void updateMovement()
    {
        

        playerRotation = lastPlayerRotation;

        if (angle1 == -1f && angle2 != -1f) playerRotation = angle2;
        else if (angle2 == -1f && angle1 != -1f) playerRotation = angle1;
        else if (angle2 != -1f && angle1 != -1f)
        {
            if (angle1 == 0f && angle2 == 270f) angle1 = 360f;
            playerRotation = (angle1 + angle2) / 2f;
        }

        gameObject.transform.eulerAngles=new Vector3(0f, 180f+playerRotation, 0f);
        lastPlayerRotation = playerRotation;
        

        if (angle2 != -1f || angle1 != -1f)
        {
            //Debug.Log(playerRotation + " " + angle1 + " " + angle2);
            //Debug.Log(playerVelocity * Mathf.Sin(playerRotation * Mathf.Deg2Rad));
            //Debug.Log(playerVelocity * Mathf.Cos(playerRotation * Mathf.Deg2Rad))   ;
            gameObject.transform.Translate(new Vector3(
                playerVelocity * Mathf.Cos(playerRotation * Mathf.Deg2Rad),
                0f, 
               - playerVelocity * Mathf.Sin(playerRotation * Mathf.Deg2Rad)
                ), Space.World);
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
          
       
    }
}
