using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : MonoBehaviour
{

    public GameObject button;
    private bool colliding;
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

    private void Update()
    {
        if (collidingObjects.Count > 0)
        {
            if(colliding == false){
                button.transform.localPosition = new Vector3(0, 0.05f, 0);
                Debug.Log("On");
                colliding = true;
                // Serial.Write(";a(\"startTrack(\"1\")\")b(\"startTrack(\"1\")\");");
            }
        }
        else if(colliding == true){
            button.transform.localPosition = new Vector3(0, 0.2f, 0);
            Debug.Log("Off");
            colliding = false;
        }

    }
}
