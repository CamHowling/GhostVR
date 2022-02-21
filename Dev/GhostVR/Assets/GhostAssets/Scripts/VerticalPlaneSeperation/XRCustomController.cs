using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRCustomController : MonoBehaviour
{
    private ActionBasedController controller;
    // Start is called before the first frame update
    void Start()
    {
        // Get left or right hand action based controller
        if (this.transform.parent.name == "LeftHand")
            controller = GameObject.Find("LeftHand Controller").GetComponent<ActionBasedController>();
        else
            controller = GameObject.Find("RightHand Controller").GetComponent<ActionBasedController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update hand rotation and position
        this.transform.parent.rotation = controller.transform.rotation;
        this.transform.parent.localPosition = controller.transform.localPosition;
    }

}
