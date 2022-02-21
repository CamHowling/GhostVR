using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class MainMenuModeCheck : MonoBehaviour
{
    // This script is just to detect if game is played in VR or on Desktop. If VR switch to world view, if desktop switch to overlay
   public GameObject CanvasGameobject;
    private Canvas canvas;
    List<InputDevice> devices;
    enum RenderModeStates { camera, overlay, world };
    RenderModeStates CamRenderModeStates;
    void Start()
    {   
        devices = new List<InputDevice>();
        canvas = CanvasGameobject.GetComponent<Canvas>();
      
    }
    void Update()
    {
        if (XRSettings.isDeviceActive)
        {
            canvas.renderMode = RenderMode.WorldSpace;
         
        }
        else 
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
           
        }
    }
}
