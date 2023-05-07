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

    //calling in loopermanager script
    LooperManager looper;

    private string currentButton;


    // Start is called before the first frame update
    void Start()
    {
        //get exact button component
        body = transform.Find("Body").GetComponent<Image>(); 

        //get looper script from parent
        looper = GetComponentInParent<LooperManager>();

        currentButton = gameObject.name;

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
                    looper.recordAudio(currentButton);
                    if (timer >= 1.0)
                    {
                        timer = timerMax;
                        progressBar.fillAmount = timerMax;
                        progressBar.enabled = false;
                        //do something when progress is done -> send recorded audio to sequencer
                        Debug.Log("event triggered");
                        //play loop back
                        looper.playAudio(currentButton);

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
