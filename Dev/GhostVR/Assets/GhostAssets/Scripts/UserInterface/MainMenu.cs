using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class MainMenu : MonoBehaviour
{
    //Stored as ints in playerpreferences
    //may not need the variables - can clean up later

    public GameObject FPSSwapSwitch;
    public GameObject FPSCamSwitch;
    public GameObject GridSwitch;
    public GameObject XRSwitch;

    public void Start()
    {
        //First run to set initial values - can use this to create generic cases
        FirstPersonSwap();
        FirstPersonCamera();
        MeasurementGrid();
        XRSim();
    }

    public void StartBaseGame()
    {
        SceneManager.LoadScene(1);
    }

    public void StartS1()
    {
        SceneManager.LoadScene(2);
    }

    public void StartS2()
    {
        SceneManager.LoadScene(3);
    }

    public void StartS3()
    {
        SceneManager.LoadScene(4);
    }

    public void StartA1()
    {
        SceneManager.LoadScene(5);
    }

    public void StartA2()
    {
        SceneManager.LoadScene(6);
    }

    public void QuitGame()
    {
        Debug.Log("Quit msg");
        Application.Quit();
    }


    public void FirstPersonSwap()
    {    

        bool val = FPSSwapSwitch.GetComponent<Toggle>().isOn;
        if(!val)
        {
            PlayerPrefs.SetInt("FirstPersonSwap", 0);
        }
        else
        {
            PlayerPrefs.SetInt("FirstPersonSwap", 1);
        }
    }

    public void FirstPersonCamera()
    {
        bool val = FPSCamSwitch.GetComponent<Toggle>().isOn;
        if (!val)
        {
            PlayerPrefs.SetInt("FirstPersonCamera", 0);
        }
        else
        {
            PlayerPrefs.SetInt("FirstPersonCamera", 1);
        }
    }

    public void MeasurementGrid()
    {
        bool val = GridSwitch.GetComponent<Toggle>().isOn;
        if (!val)
        {
            PlayerPrefs.SetInt("MeasurementGrid", 0);
        }
        else
        {
            PlayerPrefs.SetInt("MeasurementGrid", 1);
        }
    }

    public void XRSim()
    {
        bool val = XRSwitch.GetComponent<Toggle>().isOn;
        if (!val)
        {
            PlayerPrefs.SetInt("XRSim", 0);
            Debug.Log("XR sim set off");
        }
        else
        {
            PlayerPrefs.SetInt("XRSim", 1);
            Debug.Log("XR sim set on");
        }
    }
}
