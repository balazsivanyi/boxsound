using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSound : MonoBehaviour
{
    ChuckSubInstance myChuck;
    public bool soundPaused = true; 
    public AudioSource audioSource;

    
    void Start()
    {
        myChuck = GetComponent<ChuckSubInstance>();
        myChuck.RunCode(@"
            1::second => dur beat;

            SinOsc osc => ADSR env1 => NRev rev1 => dac;
            (beat / 2, beat / 2, 0, 1::ms) => env1.set;
            0.1 => osc.gain;

            0.1 => rev1.mix;

            SinOsc osc2 => ADSR env2 => NRev rev2 => dac;
            env2 => Delay delay2 => dac;
            delay2 => delay2;

            (4::ms, beat / 8, 0, 1::ms) => env2.set;
            0.2 => osc2.gain;

            beat => delay2.max;
            beat /8 => delay2.delay;
            0.5 => delay2.gain;

            0.2 => rev2.mix;


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
                        
            while(true) {
                playSound => now;
                // start shred with melody loop
                playMelody();                  
            }
            ");
    }

    void Update()
    {  

        
    }


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
        
}
