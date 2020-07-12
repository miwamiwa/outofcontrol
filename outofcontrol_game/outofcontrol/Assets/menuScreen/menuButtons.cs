using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuButtons : MonoBehaviour
{
    AudioSource click;
    // Start is called before the first frame update
    void Start()
    {
        click = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        Application.LoadLevel(1);
        click.Play();
    }

    public void QuitGame()
    {
        Application.Quit();
        click.Play();
    }
}
