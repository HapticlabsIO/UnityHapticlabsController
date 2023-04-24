using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchLine : MonoBehaviour
{
    public GameObject hands;
    public GameObject line;
    HandTracking handTracking;
    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        handTracking = hands.GetComponent<HandTracking>();
        lineRenderer = line.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(handTracking.leftGrab && handTracking.rightGrab){
            // this runs when you're grabbing with both of your hands
            lineRenderer.SetPosition(0, handTracking.leftGrabEnd);
            lineRenderer.SetPosition(1, handTracking.rightGrabEnd);
            float lineLength = Vector3.Distance(handTracking.leftGrabEnd, handTracking.rightGrabEnd);
            Color c = new Color(Mathf.Clamp(lineLength / 10.0f, 0, 1f), Mathf.Clamp(1f - lineLength / 10.0f, 0, 1f), 0, 1);
            lineRenderer.SetColors(c, c);
            lineRenderer.startWidth = lineRenderer.endWidth = Mathf.Clamp(0.5f - lineLength / 20.0f, 0.05f, 0.5f);
            line.SetActive(true);
        } else { line.SetActive(false); }
    }
}
