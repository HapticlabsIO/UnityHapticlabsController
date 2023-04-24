using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTracking: MonoBehaviour {
    public UDPReceive udpReceive;
    public GameObject rightHand;
    public GameObject leftHand;
    public GameObject[] rightHandPoints;
    public GameObject[] leftHandPoints;

    public bool useZ = false;

    [Range(0.0f, 2.0f)]
    public float grabThreshold;

    public bool rightGrab;
    public Vector3 rightIndex;
    public Vector3 rightGrabStart;
    public Vector3 rightGrabEnd;
    public bool leftGrab;
    public Vector3 leftIndex;
    public Vector3 leftGrabStart;
    public Vector3 leftGrabEnd;

    private string whichHand;

    void Update() {

        string data = udpReceive.data;

        if (data.Length > 0 && data != "[-1]") {

            string[] points = data.Substring(1, data.Length - 2).Split(", ");

            for (int j = 0; j < points.Length; j += 64) {
                for (int i = 0; i < 21; i++) {
                    float x = 7 - float.Parse(points[i * 3 + 1 + j]) / 100;
                    float y = float.Parse(points[i * 3 + 2 + j]) / 100;
                    float z = useZ ? -float.Parse(points[i * 3 + 3 + j]) / 100 - 1 : 0f;
                    if (points[j] == "'Right'") { rightHandPoints[i].transform.localPosition = new Vector3(x, y, z); whichHand = "Right"; }
                    if (points[j] == "'Left'") { leftHandPoints[i].transform.localPosition = new Vector3(x, y, z); whichHand = "Left"; }
                }
                if (j == 64) { whichHand = "Both"; }
            }
            rightIndex = rightHandPoints[8].transform.position;
            leftIndex = leftHandPoints[8].transform.position;
        } else {
            whichHand = "None";
        }

        if (whichHand == "Left" || whichHand == "Both") { 
            leftHand.SetActive(true); 
            if( Vector3.Distance(leftHandPoints[4].transform.position, leftHandPoints[8].transform.position) < grabThreshold){
                if(leftGrab == false){
                    leftGrabStart = (leftHandPoints[4].transform.position + leftHandPoints[8].transform.position) / 2;
                    leftGrab = true;
                }
                leftGrabEnd = (leftHandPoints[4].transform.position + leftHandPoints[8].transform.position) / 2;
            } else { leftGrab = false; }
        } else { leftHand.SetActive(false); leftGrab = false; }
        if (whichHand == "Right" || whichHand == "Both") { 
            rightHand.SetActive(true); 
            if( Vector3.Distance(rightHandPoints[4].transform.position, rightHandPoints[8].transform.position) < grabThreshold){
                if(rightGrab == false){
                    rightGrabStart = (rightHandPoints[4].transform.position + rightHandPoints[8].transform.position) / 2;
                    rightGrab = true;
                }
                rightGrabEnd = (rightHandPoints[4].transform.position + rightHandPoints[8].transform.position) / 2;
            } else { rightGrab = false; }
        } else { rightHand.SetActive(false); rightGrab = false; }
        if(!rightGrab){rightGrabStart = Vector3.zero; rightGrabEnd = Vector3.zero;}
        if(!leftGrab){leftGrabStart = Vector3.zero; leftGrabEnd = Vector3.zero;}

    }
}



            // for ( int i = 0; i<21; i++ ){
            //     float x = 7-float.Parse(points[i * 3 + 1])/100; // 1 - 61
            //     float y = float.Parse(points[i * 3 + 2]) / 100; // 2 - 62
            //     float z = -float.Parse(points[i * 3 + 3]) / 100 - 1; // 3 - 63

            //     if( points[0] == "'Right'" ){ rightHandPoints[i].transform.localPosition = new Vector3(x, y, z); whichHand = "Right"; }
            //     if( points[0] == "'Left'" ){ leftHandPoints[i].transform.localPosition = new Vector3(x, y, z); whichHand = "Left"; }   
            // }

            // if( points.Length > 64 ){
            //     Debug.Log(points[0]); 
            //     Debug.Log(points[64]); 
            //     for ( int i = 0; i<21; i++ ){
            //         float x = 7-float.Parse(points[i * 3 + 65])/100; // 65 - 125
            //         float y = float.Parse(points[i * 3 + 66]) / 100; // 66 - 126
            //         float z = -float.Parse(points[i * 3 + 67]) / 100 - 1; // 67 - 127

            //         if( points[64] == "'Right'" ){ rightHandPoints[i].transform.localPosition = new Vector3(x, y, z); }
            //         if( points[64] == "'Left'" ){ leftHandPoints[i].transform.localPosition = new Vector3(x, y, z); }   
            //     }
            //     whichHand = "Both";
            // }