using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Door;
    private Vector3 originalPosition;

   
       
    void Start()
    {
        originalPosition = Door.transform.position;
        Debug.Log(originalPosition);
    }

    // Update is called once per frame
    void Update()
    {
       
        //Weird rotations were used becasue the inspector lies about the actual value.
        if (transform.rotation.x > 0.20)
        {  
           //Maybe add lerp later or dorr hinge.
           Door.transform.position = new Vector3(originalPosition.x - 5, originalPosition.y, originalPosition.z);
       
        }
         if (this.transform.rotation.x < -0.20)
        {
           
            Door.transform.position = originalPosition;
          
        }
    }
}
