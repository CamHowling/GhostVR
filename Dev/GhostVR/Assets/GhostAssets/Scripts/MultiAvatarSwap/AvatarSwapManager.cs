using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class AvatarSwapManager : MonoBehaviour
{
    public bool avatarSwapIndicator;
    private InputDevice deviceinput;
    List<InputDevice> devices;
    private GameObject maleAvatar, femaleAvatar, maleCollider, femaleCollider;
    // Start is called before the first frame update
    void Start()
    {
        //List of devices, should only have R+L controllers
        devices = new List<InputDevice>();
        //Get R device
        InputDeviceCharacteristics deviceCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(deviceCharacteristics, devices);

        if (devices.Count > 0)
        {
            //if Device found assign it
            deviceinput = devices[0];
        }
        //Get two avatars and its colliders
        maleAvatar = GameObject.Find("Male_1_N_Fixed");
        femaleAvatar = GameObject.Find("Female_1_N_Fixed");
        maleCollider = GameObject.Find("MaleAvatarCollider");
        femaleCollider = GameObject.Find("FemaleAvatarCollider");
        //Set avatarSwapIndicator false as default
        avatarSwapIndicator = false;
        //Set male avatar scripts disabled as default
        AvatarSwap(avatarSwapIndicator);
    }

    // Update is called once per frame
    void Update()
    {
        //Press primary button of left controller or G to swap avatar
        deviceinput.TryGetFeatureValue(CommonUsages.primaryButton, out bool pressed);
        if (pressed == true || Input.GetKeyUp(KeyCode.G))
        {
            avatarSwapIndicator = !avatarSwapIndicator;
            AvatarSwap(avatarSwapIndicator);
        }
    }

    void AvatarSwap(bool indicator)
    {
        // Switch to male avatar
        if(indicator)
        {
            maleAvatar.GetComponent<CharacterControllerMultiAvatarSwap>().enabled = true;
            femaleAvatar.GetComponent<CharacterControllerMultiAvatarSwap>().enabled = false;
            foreach (Transform child in maleCollider.transform)
                child.gameObject.GetComponent<HandColliderA2>().enabled = true;
            foreach (Transform child in femaleCollider.transform)
                child.gameObject.GetComponent<HandColliderA2>().enabled = false;
        }
        // Switch to female avatar
        else
        {
            maleAvatar.GetComponent<CharacterControllerMultiAvatarSwap>().enabled = false;
            femaleAvatar.GetComponent<CharacterControllerMultiAvatarSwap>().enabled = true;
            foreach (Transform child in maleCollider.transform)
                child.gameObject.GetComponent<HandColliderA2>().enabled = false;
            foreach (Transform child in femaleCollider.transform)
                child.gameObject.GetComponent<HandColliderA2>().enabled = true;
        }
    }
}
