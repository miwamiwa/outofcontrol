using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oxygenBar : MonoBehaviour
{

    public GameObject target;
    Vector3 rotation;
    public GameObject healthbar;
    Vector3 initScale;

    int hideCounter = 0;
    int hideCounterThreshold = 50;

    // Start is called before the first frame update
    void Start()
    {
        rotation = new Vector3(0f, 0f, -80f);

        initScale = healthbar.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        target.transform.position = transform.position + new Vector3(-1f, 1f, 0);
        target.transform.eulerAngles = rotation;

        float health = GetComponent<playerController>().oxygen;
        // Debug.Log(health);

        healthbar.transform.localScale = new Vector3(initScale.x, initScale.y, initScale.z * health / 100f);

        Debug.Log(health);
        if (health != 100)
        {
            target.SetActive(true);
            hideCounter = 0;
        }
        
        else
        {
            
            hideCounter++;
            if (hideCounter > hideCounterThreshold)
            {
                target.SetActive(false);
            }
        }
    }
}
