using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class AvatarController : MonoBehaviour
{
    private XRRig rig;
    private GameObject head;
    private Quaternion headRotation_y, headRotation_xz;
    private Vector3 oldPosition, newPosition, diffPosition;

    public int MovementDimensions;
    public float MovementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // XR Rig
        rig = GameObject.FindGameObjectWithTag("XRRig").GetComponent<XRRig>();
        // Avatar head
        head = this.transform.Find("Head").gameObject;

        oldPosition = new Vector3(rig.cameraGameObject.transform.localPosition.x, 0, rig.cameraGameObject.transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        //Allows finer controls over which axis of motion is enabled for translation of character 1 - Z, 2 - X, 3 - Y
        float xPos = rig.cameraGameObject.transform.localPosition.x;
        float yPos = rig.cameraGameObject.transform.localPosition.y;
        float zPos = rig.cameraGameObject.transform.localPosition.z;

        float xVal = 0.0f;
        float yVal = 0.0f;
        float zVal = 0.0f;

        switch (MovementDimensions)
        {
            case 1:
                zVal = zPos;
                break;
            case 2:
                xVal = xPos;
                zVal = zPos;
                break;
            case 3:
                xVal = xPos;
                yVal = yPos;
                zVal = zPos;
                break;
            default:
                break;
        }
            
        // Calculate the headset position difference
        newPosition = new Vector3(xVal, yVal, zVal);
        diffPosition = newPosition - oldPosition;

        // If position has been changed
        if (diffPosition != Vector3.zero)
        {
            // Speed cannot be negative
            if (MovementSpeed <= 0)
                MovementSpeed = 1;
            // Move avatar
            this.transform.position += (diffPosition * MovementSpeed);
            // Update oldPosition
            oldPosition = new Vector3(rig.cameraGameObject.transform.localPosition.x, 0, rig.cameraGameObject.transform.localPosition.z);
        }
    }

    //private void FixedUpdate()
    //{
    //    // Rotate avatar y degrees around the y axis
    //    headRotation_y = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
    //    this.transform.rotation = headRotation_y;
    //    // Rotate avatar head x degrees around the x axis, z degrees around the z axis
    //    headRotation_xz = Quaternion.Euler(rig.cameraGameObject.transform.eulerAngles.x, 0, rig.cameraGameObject.transform.eulerAngles.z);
    //    head.transform.rotation = headRotation_xz;
    //}
}
