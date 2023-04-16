using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerometerData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get accelerometer data
        Vector3 accelerometerData = Input.acceleration;

        // Map accelerometer data to RGB color
        float r = Mathf.Clamp01(accelerometerData.x + 0.5f);
        float g = Mathf.Clamp01(accelerometerData.y + 0.5f);
        float b = Mathf.Clamp01(accelerometerData.z + 0.5f);

        // Set object color
        GetComponent<Renderer>().material.color = new Color(r, g, b);
    }
}
