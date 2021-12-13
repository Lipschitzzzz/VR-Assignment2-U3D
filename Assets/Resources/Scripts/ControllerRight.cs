using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRight : MonoBehaviour
{
    List<UnityEngine.XR.InputDevice> rightHandDevices; // inputDevices
    private GameObject cueStick, leftHand, rightHand;
    public bool isGrabbed;
    public bool isReleased;
    public static ControllerRight Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        isGrabbed = false;
        isReleased = true;

        // Initialization
        cueStick = GameObject.Find("cueStick");
        rightHand = GameObject.FindGameObjectWithTag("rightHand");
        leftHand = GameObject.FindGameObjectWithTag("leftHand");
        rightHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.RightHanded, rightHandDevices);


        // bind the handUI to right hand
        GameObject HandUI = GameObject.Find("HandUI");
        HandUI.transform.SetParent(Instance.transform);
        HandUI.transform.localPosition = new Vector3(0.08f, 0.015f, -0.1f);
        HandUI.transform.localEulerAngles = new Vector3(12.0f, -90.0f, -90.0f);
        HandUI.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);


    }

    // Haptice Feedback for the touch(can be grab), grab and release.
    public void HapticFeedbackMode1()
    {
        foreach (var device in rightHandDevices)
        {
            UnityEngine.XR.HapticCapabilities capabilities;
            if (device.TryGetHapticCapabilities(out capabilities))
            {
                if (capabilities.supportsImpulse)
                {
                    device.SendHapticImpulse(0, 0.2f, 0.1f);
                }
            }
        }
    }

    // Haptice Feedback for hitting the ball
    public void HapticFeedbackModel2()
    {
        foreach (var device in rightHandDevices)
        {
            UnityEngine.XR.HapticCapabilities capabilities;
            if (device.TryGetHapticCapabilities(out capabilities))
            {
                if (capabilities.supportsImpulse)
                {
                    device.SendHapticImpulse(0, 0.4f, 0.4f);
                }
            }
        }
    }


    // make the controller pulse when touching the cue, if this is not yet being grabbed with the respective controller
    public void CanBeGrabbed()
    {
        HapticFeedbackMode1();
    }

    // implement a single, noticeable pulse when the grabbing or releasing action happens,
    public void GrabSlideRelease()
    {
        UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.RightHanded, rightHandDevices);
        float triggerValue, gripValue;
        
        foreach (var device in rightHandDevices)
        {

            // righthand moving has been grabbed
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out gripValue) && gripValue > 0.7f)
            {
                if (!isGrabbed)
                {
                    HapticFeedbackMode1();
                    isGrabbed = true;
                    isReleased = false;
                }

                cueStick.transform.SetParent(rightHand.transform);
            }

            // righthand moving has been released
            else if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out gripValue) && gripValue < 0.7f)
            {
                if (!isReleased)
                {
                    HapticFeedbackMode1();
                    isReleased = true;
                    isGrabbed = false;
                }

                cueStick.transform.parent = null;
            }

            // righthand sliding has been grabbed
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out triggerValue) && triggerValue > 0.7f)
            {
                if (!isGrabbed)
                {
                    HapticFeedbackMode1();
                    isGrabbed = true;
                    isReleased = false;
                }
                cueStick.transform.GetComponent<Rigidbody>().AddForce(cueStick.transform.up * 60.0f);
            }

            // righthand sliding has been released
            else if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out triggerValue) && triggerValue < 0.7f && triggerValue > 0.0f)
            {
                if (!isReleased)
                {
                    HapticFeedbackMode1();
                    isReleased = true;
                    isGrabbed = false;
                }

                cueStick.transform.parent = null;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "cueStick")
        {
            CanBeGrabbed();
        }
        if (other.transform.name == "Button")
        {
            if (other.transform.localEulerAngles.x >= 355.0f)
            {
                other.transform.localEulerAngles = new Vector3(5.0f, 0.0f, 0.0f);
                AudioSource audioData = other.GetComponent<AudioSource>();
                audioData.Play(0);
            }
            else
            {
                other.transform.localEulerAngles = new Vector3(-5.0f, 0.0f, 0.0f);
                Debug.Log(other.transform.localEulerAngles.x);
                AudioSource audioData = other.GetComponent<AudioSource>();
                audioData.Play(0);
            }
        }


        


    }

    // Anyway when rightcontroller exit, the stick must be released
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "cueStick")
        {
            cueStick.transform.parent = null;
            isReleased = true;
            isGrabbed = false;
        }
        // Debug.Log("stick has been released by right hand");
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.transform.name == "cueStick")
        {
            if (other.attachedRigidbody)
            {
                other.attachedRigidbody.useGravity = false;
                other.attachedRigidbody.velocity = Vector3.zero;
                other.attachedRigidbody.angularVelocity = Vector3.zero;
            }
            GrabSlideRelease();
        }

        if (other.transform.name == "Slider")
        {
            Debug.Log("Slider");
            UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.RightHanded, rightHandDevices);
            float triggerValue;
            foreach (var device in rightHandDevices)
            {

                // 0.0216 - 0.2004
                if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out triggerValue) && triggerValue > 0.7f)
                {
                    if (this.transform.position.x >= 0.0216 && this.transform.position.x <= 0.2004)
                    {
                        other.transform.position = new Vector3(this.transform.position.x, 0.4416f, -0.5481f);
                    }
                }
            }
        }


    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

    void Update()
    {
        // sometimes controller will lost
        UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.RightHanded, rightHandDevices);
    }
}
