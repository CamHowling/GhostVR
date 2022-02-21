using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRRigController : MonoBehaviour
{
    private XRRig rig;
    private GameObject Avatar, CameraPosition;

    private Quaternion oldRotation, newRotation;
    private float diffRotation;
    private Vector3 oldPosition, newPosition, diffPosition;
    // Start is called before the first frame update
    void Start()
    {
        Avatar = GameObject.Find("Female_1_N_Fixed");
        CameraPosition = GameObject.Find("CameraPosition");

        rig = this.GetComponent<XRRig>();

        oldRotation = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);

        //oldPosition = new Vector3(rig.cameraGameObject.transform.localPosition.x, 0, rig.cameraGameObject.transform.localPosition.z);
        oldPosition = CameraPosition.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        newPosition = CameraPosition.transform.position;
        diffPosition = newPosition - oldPosition;
        if (diffPosition != Vector3.zero)
        {
            this.transform.position = CameraPosition.transform.position - rig.cameraGameObject.transform.localPosition;
            oldPosition = CameraPosition.transform.position;
        }

        newRotation = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        diffRotation = newRotation.eulerAngles.y - oldRotation.eulerAngles.y;
        // If user rotates head
        if (diffRotation != 0)
        {
            // Calculate the new position of xr rig so that the camera rotates around the avatar
            XRRigMove();
        }
    }

    void XRRigMove()
    {
        // Calculate camera new position
        // angle = degree * pi/180
        float angle = (rig.cameraGameObject.transform.eulerAngles.y - 180) * Mathf.PI / 180;
        // radius = Vector3.distance(avatar position, camera xz position)
        float radius = Vector3.Distance(Avatar.transform.position, new Vector3(rig.cameraGameObject.transform.position.x, 0, rig.cameraGameObject.transform.position.z));
        // x = radius *sin(angle)
        float x = radius * Mathf.Sin(angle);
        // z = radius *cos(angle)
        float z = radius * Mathf.Cos(angle);
        // Camera new position
        Vector3 cameraNewPos = new Vector3(x, rig.cameraGameObject.transform.position.y, z) + Avatar.transform.position;
        // Calculate xr rig new position
        Vector3 rigNewPos = cameraNewPos - rig.cameraGameObject.transform.localPosition;
        // Move xr rig to new position
        transform.position = new Vector3(rigNewPos.x, transform.position.y, rigNewPos.z);
        // Update oldRotation
        oldRotation = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
    }
}
