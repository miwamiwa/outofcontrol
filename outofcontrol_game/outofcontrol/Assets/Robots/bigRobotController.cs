using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bigRobotController : MonoBehaviour
{
    GameObject player;
    public GameObject smallRobot;

    
    string currentState = "idle";
    Vector3 currentTarget;
    Vector3 fightTarget;

    float harvestVelocity = 0.01f; // speed while moving towards a tree 
    float attackVelocity = 0.03f; // speed while attacking

    int attackCounter = 0;
    int chargeCounter = 0;
    int nextAttack = 0;

    int chargeLength = 22; // how far the robot charges 

    float targetReachedMargin = 0.1f;
    float aggroRadius = 4f;

    int minionCount = 3; // how many small robots spawn on start 
    public GameObject[] minions = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {   
        // pointer for player
        player = GameObject.Find("Player");

        // instantiate minions 
        for(int i=0; i<minionCount; i++)
        {
            minions[i]= Instantiate(smallRobot, transform.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f)), Quaternion.identity);
            minions[i].GetComponent<smallRobotController>().parentRobot = gameObject;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentState == "idle")
        {
            pickTarget();
            
        }
        else if(currentState == "harvesting")
        {
            

            Vector3 distance = new Vector3(transform.position.x - currentTarget.x, 0, transform.position.z - currentTarget.z);
            // if target reached 
            if (distance.magnitude < targetReachedMargin)
            {
                currentState = "idle";
            }

         
            Vector3 distToPlayer = transform.position - player.transform.position;

            if (distToPlayer.magnitude < aggroRadius)
                // if player is in range move towards player 
            {
                if (attackCounter > nextAttack)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, attackVelocity);
                    chargeCounter++;
                    if (chargeCounter > chargeLength)
                    {
                        nextAttack = attackCounter + 200;
                        chargeCounter = 0;
                        fightTarget = player.transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f));
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, fightTarget, attackVelocity);
                }
                
            }
            else
            // otherwise move towards the target 
            {
                transform.position = Vector3.MoveTowards(transform.position, currentTarget, harvestVelocity);
            }

            attackCounter++;
        }
    }

    void pickTarget()
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        if (trees.Length > 0)
        {
            int randomPick = Random.Range(0, trees.Length);
            Vector3 pos = trees[randomPick].transform.position;
            currentTarget = new Vector3(pos.x, 0f, pos.z);
            currentState = "harvesting";
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Tree") Destroy(collision.gameObject);
        if (collision.gameObject.name == "Stick" && GameObject.Find("Player").GetComponent<playerController>().playerAttacking)
        {
            gameObject.GetComponent<robotHealth>().hitPoints -= 10f;
        }
    }

}
