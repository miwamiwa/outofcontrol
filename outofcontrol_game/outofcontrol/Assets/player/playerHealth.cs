using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{

    public float hitPoints = 100f;
    float bigRobotDamagePerFrame = 0.4f;
    float smallRobotDamagePerFrame = 0.2f;
    public UnityEngine.UI.Text counter;

    int lives = 5;
    // Start is called before the first frame update
    void Start()
    {
        counter.text = lives.ToString();
    }

    // Update is called once per frame
    void Update()
    {
      

        if (GetComponent<playerController>().oxygen <= 0) hitPoints--;

        hitPoints = Mathf.Clamp(hitPoints, 0f, 100f);

        if (hitPoints <= 0)
        {
            //Debug.Log("player dead!");
            GetComponent<playerController>().pendingSpawn = true;
            hitPoints = 100;
            lives--;

            counter.text = lives.ToString();
            if (lives == 0)
            {
                // game is over, player fully ded 
                Debug.Log("ded!!");
                Application.LoadLevel(3);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log("collision");
        if (collision.gameObject.tag == "BigRobot")
        {
            hitPoints -= bigRobotDamagePerFrame;
            GetComponent<playerSFX>().triggerRobotCollision();
            //Debug.Log(hitPoints);
        }

        if (collision.gameObject.tag == "SmallRobot")
        {
            hitPoints -= smallRobotDamagePerFrame;
            GetComponent<playerSFX>().triggerRobotCollision();
            //Debug.Log(hitPoints);
        }
    }
}
