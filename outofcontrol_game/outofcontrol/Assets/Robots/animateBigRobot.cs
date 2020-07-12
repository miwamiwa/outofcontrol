using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animateBigRobot : MonoBehaviour
{
    public GameObject BigSaw;
    public GameObject SmallSaw;

    float blowUpForce = 0.5f;

    float bigSawCounter = 0f;
    float smallSawCounter = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        BigSaw.transform.localEulerAngles = new Vector3(-90f, bigSawCounter, 0f);
        SmallSaw.transform.localEulerAngles = new Vector3(90f, smallSawCounter, 0f);
        bigSawCounter -= 4.0f;
        smallSawCounter += 2.5f;


        //  Debug.Log(GetComponent<robotHealth>().deadCounter);
        if (GetComponent<robotHealth>().deadCounter != 0)
        {
            

                if(GetComponent<robotHealth>().deadCounter == 2)
                {
                foreach (Transform child in transform)
                {

                    Debug.Log("explode");
                    float speed = 600f;
                    Rigidbody rb = child.gameObject.AddComponent<Rigidbody>();
                    rb.isKinematic = false;
                  //  Vector3 force = transform.forward;
                    Vector3 force = new Vector3(Random.Range(-blowUpForce, blowUpForce), Random.Range(-blowUpForce, blowUpForce), Random.Range(-blowUpForce, blowUpForce));
                    rb.AddForce(force * speed);
                }
            }
               

               // child.Translate( new Vector3 (Random.Range(-0.1,0.1)) );
           
        }
        
    }
}
