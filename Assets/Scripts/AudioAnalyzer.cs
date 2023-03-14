using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioAnalyzer : MonoBehaviour
{
    ChuckSubInstance myChuck;
	ChuckFloatSyncer myGetFreqSyncer;
    private Image SafeArea;
    
    public float mappedFreq;

    // Start is called before the first frame update
    void Start()
    {
        myChuck = GetComponent<ChuckSubInstance>();
		myChuck.RunCode( @"
			dac => FFT fft =^ Centroid cent => blackhole;
			
            global float dacFreq;
			1024 => fft.size;
			Windowing.hann( fft.size() ) => fft.window;
            second / samp => float srate;

			while( true )
			{
				// upchuck: take fft
				cent.upchuck();

				// store value in global variable
				cent.fval(0) * srate / 2 => dacFreq;

				// advance time
				fft.size()::samp => now;
			}	
		" );

		myGetFreqSyncer = gameObject.AddComponent<ChuckFloatSyncer>();
        myGetFreqSyncer.SyncFloat(myChuck, "dacFreq");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log( "most recent frequency value: " + 
			myGetFreqSyncer.GetCurrentValue().ToString( "0.000" )
		);
        FrequencyVisualizer();
    }

    public void FrequencyVisualizer() {
        SafeArea = GameObject.Find("SafeArea").GetComponent<Image>();
        mappedFreq = Mathf.InverseLerp(300, 4000, myGetFreqSyncer.GetCurrentValue());
        //GameObject.Find("Button").GetComponent<MakeSound>().soundPaused;
        
        if (GameObject.Find("Button").GetComponent<MakeSound>().soundPaused == false)
        {
            SafeArea.color = new Color(mappedFreq, (float)(mappedFreq + 0.2), (float)(mappedFreq - 0.2));
        } else
        {
            SafeArea.color = Color.white;
        }
    }
}
