using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
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
    Vector3 stickAttackPos = new Vector3(0.3f, -0.4f, 0f);

    Rigidbody rigidbody;

    public bool playerAttacking = false;
    int attackCounter = 0;
    int attackLength = 9; // frames
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        checkMotionInput();
        checkStickInput();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log("update");
        updateMovement();
        updateStick();
    }

    void checkMotionInput()
    {
        angle1 = -1f;
        angle2 = -1f;

        if (Input.GetKey(KeyCode.W)) angle1 = 0f;
        else if (Input.GetKey(KeyCode.S)) angle1 = 180f;

        if (Input.GetKey(KeyCode.D)) angle2 = 90f;
        else if (Input.GetKey(KeyCode.A)) angle2 = 270f;
    }
    void checkStickInput()
    {
        if (Input.GetMouseButtonDown(0) && !playerAttacking)
        {
            Debug.Log("start swing");
            playerAttacking = true;
            attackCounter = 0;
            stick.transform.Translate(stickAttackPos);
            stick.transform.localEulerAngles = new Vector3(0f, initialSwingAngle, stickAttackAngle);
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
                
                stick.transform.localEulerAngles = new Vector3(0f, 0f, stickRestAngle);
                //  stick.transform.Translate(-stickAttackPos);//(new Vector3(1f,0.4f,0.2f));
                stick.transform.localPosition = new Vector3(0.83f, 0.87f, -0.22f);
            }
            else stick.transform.RotateAround(pivot.transform.position, Vector3.up, -swingSpeed);
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

        gameObject.transform.eulerAngles=new Vector3(0f, playerRotation, 0f);
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
