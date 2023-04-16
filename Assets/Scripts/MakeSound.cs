using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeSound : MonoBehaviour
{
    //chuck variables
    ChuckSubInstance myChuck;
    ChuckFloatSyncer myFloatSyncer;
    
    //play/pause button variables
    public bool soundPaused = true; 
    public AudioSource audioSource;

    //setting variable for image color
    private Image backGround;

    //accelerometer variables
    private Vector3 smoothedAccelerometerData;
    public float smoothingValue = 0.2f;

    
    void Start()
    {
        //initalise chuck
        myChuck = GetComponent<ChuckSubInstance>();
        
        //run chuck script
        myChuck.RunCode(@"
            //global effect parameters
            global float myDelayGain;
            global float myReverbGain1;
            global float myReverbGain2;
            global float myFilterFreq;
            global float myFilterRes;

            2::second => dur beat;
            SinOsc osc => ADSR env1 => NRev rev1 => dac;
            (beat / 2, beat / 2, 0, 1::ms) => env1.set;
            0.2 => osc.gain;

            myReverbGain1 => rev1.mix;

            SinOsc osc2 => ADSR env2 => LPF filter => NRev rev2 => dac;
            env2 => Delay delay2 => dac;
            delay2 => delay2;

            (4::ms, beat / 8, 0, 1::ms) => env2.set;
            0.4 => osc2.gain;

            beat => delay2.max;
            beat / 8 => delay2.delay;
            myDelayGain => delay2.gain;

            myReverbGain2 => rev2.mix;

            10000 => filter.freq;
            10 => filter.Q;


            [0, 4, 7, 12] @=> int major[];
            [0, 3, 7, 12] @=> int minor[];

            48 => int offset;
            int position;

            fun void playTwoBars(int position, int chord[]) 
            {    
                for(0 => int i; i < 4; i++) 
                {
                    Std.mtof(chord[0] + offset + position) => osc.freq;
                    1 => env1.keyOn;
                    for(0 => int j; j < 4; j++) {
                        Std.mtof(chord[j] + offset + position + 12) => osc2.freq;
                        1 => env2.keyOn;
                        beat / 8 => now;
                    }
                }
            }

            fun void playMelody() {
                while(true) {
                    playTwoBars(0, minor);
                    playTwoBars(-4, major);
                    playTwoBars(-2, minor);
                    playTwoBars(-5, major);

                    playTwoBars(-7, minor);
                    playTwoBars(-2, major);
                    playTwoBars(3, major);
                    playTwoBars(-5, major);   
                }
            }
            global Event playSound;
            
            chout <= ""Rate is "" <= delay2.gain() <= IO.newline();
            
            while(true) {
                playSound => now;
                // start shred with melody loop
                playMelody();
            }

            ");

        //get background color of safearee
        backGround = GameObject.Find("SafeArea").GetComponent<Image>();

        // create a callback to use with GetFloar in Update()
        //myFloatSyncer = gameObject.AddComponent<ChuckFloatSyncer>();

        // start syncing effect parameters
        /* myFloatSyncer.SyncFloat(myChuck, "myDelayGain"); //delay gain
        myFloatSyncer.SyncFloat(myChuck, "myReverbGain1"); //reverb1 gain
        myFloatSyncer.SyncFloat(myChuck, "myReverbGain2"); //reverb2 gain
        myFloatSyncer.SyncFloat(myChuck, "myFilterFreq"); //filter freq
        myFloatSyncer.SyncFloat(myChuck, "myFilterRes"); //filter resonance */
        
    }

    void Update()
    {
        AccelerometerDataHandler();

    }

    //play/pause script for button
    public void PlaySound() 
    {
        soundPaused = !soundPaused;

        if (soundPaused)
        {            
            audioSource.Pause();

        } else
        {
            myChuck.BroadcastEvent("playSound");
            audioSource.UnPause();    
        }
    }

    public void AccelerometerDataHandler() {
        // Get accelerometer data
        Vector3 accelerometerData = Input.acceleration;

        // low-pass filter of accelerometer data
        smoothedAccelerometerData = Vector3.Lerp(smoothedAccelerometerData, accelerometerData, smoothingValue);

        // Map accelerometer data netween 0 and 1 and to RGB color
        float x_axis = Mathf.Clamp01(smoothedAccelerometerData.x + 0.5f);
        float y_axis = Mathf.Clamp01(smoothedAccelerometerData.y + 0.5f);
        float z_axis = Mathf.Clamp01(smoothedAccelerometerData.z + 0.5f);

        // Set object color to bg
        backGround.material.color = new Color(x_axis, y_axis, z_axis);
        //Debug.Log("Accelerometer Data: X = " + smoothedAccelerometerData.x + ", Y = " + smoothedAccelerometerData.y + ", Z = " + smoothedAccelerometerData.z);

        //remap values to handpicked effect parameters
        float delayData = Mathf.Clamp(x_axis, 0.0f, 0.7f);
        Debug.Log(delayData);

        //send values to chuck
        myChuck.SetFloat("myDelayGain", delayData);
    }

        
}
