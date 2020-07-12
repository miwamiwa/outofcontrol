using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animateSmallRobot : MonoBehaviour
{

    public GameObject saw;
    float sawCounter = 0f;
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
       // smallSawCounter += 2.5f;
    }
}
