using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallEffectManager : MonoBehaviour
{

    LooperManager looper;
    public Slider slider;
    private string currentButton;

    private float sliderValue;
    
    // Start is called before the first frame update
    void Start()
    {
        looper = GetComponentInParent<LooperManager>();
        currentButton = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public void OnSliderValueChanged()
    {
        sliderValue = slider.value;
        looper.setEffect(currentButton, sliderValue);
    }
}
