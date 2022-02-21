using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class FPSCameraMode : MonoBehaviour
{

    
    public XRRig LocalXRRig;
    public GameObject FPSCameraGameObject;
    public GameObject OtherCamGameObject;
    private Camera FPS;
    private Camera OrignalCam;
    private GameObject AvatarObj;

    void Start()
    {
        OrignalCam = OtherCamGameObject.GetComponent<Camera>();
        FPS = FPSCameraGameObject.GetComponent<Camera>();
 
    }

 
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {
            if (LocalXRRig.cameraGameObject == OtherCamGameObject)
            {
                //Disable FPS screen render texture when in FPS mode or else it wont render.
                FPS.targetTexture = null;

                LocalXRRig.cameraGameObject = FPSCameraGameObject;
                FPS.enabled = true;
                OrignalCam.enabled = false;
            }
            else if (LocalXRRig.cameraGameObject == FPSCameraGameObject)
            {
                {
                 
                    LocalXRRig.cameraGameObject = OtherCamGameObject;
                    FPS.enabled = false;
                    OrignalCam.enabled = true;
                }
            }
            FPSCameraGameObject.transform.rotation = OtherCamGameObject.transform.rotation;
        }
    }
}
