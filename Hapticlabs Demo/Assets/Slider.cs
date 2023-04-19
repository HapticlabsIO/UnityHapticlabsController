using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour {
    
    public GameObject slider;
    public GameObject button;
    public GameObject line;

    private bool grabbing = false;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 sliderPosition;

    private int sliderFloor;
    private int sliderFloorOld;

    public float intensity;

    HandTracking handTracking;
    LineRenderer lineRenderer;

    void Start(){
        handTracking = gameObject.GetComponent<HandTracking>();
        lineRenderer = line.GetComponent<LineRenderer>();
    }

    void Update() {

        // slider
        if(handTracking.leftGrab){startPosition = handTracking.leftGrabStart; endPosition = handTracking.leftGrabEnd;}
        if(handTracking.rightGrab){startPosition = handTracking.rightGrabStart; endPosition = handTracking.rightGrabEnd;}
        if((handTracking.leftGrab || handTracking.rightGrab) && startPosition.y > 4.0f){
            slider.transform.localScale = new Vector3(0.8f,0.4f,0.8f);
            if(!grabbing){ 
                // this runs once when you grab the slider
                sliderPosition = slider.transform.localPosition; 
                grabbing = true; 
                Serial.Write(";a(\"startTrack(\"1\")\")b(\"startTrack(\"1\")\");");
            }
            slider.transform.localPosition = new Vector3(Mathf.Clamp(sliderPosition.x + endPosition.x - startPosition.x, -5, 5), 0, 0);
            sliderFloor = Mathf.FloorToInt(Mathf.Floor(slider.transform.localPosition.x));
            if(sliderFloor != sliderFloorOld){
                // this runs every time the slider position changes a whole integer (the whole slider is -5 to 5)
                Debug.Log(sliderFloor);
                intensity = ((float)sliderFloor + 5.0f)/10.0f;
                Serial.Write(";a(\"v( " + intensity + " 120 150000)\";");
                // Serial.Write(";a(\"startTrack(\"1\")\")b(\"startTrack(\"1\")\");");
                Debug.Log(";a(\"v( " + intensity + " 120 150000)\";");
                sliderFloorOld = sliderFloor;
            }
        } else{
            slider.transform.localScale = new Vector3(0.4f,0.2f,0.4f);
            grabbing = false;
        }

        // line
        if(handTracking.leftGrab && handTracking.rightGrab){
            // this runs when you're grabbing with both of your hands
            lineRenderer.SetPosition(0, handTracking.leftGrabEnd);
            lineRenderer.SetPosition(1, handTracking.rightGrabEnd);
            float lineLength = Vector3.Distance(handTracking.leftGrabEnd, handTracking.rightGrabEnd);
            lineRenderer.startWidth = lineRenderer.endWidth = Mathf.Clamp(0.5f - lineLength / 20.0f, 0.05f, 0.5f);
            line.SetActive(true);
        } else { line.SetActive(false); }

        // button
        if(handTracking.rightIndex.x > 4.7f || handTracking.leftIndex.x > 4.7f){
            // this runs when your left or right index pushes the button
            button.transform.localPosition = new Vector3(0, 0.05f, 0);
            Serial.Write(";a(\"startTrack(\"1\")\")b(\"startTrack(\"1\")\");");
        } else{
            button.transform.localPosition = new Vector3(0, 0.2f, 0);
        }
    }
}
