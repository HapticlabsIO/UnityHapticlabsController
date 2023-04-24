using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour {
    
    public GameObject hands;
    public GameObject slider;

    private bool grabbing = false;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 sliderPosition;

    private int sliderFloor;
    private int sliderFloorOld;

    public float intensity;

    HandTracking handTracking;

    private List<GameObject> collidingObjects = new List<GameObject>();

    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("Collision detected with " + collision.gameObject.name);
        if (!collidingObjects.Contains(collision.gameObject))
        {
            collidingObjects.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collidingObjects.Contains(collision.gameObject))
        {
            collidingObjects.Remove(collision.gameObject);
        }
    }

    void Start(){
        handTracking = hands.GetComponent<HandTracking>();
    }

    void Update() {

        // slider
        if(handTracking.leftGrab){startPosition = handTracking.leftGrabStart; endPosition = handTracking.leftGrabEnd;}
        if(handTracking.rightGrab){startPosition = handTracking.rightGrabStart; endPosition = handTracking.rightGrabEnd;}
        // if((handTracking.leftGrab || handTracking.rightGrab) && startPosition.y > 4.0f){
        if((handTracking.leftGrab || handTracking.rightGrab) && collidingObjects.Count > 0){
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
    }
}
