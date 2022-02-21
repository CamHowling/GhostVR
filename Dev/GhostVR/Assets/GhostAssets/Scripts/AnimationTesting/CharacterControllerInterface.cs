using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerInterface : MonoBehaviour
{
    protected Animator animator;
    // Hand Ik
    public Transform targetL;
    public Transform hintL;
    public Transform targetR;
    public Transform hintR;
    // Head IK
    public Transform head;

    public float floor;
    public float speedTreshold = 1f;
    public float smoothing = 0.4f;
    // Foot IK
    public float speedScale = 1f;
    public Vector3 footIKDirection;

    //sitting
    public bool isSitting = false;
    public bool onCouch = false;

    //Foot lock
    private bool isLockFootL = false;
    private bool needUpdateFootL = false;
    private Transform lockFootLPosition;
    private float smoothingtimeL = 0;
    private bool isLockFootR = false;
    private bool needUpdateFootR = false;
    private Transform lockFootRPosition;
    private float smoothingtimeR = 0;
    public float smoothingFootTime = 0.2f;

    //Debug testing
    private bool testIsInSmoothing = false;

    private Vector3 testLockLocation;
    private Vector3 testLookTarget;

    public float animationSpeed = 1.0f;
    protected bool needFootIK = false;



    protected Transform lastHeadTransform;
    private Vector3 previousPos;

    protected float timeStep = 0.1f;
    protected float durtionTime = 0;
    protected float height = 0;
    protected float forward = 0;
    protected float right = 0;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();

        GameObject emptyGO = new GameObject();
        lastHeadTransform = emptyGO.transform;
        if (head != null)
        {
            copyTransform(head, lastHeadTransform);
            previousPos = head.position;
        }
        GameObject emptyGO2 = new GameObject();
        lockFootLPosition = emptyGO2.transform;
        GameObject emptyGO3 = new GameObject();
        lockFootRPosition = emptyGO3.transform;
        footIKDirection = new Vector3(1, 1, 1);
        smoothingtimeR = 1.0f;
        smoothingtimeL = 1.0f;

    }

    // Update is called once per frame
    protected virtual void Update()
    {

        height = head.position.y - 0.5f;
        durtionTime = durtionTime + Time.deltaTime;
        if (durtionTime >= timeStep)
        {
            calculateBodyPose();

            durtionTime = 0;
        }
        this.transform.position = new Vector3(head.position.x, floor, head.position.z);

    }
    protected virtual void LateUpdate()
    {
        AimScaling(true);
        AimScaling(false);

    }
    private void updateLockPosition()
    {
        if (needUpdateFootR)
        {
            needUpdateFootR = false;
            copyTransform(animator.GetBoneTransform(HumanBodyBones.RightFoot), lockFootRPosition);
        }
        if (needUpdateFootL)
        {
            needUpdateFootL = false;
            copyTransform(animator.GetBoneTransform(HumanBodyBones.LeftFoot), lockFootLPosition);
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetFloat("Height", height);
        animator.SetFloat("Forward", forward);
        animator.SetFloat("Right", right);
        //animator.SetBool("isSitting", isSitting);

        //Run check first, if true then set to bool value
        if (onCouch && isSitting)
        {
            //Check height value here - if low enough, then sit
            animator.SetBool("isSitting", true);
        }
        else
        {
            animator.SetBool("isSitting", false);
        }

        HandIK(true);
        HandIK(false);

        HeadIK();

        if (isLockFootR)
        {
            FootLock(true);
        }
        else
        {
            if (!smoothingFoot(true) && needFootIK)
            {
                FootIK(true);
            }
        }
        if (isLockFootL)
        {
            FootLock(false);
        }
        else
        {
            if (!smoothingFoot(false) && needFootIK)
            {
                FootIK(false);
            }
        }
        updateLockPosition();

    }
    private bool smoothingFoot(bool isRightFoot)
    {
        var smoothingTime = isRightFoot ? smoothingtimeR : smoothingtimeL;
        smoothingTime = smoothingTime + Time.deltaTime;
        var step = smoothingTime / smoothingFootTime;

        if (step > 1)
        {
            if (!isRightFoot)
            {
                testIsInSmoothing = false;
            }
            return false;
        }
        var currentPosition = isRightFoot ? lockFootRPosition.position : lockFootLPosition.position;
        Vector3 targetPosition;
        if (needFootIK)
        {
            targetPosition = FootStepScale(isRightFoot);
        }
        else
        {
            targetPosition = animator.GetBoneTransform(isRightFoot ? HumanBodyBones.RightFoot : HumanBodyBones.LeftFoot).position;
        }

        Vector3 newPos = Vector3.Lerp(currentPosition, targetPosition, step);
        if (!isRightFoot)
        {
            testLockLocation = newPos;
        }
        setFootPosition(isRightFoot, newPos);

        if (isRightFoot)
        {
            smoothingtimeR = smoothingTime;
        }
        else
        {
            smoothingtimeL = smoothingTime;
            testIsInSmoothing = true;
        }

        return true;
    }
    protected virtual void AimScaling(bool isRightHand)
    {
        Transform hand = animator.GetBoneTransform(isRightHand ? HumanBodyBones.RightHand : HumanBodyBones.LeftHand);
        Transform target = isRightHand ? targetR : targetL;
        if (hand.position != target.position)
        {

            Transform LArm = animator.GetBoneTransform(isRightHand ? HumanBodyBones.RightLowerArm : HumanBodyBones.LeftLowerArm);

            Transform UArm = animator.GetBoneTransform(isRightHand ? HumanBodyBones.RightUpperArm : HumanBodyBones.LeftUpperArm);
            Vector3 aimLenght = target.position - UArm.position;
            LArm.position = UArm.position + aimLenght / 2;
            // leftULArm.localScale = scaleIKDirection;
            hand.position = target.position;
        }

    }
    protected virtual void HandIK(bool isRightHand)
    {
        Transform target = isRightHand ? targetR : targetL;
        Transform hint = isRightHand ? hintR : hintL;
        AvatarIKGoal g = isRightHand ? AvatarIKGoal.RightHand : AvatarIKGoal.LeftHand;
        AvatarIKHint h = isRightHand ? AvatarIKHint.RightElbow : AvatarIKHint.LeftElbow;
        animator.SetIKPositionWeight(g, 1f);
        animator.SetIKPosition(g, target.position);
        animator.SetIKRotationWeight(g, 1f);
        animator.SetIKRotation(g, target.rotation);

        animator.SetIKHintPositionWeight(h, 1f);
        animator.SetIKHintPosition(h, hint.position);
    }
    private void HeadIK()
    {
        Vector3 target = head.TransformPoint(0, 0, 1);
        testLookTarget = target;
        animator.SetLookAtWeight(0.5f, 0.3f, 0.8f, 1);
        animator.SetLookAtPosition(target);

    }
    private void FootIK(bool isRightFoot)
    {
        Vector3 newPos = FootStepScale(isRightFoot);
        setFootPosition(isRightFoot, newPos);

    }
    private Vector3 FootStepScale(bool isRightFoot)
    {
        AvatarIKGoal g = isRightFoot ? AvatarIKGoal.RightFoot : AvatarIKGoal.LeftFoot;
        // get original position
        Vector3 footPos = animator.GetIKPosition(g);
        Vector3 localPos = this.transform.InverseTransformPoint(footPos);
        animator.speed = animationSpeed;
        Vector3 newPos;
        // Debug.Log("Right " + right + "forward" + forward);
        // scale with speed
        if (Mathf.Abs(right) > Mathf.Abs(forward))
        {
            newPos.x = localPos.x * speedScale * speedTreshold * footIKDirection.x;
            newPos.y = localPos.y * speedScale * speedTreshold * footIKDirection.y;
            newPos.z = localPos.z * footIKDirection.z;

        }
        else
        {
            newPos.x = localPos.x * footIKDirection.x;
            newPos.y = localPos.y * speedScale * speedTreshold * footIKDirection.y;
            newPos.z = localPos.z * speedScale * speedTreshold * footIKDirection.z;
        }

        return this.transform.TransformPoint(newPos);

    }
    private void FootLock(bool isRightFoot)
    {
        var currentPosition = isRightFoot ? lockFootRPosition.position : lockFootLPosition.position;
        setFootPosition(isRightFoot, currentPosition);
    }
    private void setFootPosition(bool isRightFoot, Vector3 footLockedPoint)
    {
        AvatarIKGoal g = isRightFoot ? AvatarIKGoal.RightFoot : AvatarIKGoal.LeftFoot;

        animator.SetIKPositionWeight(g, 1f);
        animator.SetIKPosition(g, footLockedPoint);
    }
    void OnDrawGizmos()
    {
        if (lockFootLPosition != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(lockFootLPosition.position, 0.1f);
            if (testIsInSmoothing)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(testLockLocation, 0.1f);
            }

        }
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(testLookTarget, 0.2f);

    }
    protected virtual void calculateBodyPose()
    {
        needFootIK = false;
        if (head.position != lastHeadTransform.position)
        {
            Vector3 headsetSpeed = (head.position - previousPos) / timeStep;
            headsetSpeed.y = 0;

            Vector3 headsetLocalSpeed = this.transform.InverseTransformDirection(headsetSpeed);
            previousPos = head.position;
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
        // Debug.Log("current "+head.position.ToString() + "::last "+lastHeadTransform.position.ToString());
    }
    protected virtual void copyTransform(Transform from, Transform to)
    {
        to.position = from.position;
        to.rotation = from.rotation;
        to.localScale = from.localScale;
    }

    public void lockFoot(int isRightFoot)
    {
        if (isRightFoot == 0)
        {
            isLockFootL = true;
            needUpdateFootL = true;
        }
        else
        {
            isLockFootR = true;
            needUpdateFootR = true;
        }
        updateLockPosition();
    }
    public void unlockFoot(int isRightFoot)
    {
        if (isRightFoot == 0)
        {
            if (isLockFootL)
            {
                isLockFootL = false;
                smoothingtimeL = 0;
                // Debug.Log("UnlockEvent");
            }
        }
        else
        {
            if (isLockFootR)
            {
                isLockFootR = false;
                smoothingtimeR = 0;
            }

        }

    }
}
