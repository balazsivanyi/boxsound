using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.iOS;

public class RecordAudio : MonoBehaviour
{
    //timer variables (change later)
    private float timer = 0.0f;
    private float timerMax = 1.0f;
    
    //calling progress bar
    [SerializeField] private Image progressBar = null;
    
    //calling button for check if within bounds 
    private Image body;

    private bool shouldUpdate = false;

    ChuckSubInstance myChuck;

    // Start is called before the first frame update
    void Start()
    {
        //get exact button component
        body = transform.Find("Body").GetComponent<Image>();

        myChuck = GetComponent<ChuckSubInstance>();
  
        myChuck.RunCode(@"
            //a simple signal path
            adc => LiSa loopme => dac;

            //gotta tell LiSa how much memory to allocate
            //alloc memory
            2::second => loopme.duration;
            10::ms => loopme.recRamp;

            global Event letsRecord;
            global Event letsPlayBack;

            fun void recordSound() {
                //start recording input
                loopme.record(1);
                //begin ramping down
                1600::ms => now;
                400::ms => loopme.recRamp;
                //wait for ramp to finish, then stop recording
                400::ms => now;
                loopme.record(0);
            }

            fun void playRecording() {
                //set playback rate
                1 => loopme.rate;
                1 => loopme.loop;
                //1 => loopme.bi;
                1 => loopme.play;
                while(true) {500::ms => now;}
            }
            while (true) {
                
                letsRecord => now;
                recordSound();
                
                letsPlayBack => now;
                playRecording();
            }
        "); 

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            //check if our touch is within the button boundaries
            if (RectTransformUtility.RectangleContainsScreenPoint(body.rectTransform, touch.position))
            {
                if (touch.phase == TouchPhase.Stationary) 
                {
                    Debug.Log("Touch Pressed");
                    shouldUpdate = false;
                    
                    timer += Time.deltaTime;

                    progressBar.enabled = true;
                    progressBar.fillAmount = timer;

                    //start rercording audio
                    myChuck.BroadcastEvent("letsRecord");
                    if (timer >= 1.0)
                    {
                        timer = timerMax;
                        progressBar.fillAmount = timerMax;
                        progressBar.enabled = false;
                        //do something when progress is done -> send recorded audio to sequencer
                        Debug.Log("event triggered");
                        myChuck.BroadcastEvent("letsPlayBack");

                    }
                    //else just delete reecorded audio
                }
            }
        }
        else
        {
            if (shouldUpdate)
            {
                timer -= Time.deltaTime;
                progressBar.fillAmount = timer;

                if (timer <= 0.0)
                {
                    timer = 0;
                    progressBar.fillAmount = timerMax;
                    progressBar.enabled = false;
                    shouldUpdate = false;
                }
            }
        }
        
        if (Input.touchCount == 0)
        {
            Debug.Log("Touch Lifted/Released");
            shouldUpdate = true;
        }
    }
}
