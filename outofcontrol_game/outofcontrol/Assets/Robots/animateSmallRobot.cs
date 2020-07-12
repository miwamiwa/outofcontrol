using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animateSmallRobot : MonoBehaviour
{

    public GameObject saw;
    float sawCounter = 0f;

    float blowUpForce = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        saw.transform.localEulerAngles = new Vector3(-90f, sawCounter, 0f);
    //    SmallSaw.transform.localEulerAngles = new Vector3(90f, smallSawCounter, 0f);

        sawCounter -= 3.0f;

        //  Debug.Log(GetComponent<robotHealth>().deadCounter);
        if (GetComponent<robotHealth>().deadCounter != 0)
        {


            if (GetComponent<robotHealth>().deadCounter == 2)
            {
                foreach (Transform child in transform)
                {

                    Debug.Log("explode");
                    float speed = 600f;
                    Rigidbody rb = child.gameObject.AddComponent<Rigidbody>();
                    rb.isKinematic = false;
                    //  Vector3 force = transform.forward;
                    Vector3 force = new Vector3(Random.Range(-blowUpForce, blowUpForce), Random.Range(0f, blowUpForce), Random.Range(-blowUpForce, blowUpForce));
                    rb.AddForce(force * speed);
                }
            }


            // child.Translate( new Vector3 (Random.Range(-0.1,0.1)) );

        }
        // smallSawCounter += 2.5f;
    }
}
