using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class AvatarRaycastController : MonoBehaviour
{
    private XRRig rig;
    private GameObject head;
    private GameObject MainCam;

    private float CameraVerticalOffset;
    private Vector3 CameraOffset;

    public float ProjectionDistance;
    private Vector3 ProjectionVector;
    private float CollisionDistance;
    private Vector3 CollisionVector;

    // Start is called before the first frame update
    void Start()
    {
        // XR Rig
        rig = GameObject.FindGameObjectWithTag("XRRig").GetComponent<XRRig>();
        //MainCamera
        MainCam = GameObject.Find("Main Camera").gameObject;

        //Disable/enable correct camera scripts
        GameObject.FindGameObjectWithTag("XRRig").GetComponent<CameraController>().enabled = false;

        //Store Camera offset for raycasting
        CameraVerticalOffset = MainCam.transform.position.y;
        CameraOffset = new Vector3(0.0f, -CameraVerticalOffset, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateProjection();
        CalculateCollision();
        CalculatePosition();
        CalculateRotation();
    }

    void CalculateRotation()
    {
        float yRotation = MainCam.transform.rotation.eulerAngles.y;
        this.transform.rotation = Quaternion.Euler(0.0f, yRotation, 0.0f);
    }

    void CalculatePosition()
    {


        //IE a collision has occured
        if (CollisionDistance < ProjectionDistance)
        {
            this.transform.position = rig.transform.position + CollisionVector + CameraOffset;
        }
        else
        {
            this.transform.position = rig.transform.position + ProjectionVector + CameraOffset;
        }

        this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
    }

    void CalculateProjection()
    {
        ProjectionVector = MainCam.transform.rotation * new Vector3(0.0f, 0.0f, ProjectionDistance); //Should place in front of user
    }

    void CalculateCollision()
    {
        //Default value only changes if raycast into environment
        CollisionDistance = ProjectionDistance;
        CollisionVector = new Vector3(0.0f, 0.0f, ProjectionDistance);

        //Define more points for higher precision checking
        Vector3 FootPoint = MainCam.transform.position + CameraOffset + new Vector3(0.0f, 0.1f, 0.0f);
        //GameObject.Find("Footpoint").transform.position = FootPoint; //testing
        Vector3 FootCast = new Vector3(FootPoint.x, 0.1f, FootPoint.z);

        //Remove vertical rotated portion of ray cast so is only 2d
        Vector3 CastForward = new Vector3(MainCam.transform.forward.x, 0.0f, MainCam.transform.forward.z);

        RaycastHit AvatarHit;
        if (Physics.Raycast(FootCast, CastForward, out AvatarHit, ProjectionDistance))
        {
            
            GameObject CollisionObj = AvatarHit.transform.gameObject;
            if (CollisionObj.CompareTag("Environment"))
            {
                //Debug.Log("Ray Hit");
                CollisionVector = AvatarHit.point - MainCam.transform.position - CameraOffset; //May need to be more careful/precision here on collision position to account for avatar dimensions
            }
            else
            {
                //Debug.Log("Wrong tag");
            }
        }
        CollisionDistance = CollisionVector.magnitude;
    }
}
