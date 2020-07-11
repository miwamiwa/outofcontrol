using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotHealth : MonoBehaviour
{
    float hitPoints = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hitPoints <= 0)
        {
            Destroy(gameObject);
            // robot is dead
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       // if hit by player's stick while player is attacking 
        if (collision.gameObject.name == "Stick" && GameObject.Find("Player").GetComponent<playerController>().playerAttacking)
        {
            hitPoints -= 10f;
        }
    }
}
