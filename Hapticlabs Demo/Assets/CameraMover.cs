using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{

    private float pos;
    // private float startPosition;
    private float targetPosition;
    // private float startTime;

    void Start()
    {
        // targetPosition = startPosition = transform.localPosition;
        
    }

    void Update()
    {
        if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.LeftArrow)) {
            // startTime = Time.time;
            // startPosition = pos * 90.0f;
            pos = Input.GetKeyDown (KeyCode.RightArrow) ? pos - 1 : pos + 1;
            targetPosition = pos * 90.0f;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, targetPosition, 0),  Time.deltaTime * 5.0f);

        // if(targetPosition != startPosition){
            // transform.position = new Vector3(Mathf.Lerp(startPosition, targetPosition, 50.0f * (Time.time - startTime) / Mathf.Abs(startPosition - targetPosition)), transform.position.y, transform.position.z);
        // }

    }
}
