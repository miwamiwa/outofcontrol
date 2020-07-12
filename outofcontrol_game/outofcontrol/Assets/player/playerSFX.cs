using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSFX : MonoBehaviour
{
    AudioSource footstep;
    AudioSource robotCollision;
    AudioSource aie;
    AudioSource ouille;
    AudioSource swish1;
    AudioSource swish2;
    AudioSource oxygenStartSound;
    AudioSource oxygenOut;
    AudioSource pickup;
    AudioSource spawn;
    AudioSource capture;

    bool stepsActive = false;
    int stepCounter = 0;
    int nextStep = 0;
    int stepInterval = 12;
    float basePitch = 2.75f;
    float lastRobotCollision = 0f;
    int oxygenFadeoutCounter = 200;
    int footStepStop = 100;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] aSources = GetComponents<AudioSource>();
        footstep = aSources[0];
        robotCollision = aSources[1];
        aie = aSources[2];
        ouille = aSources[3];
        swish1 = aSources[4];
        swish2 = aSources[5];
        oxygenStartSound = aSources[6];
        oxygenOut = aSources[7];
        pickup = aSources[8];
        spawn = aSources[9];
        capture = aSources[10];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stepsActive)
        {
            if (stepCounter == nextStep)
            {
                footstep.pitch = basePitch + Random.Range(-0.2f, 0.2f);
                if (footstep.isPlaying)
                    footstep.Stop();
                footstep.Play();
                nextStep = stepCounter + stepInterval + Random.Range(-1, 2);
            }
            stepCounter++;

            if (stepCounter == footStepStop)
            {
                stepsActive = false;
            }
        }

        if (oxygenFadeoutCounter < 150)
        {
            oxygenStartSound.volume = 1-oxygenFadeoutCounter / 150f;
        }
        else if(oxygenFadeoutCounter==150) oxygenStartSound.Stop();

        if (oxygenStartSound.volume < 1 && oxygenStartSound.isPlaying)
        {
            oxygenStartSound.volume = Mathf.Clamp(oxygenStartSound.volume + 0.01f, 0f, 1f);
        }
        oxygenFadeoutCounter++;
    }

    public void triggerFootSteps()
    {
       // 
        
        stepsActive = true;
        // stepCounter = 0;
        footStepStop = stepCounter + 5;
    }

    public void stopFootSteps()
    {
        
        stepsActive = false;
    }

    public void triggerRobotCollision()
    {
        float timeSinceLast = Time.time - lastRobotCollision;
        if(timeSinceLast>0.3f)
        robotCollision.Play();

        lastRobotCollision = Time.time;

        float chance = Random.Range(0f, 6f);
        if (chance > 4f)
        {
            chance = Random.Range(0f, 6f);
            if (chance > 3f &&!aie.isPlaying) aie.Play();
            else if(!ouille.isPlaying) ouille.Play();
        }
    }

    public void swishStick()
    {
        float chance = Random.Range(0f, 2f);
        if (chance > 1f) swish1.Play();
        else swish2.Play();
    }

    public void oxygenStart()
    {
        Debug.Log("yo");
        //if(!oxygenStartSound.isPlaying)
        //oxygenStartSound.volume = 1f;
       oxygenFadeoutCounter = 200;
        oxygenStartSound.Play();
    }

    public void stopOxygenSound()
    {
        oxygenFadeoutCounter = 0;
    }

    public void oxygenRanOut()
    {
        Debug.Log("ded");
        oxygenOut.Play();
        stopOxygenSound();
    }

    public void pickupSFX()
    {
        if (!pickup.isPlaying)
            pickup.Play();
    }

    public void triggerSpawn()
    {
        spawn.Play();
    }

    public void captureSFX()
    {
        capture.Play();
    }
}
