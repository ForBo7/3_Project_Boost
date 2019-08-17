using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.W)) // 'W' has its own if statement so that turning while thrusting can happen
        {
            print("'W' pressed");
        }

        if (Input.GetKey(KeyCode.A))
        {
            print("'A' pressed");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("'D' pressed");
        } 
    }
}
