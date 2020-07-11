using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotHealthBar : MonoBehaviour
{
    public GameObject target;
    Vector3 rotation;
    public GameObject healthbar;
    Vector3 initScale;

    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        rotation = new Vector3(0f, 0f, -80f);
        mat = healthbar.GetComponent<Renderer>().material;

        initScale = healthbar.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = target.transform.position + new Vector3(0f, 1f, 0);
        gameObject.transform.eulerAngles = rotation;

        float health = target.GetComponent<robotHealth>().hitPoints;
        // Debug.Log(health);

        healthbar.transform.localScale = new Vector3(initScale.x, initScale.y, initScale.z * health / 100f);
    }
}
