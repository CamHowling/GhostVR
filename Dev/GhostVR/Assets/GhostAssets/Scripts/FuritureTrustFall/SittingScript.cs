using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SittingScript : MonoBehaviour
{
    public GameObject Avatar;
    public GameObject XR;
    private float initPlayerHeight;
    public float sittingThreshold;
    private float sittingHeight;
    private IEnumerator coroutine;
   
    void Start()
    {
        coroutine = InitializeHeight();
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    void Update()
    {
        sittingHeight = sittingThreshold * initPlayerHeight;
        //Check to see if sitting down
        UnityEngine.XR.InputDevice Head = InputDevices.GetDeviceAtXRNode(XRNode.CenterEye);
        Head.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out Vector3 posH);
        float curHeight = posH.y;
        if (curHeight <= sittingHeight)
        {
            //Sitting
            Avatar.gameObject.GetComponent<CharacterControllerInterface>().isSitting = true;
        }
        else
        {
            //Standing
            Avatar.gameObject.GetComponent<CharacterControllerInterface>().isSitting = false;
        }
        //Debug.Log("Current Height : " + curHeight);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "VRCouch")
        {
            //Enable crouch on ducking
            Avatar.gameObject.GetComponent<CharacterControllerInterface>().onCouch = true;
        }
    }
    private IEnumerator InitializeHeight()
    {
        yield return new WaitForSeconds(0.1f);

        UnityEngine.XR.InputDevice Head = InputDevices.GetDeviceAtXRNode(XRNode.CenterEye);
        Head.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out Vector3 posH);
        sittingHeight = sittingThreshold * posH.y;
        initPlayerHeight = posH.y;
        //Debug.Log("SECOND Starting Height : " + initPlayerHeight);
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "VRCouch")
        {

            //Disable crouch on ducking     
            Avatar.gameObject.GetComponent<CharacterControllerInterface>().onCouch = false;
        }
    }
}