using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{

    public float pos;
    public float startPosition;
    public float targetPosition;
    private float startTime;

    void Start()
    {
        // targetPosition = startPosition = transform.localPosition;
        
    }

    void Update()
    {
        if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.LeftArrow)) {
            startTime = Time.time;
            startPosition = pos * 15.0f;
            pos = Input.GetKeyDown (KeyCode.RightArrow) ? pos + 1 : pos - 1;
            targetPosition = pos * 15.0f;
        }

        if(targetPosition != startPosition){
            transform.position = new Vector3(Mathf.Lerp(startPosition, targetPosition, 50.0f * (Time.time - startTime) / Mathf.Abs(startPosition - targetPosition)), transform.position.y, transform.position.z);
        }

    }
}
