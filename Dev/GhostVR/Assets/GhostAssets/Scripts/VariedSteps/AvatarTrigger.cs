using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class AvatarTrigger : MonoBehaviour
{
    private GameObject Avatar;
    private ActionBasedController controller;
    // Start is called before the first frame update
    void Start()
    {
        Avatar = GameObject.Find("Female_1_N_Fixed");
        // Get left or right hand action based controller
        if (this.transform.name == "TriggerLeft")
            controller = GameObject.Find("LeftHand Controller").GetComponent<ActionBasedController>();
        else
            controller = GameObject.Find("RightHand Controller").GetComponent<ActionBasedController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update hand position
        this.transform.localPosition = controller.transform.localPosition + Avatar.transform.position;
    }
    private void OnTriggerStay(Collider other)
    {
        // If player pressed grip to grab an interactable object
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable") && controller.selectAction.action.ReadValue<float>() != 0)
        {
            other.transform.parent = this.transform;
            other.transform.position = this.transform.position;
        }
        else
        {
            other.transform.parent = null;
        }
    }
}
