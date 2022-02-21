using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class CameraController2 : MonoBehaviour
{


    private XRRig rig;
    private GameObject Avatar;
    private GameObject head;
    private GameObject Camera;

    private Vector3 CameraHeight;
    private Vector3 oldPosition;
    public float Height;
    private Quaternion headRotation;
    public float CameraDistance;
    private Vector3 TestVector;
    public int MovementDimensions;

    // Start is called before the first frame update
    void Start()
    {

        rig = GameObject.FindGameObjectWithTag("XRRig").GetComponent<XRRig>();
        //Store location of avatar and camera
        Avatar = GameObject.Find("Avatar");
        head = GameObject.Find("Head");
        Camera = GameObject.Find("Camera Offset");
     
       
        CameraHeight = new Vector3(0.0f, Height, 0.0f);


        oldPosition = new Vector3(rig.cameraGameObject.transform.localPosition.x, 0, rig.cameraGameObject.transform.localPosition.z);
     
        CalcPos();

    }

    // Update is called once per frame
    void Update()
    {
        oldPosition = new Vector3(rig.cameraGameObject.transform.localPosition.x, 0, rig.cameraGameObject.transform.localPosition.z);
        TestVector = rig.transform.forward;
        
        
  


        Debug.Log("RIgVect:" + oldPosition);


        
        CalcPos();
    }

    void CalcPos()
    {
        //Uses VR headset to get normal and rotation, doesnt work with device
        UnityEngine.XR.InputDevice Head = InputDevices.GetDeviceAtXRNode(XRNode.CenterEye);
        Head.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out Vector3 posH);
        Head.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out Quaternion rotH);
        Vector3 DisplacementVector = (oldPosition);
        
     
        this.transform.RotateAround(Avatar.transform.position, new Vector3(0, 1, 0), (headRotation.eulerAngles.y - rotH.eulerAngles.y)*-1);
        headRotation = rotH;
    
    }

}
