using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smallRobotController : MonoBehaviour
{

    AudioSource rattle;
    AudioSource hitSound;

    public GameObject parentRobot;
    public GameObject treeObject;
    AudioSource dash;
    AudioSource cutTree;
    AudioSource treePlantingSound;
    GameObject player;


    int plantingInterval = 200;
    public Material friendlyHealthBarMat;
    public GameObject healthBarObj;

    public float maxStrayDistance = 7f;

    public string currentState = "idle";

    Vector3 currentTarget;
    Vector3 fightTarget;

    public bool friendly = false;

    float harvestVelocity = 0.01f; // speed while moving towards a tree 
    float attackVelocity = 0.03f; // speed while attacking

    int attackCounter = 0;
    int chargeCounter = 0;
    int nextAttack = 0;

    int chargeLength = 22; // how far the robot charges 

    float targetReachedMargin = 0.1f;
    float aggroRadius = 4f;
    int fCounter = 0;
    int minionCount = 3; // how many small robots spawn on start 
    GameObject[] minions = new GameObject[3];

    float rattleRange = 10f;
    

    float noTreeRange = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        // pointer for player
        player = GameObject.Find("Player");

        AudioSource[] aSources = GetComponents<AudioSource>();
        rattle = aSources[0];
        hitSound = aSources[1];
        dash = aSources[2];
        cutTree = aSources[3];
        treePlantingSound=aSources[5];

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
            if (currentState == "idle")
            {
                pickTarget();

            }
            else if (currentState == "harvesting")
            {


                Vector3 distance = new Vector3(transform.position.x - currentTarget.x, 0, transform.position.z - currentTarget.z);
                // if target reached 
                if (distance.magnitude < targetReachedMargin)
                {
                    currentState = "idle";
                }


                Vector3 distToPlayer = transform.position - player.transform.position;

            // trigger rattle sound on/off 
            if (distToPlayer.magnitude < rattleRange && !rattle.isPlaying) rattle.UnPause();
            else if (distToPlayer.magnitude >= rattleRange && rattle.isPlaying) rattle.Pause();


                if (distToPlayer.magnitude < aggroRadius && !friendly)
                // if player is in range move towards player 
                {
                    if (attackCounter > nextAttack)
                    {
                    if (chargeCounter == 0) dash.Play();
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

        if (friendly)
        // if robot is friendly
        {
            if (fCounter == 0)
            {
                // make health bar green 
                healthBarObj.GetComponent<Renderer>().material = friendlyHealthBarMat;
            }
            fCounter++;

            // spawn trees 
            if (fCounter % plantingInterval == 0)
            {
                GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
                bool nevermind = false;
                for(int i=0; i<trees.Length; i++)
                {
                    Vector3 distance = trees[i].transform.position - transform.position;
                    if (distance.magnitude < noTreeRange) nevermind = true;
                }
                if (!nevermind)
                {
                    Vector2 offset = 2f * Random.insideUnitCircle;
                    Vector3 newPos = new Vector3(offset.x + transform.position.x, 1.92f, offset.y + transform.position.z);
                    Instantiate(treeObject, newPos, Quaternion.identity);
                    treePlantingSound.Play();
                }
                
            }
        }
        
    }

    void pickTarget()
    {
        float range = maxStrayDistance;
        Vector2 randompos = range*Random.insideUnitCircle;

        currentTarget = parentRobot.transform.position + new Vector3(randompos.x, 0f, randompos.y);
       //Debug.Log(currentTarget);
        currentState = "harvesting";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Tree" && !friendly)
        {
            Destroy(collision.gameObject);
            cutTree.Play();
        }
        if (collision.gameObject.name == "Stick" && GameObject.Find("Player").GetComponent<playerController>().playerAttacking)
        {
            gameObject.GetComponent<robotHealth>().hitPoints -= 40f;
            hitSound.Play();
        }
    }
}
