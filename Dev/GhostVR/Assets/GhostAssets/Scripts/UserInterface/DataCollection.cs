using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class DataCollection : MonoBehaviour
{
    public GameObject XR;
    public GameObject MainCam;
    public GameObject FPSCamScreen;
    public GameObject MeasurementGrid;

    string fname;
    string path;
    StreamWriter file;

    Vector3 posHead;
    Vector3 rotHead;
    Vector3 posRCon;
    Vector3 rotRCon;
    Vector3 posLCon;
    Vector3 rotLCon;

    // Start is called before the first frame update
    void Start()
    {
        string SceneID = SceneManager.GetActiveScene().name.Substring(0, 2);
        fname = "GhostVR_" + SceneID + System.DateTime.Now.ToString(" dd-MMMM HH.mm tt") + ".csv";
        path = Path.Combine(Application.persistentDataPath, fname);
        Debug.Log(path.ToString());
        file = new StreamWriter(path);
        file.WriteLine(WriteLegend());
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.XR.InputDevice Head = InputDevices.GetDeviceAtXRNode(XRNode.CenterEye);
        Head.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out Vector3 posHead);
        Head.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out Vector3 rotHead);

        UnityEngine.XR.InputDevice RCon = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        RCon.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out Vector3 posRCon);
        RCon.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out Vector3 rotRCon);

        UnityEngine.XR.InputDevice LCon = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        LCon.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out Vector3 posLCon);
        LCon.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out Vector3 rotLCon);

        string UpdateData = CollectData();
        file.WriteLine(UpdateData);
    }

    string CollectData()
    {
        string Val;
        string Br = ", ";

        //Head Position, Head Rotation, Controller Positions, Controller Rotations
        string timeStamp = CalcTime();
        string HeadPos = CalcHeadPos();
        string HeadRot = CalcHeadRot();
        string RControllerPos = CalcRConPos();
        string RControllerRot = CalcRConRot();
        string LControllerPos = CalcLConPos();
        string LControllerRot = CalcLConRot();
        string ToolsUsed = CalcTools();

        Val = timeStamp + HeadPos + HeadRot + RControllerPos + RControllerRot + LControllerPos + LControllerRot + ToolsUsed;

        return Val;
    }

    string CalcTime()
    {
        string Val = "";
        string Br = ", ";

        System.DateTime theTime = System.DateTime.Now;
        string time = theTime.ToString("HH:mm:ss:fff");

        Val = time + Br;
        return Val;
    }

    string CalcHeadPos()
    {
        string Val = "";
        string Br = ", ";
        Val = posHead.x.ToString("F4") + Br + posHead.y.ToString("F4") + Br + posHead.z.ToString("F4") + Br;
        return Val;
    }

    string CalcHeadRot()
    {
        string Val = "";
        string Br = ", ";
        Val = rotHead.x.ToString("F4") + Br + rotHead.y.ToString("F4") + Br + rotHead.z.ToString("F4") + Br;
        return Val;
    }

    string CalcRConPos()
    {
        string Val = "";
        string Br = ", ";
        Val = posRCon.x.ToString("F4") + Br + posRCon.y.ToString("F4") + Br + posRCon.z.ToString("F4") + Br;
        return Val;
    }

    string CalcRConRot()
    {
        string Val = "";
        string Br = ", ";
        Val = rotRCon.x.ToString("F4") + Br + rotRCon.y.ToString("F4") + Br + rotRCon.z.ToString("F4") + Br;
        return Val;
    }


    string CalcLConPos()
    {
        string Val = "";
        string Br = ", ";
        Val = posLCon.x.ToString("F4") + Br + posLCon.y.ToString("F4") + Br + posLCon.z.ToString("F4") + Br;
        return Val;
    }

    string CalcLConRot()
    {
        string Val = "";
        string Br = ", ";
        Val = rotLCon.x.ToString("F4") + Br + rotLCon.y.ToString("F4") + Br + rotLCon.z.ToString("F4") + Br;
        return Val;
    }

       

    string CalcTools()
    {
        string Val = "";
        string Br = ", ";

        string FPSCamString = "Off";
        string SmallCamString = "Off";
        string GridString = "Off";

        //Add use of first person Camera - check if main is disabled
        if (!MainCam.GetComponent<Camera>().enabled)
        {
            FPSCamString = "On";
        }
        //Add use of corner camera - check if both main and corner are enabled
        if (MainCam.activeSelf && FPSCamScreen.activeSelf)
        {
            SmallCamString = "On";
        }
        //Add use of measurement grid - check if is enabled
        if (MeasurementGrid.activeSelf)
        {
            GridString = "On";
        }

        Val = FPSCamString + Br + SmallCamString + Br + GridString;
        return Val;
    }

    string WriteLegend()
    {
        string Val = "";
        string Br = ", ";
        Val = "Time Stamp" + Br;
        Val = Val + "HeadPosX: " + Br + "HeadPosY: " + Br + "HeadPosZ: " + Br;
        Val = Val + "HeadRotX: " + Br + "HeadRotY: " + Br + "HeadRotZ: " + Br;
        Val = Val + "RConPosX: " + Br + "RConPosY: " + Br + "RConPosZ: " + Br;
        Val = Val + "RConRotX: " + Br + "RConRotY: " + Br + "RConRotZ: " + Br;
        Val = Val + "LConPosX: " + Br + "LConPosY: " + Br + "LConPosZ: " + Br;
        Val = Val + "LConRotX: " + Br + "LConRotY: " + Br + "LConRotZ: " + Br;
        Val = Val + "FPCamOn: " + Br + "SmallCamOn: " + Br + "GridOn: " + Br;
        return Val;
    }

    void OnApplicationQuit()
    {
        file.Close();
    }
}
