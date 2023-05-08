using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooperManager : MonoBehaviour
{
    ChuckSubInstance myChuck;

    string[] buttons = {"BottomRightButton", "BottomLeftButton", "TopRightButton", "TopLeftButton"};

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
                1 => loopme1.rate;
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
                2::second => now;

                spork ~recordSound1(letsRecord1);
                spork ~ playRecording1(letsPlayBack1);

                spork ~ recordSound2(letsRecord2);
                spork ~ playRecording2(letsPlayBack2);

                spork ~ recordSound3(letsRecord3);
                spork ~ playRecording3(letsPlayBack3);

                spork ~ recordSound4(letsRecord4);
                spork ~ playRecording4(letsPlayBack4);

            }
        ");
        
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
}
