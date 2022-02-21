using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class RigController : MonoBehaviour
{
    private XRRig rig;
    private Vector3 RotationPoint;
    private Quaternion oldRotation, newRotation;
    private float diffRotation;
    private Vector3 oldPosition, newPosition, diffPosition;
    // Start is called before the first frame update
    void Start()
    {
        // Set rotation point
        RotationPoint = Vector3.zero;
        // Get XR rig
        rig = this.GetComponent<XRRig>();
        // Get initial headset rotation and position
        oldRotation = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        oldPosition = new Vector3(rig.cameraGameObject.transform.localPosition.x, 0, rig.cameraGameObject.transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        // Move rig to keep user stationary
        RigMovement();
        // Move rig to cooperate with camera rotation
        RigRotation();
    }

    void RigRotation()
    {
        newRotation = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        diffRotation = newRotation.eulerAngles.y - oldRotation.eulerAngles.y;
        // If user rotates head
        if (diffRotation != 0)
        {
            // Calculate the new position of xr rig to cooperate with camera rotation
            RigNewPos();
        }
    }

    void RigNewPos()
    {
        // Calculate camera new position
        // angle = degree * pi/180
        float angle = (rig.cameraGameObject.transform.eulerAngles.y - 180) * Mathf.PI / 180;
        // radius = Vector3.distance(avatar position, camera xz position)
        float radius = Vector3.Distance(RotationPoint, new Vector3(rig.cameraGameObject.transform.position.x, 0, rig.cameraGameObject.transform.position.z));
        // x = radius *sin(angle)
        float x = radius * Mathf.Sin(angle);
        // z = radius *cos(angle)
        float z = radius * Mathf.Cos(angle);
        // Camera new position
        Vector3 cameraNewPos = new Vector3(x, rig.cameraGameObject.transform.position.y, z) + RotationPoint;
        // Calculate xr rig new position
        Vector3 rigNewPos = cameraNewPos - rig.cameraGameObject.transform.localPosition;
        // Move xr rig to new position
        transform.position = new Vector3(rigNewPos.x, transform.position.y, rigNewPos.z);
        // Update oldRotation
        oldRotation = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
    }

    void RigMovement()
    {
        newPosition = new Vector3(rig.cameraGameObject.transform.localPosition.x, 0, rig.cameraGameObject.transform.localPosition.z);
        diffPosition = newPosition - oldPosition;
        // If user moves head
        if (diffPosition != Vector3.zero)
        {
            // Move xr rig
            this.transform.position -= diffPosition;
            // Update oldPosition
            oldPosition = new Vector3(rig.cameraGameObject.transform.localPosition.x, 0, rig.cameraGameObject.transform.localPosition.z);
        }
    }
}
