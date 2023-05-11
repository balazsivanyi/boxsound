using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallEffectManager : MonoBehaviour
{

    LooperManager looper;
    private Slider slider;
    private string currentSlider;
    private string parentButton;
    private float sliderValue;
    
    // Start is called before the first frame update
    void Start()
    {
        currentSlider = gameObject.name;
        parentButton = transform.parent.gameObject.name;

        looper = GetComponentInParent<LooperManager>();
        slider = GameObject.Find(currentSlider).GetComponent<Slider>();

        //OnSliderValueChanged();
        slider.onValueChanged.AddListener((value) => {
            Debug.Log(currentSlider);
            looper.setEffect(parentButton, currentSlider, value);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public void OnSliderValueChanged()
    {
        sliderValue = slider.value;
        Debug.Log(sliderValue);
        looper.setEffect(parentButton, currentSlider, sliderValue);
    }
}
