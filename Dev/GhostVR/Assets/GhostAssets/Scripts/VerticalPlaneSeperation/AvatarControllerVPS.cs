using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class AvatarControllerVPS : CharacterControllerInterface
{
    protected Animator animator;
    public float MovementSpeed;

    private XRRig rig;
    private ActionBasedController leftController, rightController;
    private float oldRotation, newRotation, diffRotation;

    private bool CanRotate;

    public int MovementDimensions;
    // Start is called before the first frame update
    protected override void Start()
    {

        // Set head to camera, targetLR to left and right controller
        rig = GameObject.FindGameObjectWithTag("XRRig").GetComponent<XRRig>();
        head = rig.cameraGameObject.transform;

        // Set targetL to left controller
        leftController = GameObject.Find("LeftHand Controller").GetComponent<ActionBasedController>();
        targetL = leftController.transform;

        // Set targetR to right controller
        rightController = GameObject.Find("RightHand Controller").GetComponent<ActionBasedController>();
        targetR = rightController.transform;

        // The initial rotation of head
        oldRotation = rig.cameraGameObject.transform.eulerAngles.y;

        // Reuse the base class functionality
        base.Start();

        // Set MovementSpeed to normal speed
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        height = head.position.y - 0.5f;
        durtionTime = durtionTime + Time.deltaTime;
        if (durtionTime >= timeStep)
        {
            calculateBodyPose();

            // Speed up/down
            speedAdjust();

            durtionTime = 0;
        }
        // Avatar rotation
        avatarRotation();
    }

    protected override void HandIK(bool isRightHand)
    {
        Transform target = isRightHand ? targetR : targetL;
        AvatarIKGoal g = isRightHand ? AvatarIKGoal.RightHand : AvatarIKGoal.LeftHand;
        AvatarIKHint h = isRightHand ? AvatarIKHint.RightElbow : AvatarIKHint.LeftElbow;
        animator.SetIKPositionWeight(g, 1f);
        animator.SetIKPosition(g, target.localPosition + transform.position);
        animator.SetIKRotationWeight(g, 1f);
        animator.SetIKRotation(g, target.rotation);

        animator.SetIKHintPositionWeight(h, 1f);
        if (isRightHand)
            animator.SetIKHintPosition(h, target.localPosition + transform.position - transform.forward - transform.right);
        else
            animator.SetIKHintPosition(h, target.localPosition + transform.position - transform.forward + transform.right);
    }

    protected override void calculateBodyPose()
    {
        needFootIK = false;
        Vector3 diffPosition = new Vector3(head.localPosition.x, 0, head.localPosition.z) - new Vector3(lastHeadTransform.localPosition.x, 0, lastHeadTransform.localPosition.z);
        if (diffPosition != Vector3.zero)
        {
            // Avatar movement
            if (MovementSpeed <= 0)
                MovementSpeed = 1;
            this.transform.position += (diffPosition * MovementSpeed);

            Vector3 headsetSpeed = (head.localPosition - lastHeadTransform.localPosition) / timeStep;
            headsetSpeed.y = 0;

            Vector3 headsetLocalSpeed = this.transform.InverseTransformDirection(headsetSpeed);
            if (headsetLocalSpeed.sqrMagnitude < 1f)
            {
                speedScale = Mathf.Max(Mathf.Abs(headsetLocalSpeed.z), Mathf.Abs(headsetLocalSpeed.x));
                headsetLocalSpeed = headsetLocalSpeed.normalized;
                needFootIK = true;
            }

            forward = Mathf.Lerp(forward, Mathf.Clamp(headsetLocalSpeed.z, -2, 2), smoothing);
            right = Mathf.Lerp(right, Mathf.Clamp(headsetLocalSpeed.x, -2, 2), smoothing);

            copyTransform(head, lastHeadTransform);
        }
        else
        {
            forward = Mathf.Lerp(forward, 0, smoothing);
            right = Mathf.Lerp(right, 0, smoothing);
        }
    }

    protected override void copyTransform(Transform from, Transform to)
    {
        base.copyTransform(from, to);
        to.localPosition = from.localPosition;
    }

    private void speedAdjust()
    {
        // Speed up when the trigger button of right controller is pressed
        if (rightController.activateAction.action.ReadValue<float>() == 1)
        {
            if (MovementSpeed >= 1)
                MovementSpeed += 0.5f;
            else
                MovementSpeed += 0.1f;
        }

        // Speed down when the trigger button of left controller is pressed
        if (leftController.activateAction.action.ReadValue<float>() == 1)
        {
            if (MovementSpeed > 1)
                MovementSpeed -= 0.5f;
            else if (MovementSpeed > 0.1f)
                MovementSpeed -= 0.1f;
        }
    }
    private void avatarRotation()
    {
        if (CanRotate)
        {
            newRotation = rig.cameraGameObject.transform.eulerAngles.y;
            diffRotation = newRotation - oldRotation;

            // Avatar rotates when user rotates his head
            if (diffRotation != 0)
            {
                transform.rotation = Quaternion.Euler(0, newRotation, 0);
                oldRotation = rig.cameraGameObject.transform.eulerAngles.y;
            }

        }
    }

        public void enableRotation(bool val)
    {
        CanRotate = val;
    }
}
