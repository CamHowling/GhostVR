using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandCollider : MonoBehaviour
{
    private GameObject Avatar;
    private ActionBasedController Hand;
    // Start is called before the first frame update
    void Start()
    {
        // Get male character or female character
        if (this.transform.parent.name == "MaleAvatarCollider")
            Avatar = GameObject.Find("Male_1_N_Fixed");
        else
            Avatar = GameObject.Find("Female_1_N_Fixed");
        // Get left or right hand action based controller
        if (this.transform.name == "LeftHand")
            Hand = GameObject.Find("LeftHand Controller").GetComponent<ActionBasedController>();
        else
            Hand = GameObject.Find("RightHand Controller").GetComponent<ActionBasedController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update hand position
        this.transform.localPosition = Hand.transform.localPosition + Avatar.transform.position;
    }
    private void OnTriggerStay(Collider other)
    {
        // If player pressed grip to grab an interactable object
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable") && Hand.selectAction.action.ReadValue<float>() != 0)
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
