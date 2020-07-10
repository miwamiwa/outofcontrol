using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    public GameObject stick;
    float lastPlayerRotation = 0f;
    public float playerVelocity = 0.04f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("update");
        updateMovement();
    }

    void updateMovement()
    {
        float angle1 = -1f;
        float angle2 = -1f;

        if (Input.GetKey(KeyCode.W)) angle1 = 0f;
        else if (Input.GetKey(KeyCode.S)) angle1 = 180f;

        if (Input.GetKey(KeyCode.D)) angle2 = 90f;
        else if (Input.GetKey(KeyCode.A)) angle2 = 270f;

        float playerRotation = lastPlayerRotation;

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
          
    }
}
