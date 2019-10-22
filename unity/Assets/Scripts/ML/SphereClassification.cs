using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SphereClassification : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("begin");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var model = ml_toolbox.linear_create_model(5);
            var input = new Double[5];
            var res = ml_toolbox.linear_classify(model, input, 5);
            Debug.Log(res);
            ml_toolbox.linear_remove_model(model); 
        }
            
        
        
    }
}
