using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOptions : MonoBehaviour
{
    int FirstPersonSwapEnabled;
    int FirstPersonCameraEnabled;
    int MeasurementGridEnabled;
    int XRSimEnabled;

    public GameObject XRSimObj, GridContainer, GridContainer2, FPSCam, FPSScreen, FPSCam2, FPSScreen2;

    //public int RefreshRate;

    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = RefreshRate;

        FirstPersonSwapEnabled = PlayerPrefs.GetInt("FirstPersonSwap");
        FirstPersonCameraEnabled = PlayerPrefs.GetInt("FirstPersonCamera");
        MeasurementGridEnabled = PlayerPrefs.GetInt("MeasurementGrid");
        XRSimEnabled = PlayerPrefs.GetInt("XRSim");

        if (FirstPersonSwapEnabled > 0) //True
        {
            this.gameObject.GetComponent<FPSCameraMode>().enabled = true; //Enable any scene elements that are required, cameras etc
        }
        else
        {
            this.gameObject.GetComponent<FPSCameraMode>().enabled = false;
        }

        if (FirstPersonCameraEnabled > 0) //True
        {
            FPSCam.SetActive(true);
            FPSScreen.SetActive(true);
            if (FPSCam2 != null)
            {
                FPSCam2.SetActive(true);
                FPSScreen2.SetActive(true);
            }
        }
        else
        {
            FPSCam.SetActive(false);
            FPSScreen.SetActive(false);
            if (FPSCam2 != null)
            {
                FPSCam2.SetActive(false);
                FPSScreen2.SetActive(false);
            }
        }

        //Currently working correctly
        if (XRSimEnabled > 0) //True
        {
            XRSimObj.SetActive(true);
            Debug.Log("XR sim loaded on");
        }
        else
        {
            XRSimObj.SetActive(false);
            Debug.Log("XR sim loaded off");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Z) && (MeasurementGridEnabled > 0))
        {
            bool currGridStatus = GridContainer.activeSelf;
            if (!currGridStatus)
            {
                GridContainer.SetActive(true);
                if (GridContainer2 != null)
                {
                    GridContainer2.SetActive(true);
                }
            }
            else
            {
                GridContainer.SetActive(false);
                if (GridContainer2 != null)
                {
                    GridContainer2.SetActive(false);
                }
            }
        }
    }
}
