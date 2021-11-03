using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueStick : MonoBehaviour
{
    private GameObject cueStick;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Balls" || other.name == "whiteBall")
        {
            // stop the stick when collide with ball
            Rigidbody stickRigidbodyComponent = cueStick.transform.GetComponent<Rigidbody>();
            if (stickRigidbodyComponent)
            {
                stickRigidbodyComponent.velocity = Vector3.zero;
                stickRigidbodyComponent.angularVelocity = Vector3.zero;
            }

            // give an different feedback
            ControllerRight.Instance.HapticFeedbackModel2();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        cueStick = GameObject.Find("cueStick");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
