using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animateBigRobot : MonoBehaviour
{
    public GameObject BigSaw;
    public GameObject SmallSaw;

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
    }
}
