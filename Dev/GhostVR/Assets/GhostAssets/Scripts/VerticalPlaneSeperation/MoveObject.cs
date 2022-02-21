using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.XR;
using UnityEngine.SpatialTracking;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
namespace UnityEngine.XR.Interaction.Toolkit
{
    public class MoveObject : ActionBasedController
    {
        public GameObject CurrentObject;
        public GameObject TargetObject;
        // Start is called before the first frame update
        void Start()
        {

        }

        protected override void UpdateController()
        {
          //  base.UpdateController();
        }

        protected override void UpdateInput(XRControllerState controllerState)
        {
            //base.UpdateInput(controllerState);
        }
        protected override void UpdateTrackingInput(XRControllerState controllerState)
        {
            CurrentObject.transform.position = TargetObject.transform.position;
        }
        protected override void OnEnable()
        {
           //base.OnEnable();
        }
     

    }
}