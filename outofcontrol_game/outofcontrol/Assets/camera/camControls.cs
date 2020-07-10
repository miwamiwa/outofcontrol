using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camControls : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = target.transform.position + new Vector3(-3f, 5f, 0);
    }
}
