using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class FPSCamera : MonoBehaviour
{
    // Make this the quad/screen with the render textue
    public GameObject FpsCameraGameObject;

    private ActionBasedController leftController, rightController;
    private InputDevice deviceinput;
    List<InputDevice> devices;
    private bool buttoRelease;
    void Start()
    {
        //List of devices, should only have R+L controllers
        devices = new List<InputDevice>();
        //Get R device
        InputDeviceCharacteristics deviceCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(deviceCharacteristics, devices);

        rightController = GameObject.Find("RightHand Controller").GetComponent<ActionBasedController>();
        leftController = GameObject.Find("LeftHand Controller").GetComponent<ActionBasedController>();
        if(devices.Count > 0)
        {
            //if Device found assign it
            deviceinput = devices[0];
        }
    }

    // Update is called once per frame
    void Update()
    {

        //The bools are used to trigger when buttons release
        deviceinput.TryGetFeatureValue(CommonUsages.secondaryButton,out bool secondarybuttonvalue);
        if (secondarybuttonvalue == true)
        {
            buttoRelease = false;
           
        }
        if (secondarybuttonvalue == false && buttoRelease == false)
        {
            buttoRelease = true;
            FpsCameraGameObject.SetActive(!FpsCameraGameObject.active);
        }


        //Press C to toggle camera for non headset
        if (Input.GetKeyUp(KeyCode.C))
        {
            FpsCameraGameObject.SetActive(!FpsCameraGameObject.active);
        }
    }
}
