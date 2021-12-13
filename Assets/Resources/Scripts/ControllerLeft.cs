using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerLeft : MonoBehaviour
{
    List<UnityEngine.XR.InputDevice> leftHandDevices; // leftHandDevices
    private GameObject cueStick, leftHand, rightHand, cueStickTip;
    public bool isGrabbed;
    public bool isReleased;
    public static ControllerLeft Instance;

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
        cueStickTip = GameObject.Find("cueStickTip");
        leftHand = GameObject.FindGameObjectWithTag("leftHand");
        rightHand = GameObject.FindGameObjectWithTag("rightHand");

        leftHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
        if (leftHandDevices.Count == 1)
        {
            UnityEngine.XR.InputDevice device = leftHandDevices[0];
            Debug.Log(string.Format("Device name '{0}' with role '{1}'", device.name, device.role.ToString()));
        }
        else if (leftHandDevices.Count > 1)
        {
            Debug.Log("Found more than one left hand!");
        }
    }

    // Haptice Feedback for the touch(can be grab), grab and release.
    public void HapticFeedbackMode1()
    {
        foreach (var device in leftHandDevices)
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
        foreach (var device in leftHandDevices)
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
    public void GrabAdjustRelease()
    {
        UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.LeftHanded, leftHandDevices);
        float triggerValue, gripValue;
        
        foreach (var device in leftHandDevices)
        {

            // lefthand moving has been grabbed
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out gripValue) && gripValue > 0.7f)
            {
                if(!isGrabbed)
                {
                    HapticFeedbackMode1();
                    isGrabbed = true;
                    isReleased = false;
                }

                cueStick.transform.SetParent(leftHand.transform);
            }

            // lefthand moving has been released
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

            // lefthand rorating has been grabbed
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out triggerValue) && triggerValue > 0.7f)
            {
                if (!isGrabbed)
                {
                    HapticFeedbackMode1();
                    isGrabbed = true;
                    isReleased = false;
                }

                Vector3 offset = cueStick.transform.position - cueStick.transform.Find("cueStickTip").transform.position;
                cueStick.transform.position = leftHand.transform.position + offset;
                cueStick.transform.LookAt(rightHand.transform.position + offset);
            }

            // lefthand rorating has been released
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
            if (other.transform.rotation.x == 5.0f)
            {
                other.transform.rotation = Quaternion.Euler(-5.0f, 0.0f, 0.0f);
            }
            else
            {
                other.transform.rotation = Quaternion.Euler(5.0f, 0.0f, 0.0f);
            }
        }
    }

    // Anyway when leftcontroller exit, the stick must be released
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "cueStick")
        {
            cueStick.transform.parent = null;
            isReleased = true;
            isGrabbed = false;
        }
        // Debug.Log("stick has been released by left hand");
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
            GrabAdjustRelease();
        }

        if (other.transform.name == "Slider")
        {
            
            UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.LeftHanded, leftHandDevices);
            float triggerValue;
            foreach (var device in leftHandDevices)
            {

                // 0.0216 - 0.2004
                if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out triggerValue) && triggerValue > 0.7f)
                {
                    Debug.Log("Slider1");
                    if (this.transform.position.x >= 0.0216 && this.transform.position.x <= 0.2004)
                    {
                        Debug.Log("Slider2");
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
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
    }
}
