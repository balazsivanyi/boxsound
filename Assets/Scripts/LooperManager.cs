using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooperManager : MonoBehaviour
{
    ChuckSubInstance myChuck;
    ChuckFloatSyncer myFloatSyncer;

    string[] buttons = {"BottomRightButton", "BottomLeftButton", "TopRightButton", "TopLeftButton"};
    string[] sliders = {"Pitch1", "Volume1", "Filter1", "Pitch2", "Volume2", "Filter2", "Pitch3", "Volume3", "Filter3", "Pitch4", "Volume4", "Filter4",};

    // Start is called before the first frame update
    void Start()
    {
        myChuck = GetComponent<ChuckSubInstance>();
        myChuck.RunCode(@"
            //a simple signal path
            adc => LiSa loopme1 => LPF filter1 => dac;
            adc => LiSa loopme2 => LPF filter2 => dac;
            adc => LiSa loopme3 => LPF filter3 => dac;
            adc => LiSa loopme4 => LPF filter4 => dac;

            //sampler setup
            //allocating memory ini timie
            2::second => loopme2.duration;
            2::second => loopme1.duration;
            2::second => loopme3.duration;
            2::second => loopme4.duration;
            //ramping for the edges of the recording so it doesn't clip
            10::ms => loopme1.recRamp;
            10::ms => loopme2.recRamp;
            10::ms => loopme3.recRamp;
            10::ms => loopme4.recRamp;

            //global effect parameters
            1 => global float myPitch1;
            0.5 => global float myVolume1;
            10000 => global float myFilter1;

            1 => global float myPitch2;
            0.5 => global float myVolume2;
            10000 => global float myFilter2;
            
            1 => global float myPitch3;
            0.5 => global float myVolume3;
            10000 => global float myFilter3;

            1 => global float myPitch4;
            0.5 => global float myVolume4;
            10000 => global float myFilter4;


        
            fun void recordSound1(Event start) {
                start => now;
                //loopme1.clear();
                //start recording input
                loopme1.record(1);
                //begin ramping down
                1600::ms => now;
                400::ms => loopme1.recRamp;
                //wait for ramp to finish, then stop recording
                400::ms => now;
                loopme1.record(0);
            }

            fun void playRecording1(Event start) {
                start => now;
                //set effeect parameters
                myPitch1 => loopme1.rate;
                myVolume1 => loopme1.gain;
                myFilter1 => filter1.freq;
                //start looping
                1 => loopme1.loop;
                //1 => loopme1.bi;
                1 => loopme1.play;
                while(true) {500::ms => now;}
                }

            fun void recordSound2(Event start) {
                start => now;
                //loopme2.clear();
                //start recording input
                loopme2.record(1);
                //begin ramping down
                1600::ms => now;
                400::ms => loopme2.recRamp;
                //wait for ramp to finish, then stop recording
                400::ms => now;
                loopme2.record(0);
            }

            fun void playRecording2(Event start) {
                start => now;
                //set effeect parameters
                myPitch2 => loopme2.rate;
                myVolume2 => loopme2.gain;
                myFilter2 => filter2.freq;
                //start looping
                1 => loopme2.rate;
                1 => loopme2.loop;
                //1 => loopme2.bi;
                1 => loopme2.play;
                while(true) {500::ms => now;}
            }

            fun void recordSound3(Event start) {
                start => now;
                //loopme3.clear();
                //start recording input
                loopme3.record(1);
                //begin ramping down
                1600::ms => now;
                400::ms => loopme3.recRamp;
                //wait for ramp to finish, then stop recording
                400::ms => now;
                loopme3.record(0);
            }

            fun void playRecording3(Event start) {
                start => now;
                //set effeect parameters
                myPitch3 => loopme3.rate;
                myVolume3 => loopme3.gain;
                myFilter3 => filter3.freq;
                //start looping
                1 => loopme3.rate;
                1 => loopme3.loop;
                //1 => loopme3.bi;
                1 => loopme3.play;
                while(true) {500::ms => now;}
            }

            fun void recordSound4(Event start) {
                start => now;
                //loopme4.clear();
                //start recording input
                loopme4.record(1);
                //begin ramping down
                1600::ms => now;
                400::ms => loopme4.recRamp;
                //wait for ramp to finish, then stop recording
                400::ms => now;
                loopme4.record(0);
            }

            fun void playRecording4(Event start) {
                start => now;
                //set effeect parameters
                myPitch4 => loopme4.rate;
                myVolume4 => loopme4.gain;
                myFilter4 => filter4.freq;
                //start looping
                1 => loopme4.rate;
                1 => loopme4.loop;
                //1 => loopme4.bi;
                1 => loopme4.play;
                while(true) {500::ms => now;}
            }

            global Event letsRecord1;
            global Event letsPlayBack1;

            global Event letsRecord2;
            global Event letsPlayBack2;

            global Event letsRecord3;
            global Event letsPlayBack3;

            global Event letsRecord4;
            global Event letsPlayBack4;


            while (true) 
            {
                spork ~ recordSound1(letsRecord1);
                spork ~ playRecording1(letsPlayBack1);
                myPitch1 => loopme1.rate;
                myVolume1 => loopme1.gain;
                myFilter1 => filter1.freq;

                spork ~ recordSound2(letsRecord2);
                spork ~ playRecording2(letsPlayBack2);
                myPitch2 => loopme2.rate;
                myVolume2 => loopme2.gain;
                myFilter2 => filter2.freq;

                spork ~ recordSound3(letsRecord3);
                spork ~ playRecording3(letsPlayBack3);
                myPitch3 => loopme3.rate;
                myVolume3 => loopme3.gain;
                myFilter3 => filter3.freq;

                spork ~ recordSound4(letsRecord4);
                spork ~ playRecording4(letsPlayBack4);
                myPitch4 => loopme4.rate;
                myVolume4 => loopme4.gain;
                myFilter4 => filter4.freq;

                2::second => now;

            }
        ");

        // create a callback to use with GetFloar in Update()
        myFloatSyncer = gameObject.AddComponent<ChuckFloatSyncer>();

        // start syncing effect parameters
        myFloatSyncer.SyncFloat(myChuck, "myPitch1"); 
        myFloatSyncer.SyncFloat(myChuck, "myVolume1");
        myFloatSyncer.SyncFloat(myChuck, "myFilter1");

        myFloatSyncer.SyncFloat(myChuck, "myPitch2"); 
        myFloatSyncer.SyncFloat(myChuck, "myVolume2");
        myFloatSyncer.SyncFloat(myChuck, "myFilter2");

        myFloatSyncer.SyncFloat(myChuck, "myPitch3"); 
        myFloatSyncer.SyncFloat(myChuck, "myVolume3");
        myFloatSyncer.SyncFloat(myChuck, "myFilter3");

        myFloatSyncer.SyncFloat(myChuck, "myPitch4"); 
        myFloatSyncer.SyncFloat(myChuck, "myVolume4");
        myFloatSyncer.SyncFloat(myChuck, "myFilter4");
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void recordAudio(string button) {
        if (button == buttons[0]) {
            myChuck.BroadcastEvent("letsRecord1");
              
        } else if (button == buttons[1])
        {
            myChuck.BroadcastEvent("letsRecord2");
             
        } else if (button == buttons[2])
        {
            myChuck.BroadcastEvent("letsRecord3");
               
        } else if (button == buttons[3])
        {
            myChuck.BroadcastEvent("letsRecord4");
                
        }
    }

    public void playAudio(string button) {
        if (button == buttons[0]) {
            myChuck.BroadcastEvent("letsPlayBack1");
            Debug.Log("audio playing");
        } else if (button == buttons[1])
        {
            myChuck.BroadcastEvent("letsPlayBack2");
        } else if (button == buttons[2])
        {
            myChuck.BroadcastEvent("letsPlayBack3");
        } else if (button == buttons[3])
        {
            myChuck.BroadcastEvent("letsPlayBack4");
        }
    }

    //figure out how to set the proper effects in the chuck script -> inside playback or global?
     public void setEffect(string button, string slider, float value) {
        if (button == buttons[0]) {
            if (slider == sliders[0]) {
                //set pitch
                myChuck.SetFloat("myPitch1", value);
                     
            } else if (slider == sliders[1])
            {
                //set volume
                myChuck.SetFloat("myVolume1", value);
                      
            } else if (slider == sliders[2])
            {
                //set filter
                myChuck.SetFloat("myFilter1", value);
                      
            }
        } else if (button == buttons[1]) {
            if (slider == sliders[3]) {
                //set pitch
                myChuck.SetFloat("myPitch2", value);
                        
            } else if (slider == sliders[4])
            {
                //set volume
                myChuck.SetFloat("myVolume2", value);
                        
            } else if (slider == sliders[5])
            {
                //set filter
                myChuck.SetFloat("myFilter2", value);
                          
            }
        } else if (button == buttons[2]) {
            if (slider == sliders[6]) {
                //set pitch
                myChuck.SetFloat("myPitch3", value);
                           
            } else if (slider == sliders[7])
            {
                //set volume
                myChuck.SetFloat("myVolume3", value);
                            
            } else if (slider == sliders[8])
            {
                //set filter
                myChuck.SetFloat("myFilter3", value);
                            
            }
        } else if (button == buttons[3]) {    
            if (slider == sliders[9]) {
                //set pitch
                myChuck.SetFloat("myPitch4", value);
                              
            } else if (slider == sliders[10])
            {
                //set volume
                myChuck.SetFloat("myVolume4", value);
                               
            } else if (slider == sliders[11])
            {
                //set filter
                myChuck.SetFloat("myFilter4", value);
                                
            }
        }
    }
    
    private float Remap(float aValue, float aIn1, float aIn2, float aOut1, float aOut2)
    {
        float t = (aValue - aIn1) / (aIn2 - aIn1);
        return aOut1 + (aOut2 - aOut1) * t;
    }
}
