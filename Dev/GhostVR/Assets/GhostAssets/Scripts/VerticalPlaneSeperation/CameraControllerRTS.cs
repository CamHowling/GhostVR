using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerRTS : MonoBehaviour
{
    public GameObject Avatar;
    private Vector3 AvatarPos;
    private Vector3 CameraPos;
    private Vector3 CameraHeight;
    private Quaternion CameraRot;

    public float Height;
    public float CameraDistance;

    // Start is called before the first frame update
    void Start()
    {
        //Store location of avatar and camera
    
        AvatarPos = Avatar.transform.position;
        CameraPos = this.transform.position;
        CameraHeight = new Vector3(0.0f, Height, 0.0f);

        CalcRot();
        CalcPos();

    }

    // Update is called once per frame
    void Update()
    {
        //Calculate rotation vector between Camera and Avatar
        //Calculate position as posVal x unit vector of distance vector
        AvatarPos = Avatar.transform.position;
        CameraPos = this.transform.position;
        CalcRot();
        CalcPos();
    }

    void CalcPos()
    {

        Vector3 Distance = CameraPos - AvatarPos;
        Vector3 UnitDisp = Distance.normalized;

        Vector3 FV = new Vector3(UnitDisp.x, Height, UnitDisp.z - CameraDistance) + AvatarPos + CameraHeight;
        // this.transform.position = (UnitDisp * CameraDistance) + AvatarPos + CameraHeight;
        this.transform.position = FV;

    }

    void CalcRot()
    {
        Quaternion DeltaRot = Quaternion.FromToRotation(CameraPos, AvatarPos);
        CameraRot = CameraRot * DeltaRot;
    }
}