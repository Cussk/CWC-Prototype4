using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    //public variables
    public float rotationSpeed; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //horizontal player input
        float horizontalInput = Input.GetAxis("Horizontal");
        //rotate camera on y-axis around focal point
        transform.Rotate(Vector3.up * horizontalInput * rotationSpeed * Time.deltaTime);
    }
}
