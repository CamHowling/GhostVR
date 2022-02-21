using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTesting : MonoBehaviour
{
    public float speedZ = 0;
    public float speedX = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x+speedX * Time.deltaTime ,
                this.transform.position.y ,this.transform.position.z+ speedZ * Time.deltaTime);
    }
}
