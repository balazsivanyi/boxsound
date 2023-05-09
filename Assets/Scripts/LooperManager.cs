using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooperManager : MonoBehaviour
{
    ChuckSubInstance myChuck;
    ChuckFloatSyncer myFloatSyncer;

    string[] buttons = {"BottomRightButton", "BottomLeftButton", "TopRightButton", "TopLeftButton"};
    string[] sliders = {"Pitch", "Volume", "Filter"};

    // Start is called before the first frame update
    void Start()
    {
        myChuck = GetComponent<ChuckSubInstance>();
        myChuck.RunCode(@"
            //a simple signal path
            adc => LiSa loopme1 => dac;
            adc => LiSa loopme2 => dac;
            adc => LiSa loopme3 => dac;
            adc => LiSa loopme4 => dac;

            //gotta tell LiSa how much memory to allocate
            //alloc memory
            2::second => loopme2.duration;
            2::second => loopme1.duration;
            2::second => loopme3.duration;
            2::second => loopme4.duration;

            10::ms => loopme1.recRamp;
            10::ms => loopme2.recRamp;
            10::ms => loopme3.recRamp;
            10::ms => loopme4.recRamp;

            //global effect params
            global float myPitch;
            global float myVolume;
            global float myFilter;

            
            

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
                //set playback rate
                myPitch => loopme1.rate;
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
                //set playback rate
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
                //set playback rate
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
                //set playback rate
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

                spork ~ recordSound2(letsRecord2);
                spork ~ playRecording2(letsPlayBack2);

                spork ~ recordSound3(letsRecord3);
                spork ~ playRecording3(letsPlayBack3);

                spork ~ recordSound4(letsRecord4);
                spork ~ playRecording4(letsPlayBack4);

                2::second => now;

            }
        ");

        // create a callback to use with GetFloar in Update()
        myFloatSyncer = gameObject.AddComponent<ChuckFloatSyncer>();

        // start syncing effect parameters
        myFloatSyncer.SyncFloat(myChuck, "myPitch"); //delay gain
        myFloatSyncer.SyncFloat(myChuck, "myVolume"); //reverb1 gain
        myFloatSyncer.SyncFloat(myChuck, "myFilter"); //reverb2 gain
        
        
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

    //TODO: add that it knows which big button we'ree talkiing about, and then seeet specific affeects throuugh that (probably other argument in effectmanager for checking the parent button gameobject name)
    //      figure out how to set the proper effects in the chuck script -> inside playback or global?
     public void setEffect(string slider, float value) {
        if (slider == sliders[0]) {
            //set pitch
            myChuck.SetFloat("myPitch", value);
        } else if (slider == sliders[1])
        {
            //set volume
            myChuck.SetFloat("myVolume", value);
        } else if (slider == sliders[2])
        {
            //set filter
            myChuck.SetFloat("myFilter", value);
        }
    }
}
