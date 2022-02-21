using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class VRButton : MonoBehaviour
{
    public UnityEvent OnPress, OnRelease;

    [SerializeField] private float threshold = 0.1f;
    [SerializeField] private float deadzone = 0.025f;
    public GameObject ButtonObject;

    private bool IsPressed;
    private Vector3 startPos;
    private ConfigurableJoint joint;


    void Start()
    {
        startPos = transform.localPosition;
        joint = GetComponent<ConfigurableJoint>();

    }

    
    void Update()
    {
       if(transform.localPosition.y> startPos.y)
        {
            ButtonObject.transform.position = new Vector3(startPos.x,startPos.y,startPos.z);
        }
        if (!IsPressed && GetValue() + threshold >= 1)
        {
            Pressed();
        }

        if (IsPressed && GetValue() - threshold <= 0)
        {
            Released();
        }
    }

    private float GetValue()
    {
        var value = Vector3.Distance(startPos, transform.localPosition / joint.linearLimit.limit);

        //Might try System.Math
        if (Mathf.Abs(value) < deadzone)
         {
            value = 0;
         }
       
        return Mathf.Clamp(value, -1f, 1f);
    }

    private void Pressed()
    {
        IsPressed = true;
        OnPress.Invoke();
        Debug.Log("Pressed");
        
    }
    private void Released()
    {
        IsPressed = false;
        OnRelease.Invoke();
        Debug.Log("Released");
    }


}
