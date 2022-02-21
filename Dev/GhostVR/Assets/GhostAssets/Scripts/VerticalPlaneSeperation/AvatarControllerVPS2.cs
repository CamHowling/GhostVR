using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class AvatarControllerVPS2 : MonoBehaviour
{
    public float MovementSpeed;

    private XRRig rig;
    private ActionBasedController leftController, rightController;
    private float oldRotation, newRotation, diffRotation;

    public bool rotationUpdateEnabled;
    public int MovementDimensions;
    // Start is called before the first frame update
    protected void Start()
    {
        // Set head to camera, targetLR to left and right controller
        rig = GameObject.FindGameObjectWithTag("XRRig").GetComponent<XRRig>();


        // Set targetL to left controller
        leftController = GameObject.Find("LeftHand Controller").GetComponent<ActionBasedController>();


        // Set targetR to right controller
        rightController = GameObject.Find("RightHand Controller").GetComponent<ActionBasedController>();


        // The initial rotation of head
        oldRotation = rig.cameraGameObject.transform.eulerAngles.y;

        // Reuse the base class functionality
        

        // Set MovementSpeed to normal speed

    }

    // Update is called once per frame
    protected  void Update()
    {
     

        speedAdjust();
        // Avatar rotation
        //avatarRotation();
    }


    private void speedAdjust()
    {
        // Speed up when the trigger button of right controller is pressed
        if (rightController.activateAction.action.ReadValue<float>() == 1)
        {
            if (MovementSpeed >= 1)
                MovementSpeed += 0.5f;
            else
                MovementSpeed += 0.1f;
        }

        // Speed down when the trigger button of left controller is pressed
        if (leftController.activateAction.action.ReadValue<float>() == 1)
        {
            if (MovementSpeed > 1)
                MovementSpeed -= 0.5f;
            else if (MovementSpeed > 0.1f)
                MovementSpeed -= 0.1f;
        }
    }
    private void avatarRotation()
    {
        newRotation = rig.cameraGameObject.transform.eulerAngles.y;
        diffRotation = newRotation - oldRotation;

        // Avatar rotates when user rotates his head
        if (diffRotation != 0)
        {
            transform.rotation = Quaternion.Euler(0, newRotation, 0);
            oldRotation = rig.cameraGameObject.transform.eulerAngles.y;
        }
    }

}
