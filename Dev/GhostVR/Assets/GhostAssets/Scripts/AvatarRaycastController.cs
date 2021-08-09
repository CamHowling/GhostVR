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

    public float ProjectionDistance;
    private Vector3 ProjectionVector;
    private float CollisionDistance;
    private Vector3 CollisionVector;
    //private Vector3 AvatarTargetPosition;


    // Start is called before the first frame update
    void Start()
    {
        // XR Rig
        rig = GameObject.FindGameObjectWithTag("XRRig").GetComponent<XRRig>();
        //MainCamera
        MainCam = GameObject.Find("Main Camera").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateProjection();
        CalculateCollision();



        //IE a collision has occured
        if (CollisionDistance < ProjectionDistance)
        {
            this.transform.position = rig.transform.position + CollisionVector;
        }
        else
        {
            this.transform.position = rig.transform.position + ProjectionVector;
        }
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

        RaycastHit AvatarHit;
        if (Physics.Raycast(MainCam.transform.position, MainCam.transform.forward, out AvatarHit, ProjectionDistance))
        {
            
            GameObject CollisionObj = AvatarHit.transform.gameObject;
            if (CollisionObj.CompareTag("Environment"))
            {
                Debug.Log("Ray Hit");
                CollisionVector = AvatarHit.point - MainCam.transform.position ; //May need to be more careful/precision here on collision position to account for avatar dimensions
            }
            else
            {
                Debug.Log("Wrong tag");
            }
        }
        CollisionDistance = CollisionVector.magnitude;
    }
}
