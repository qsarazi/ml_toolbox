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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;
        Debug.Log("begin");
        var model = ml_toolbox.linear_create_model(5);
        Debug.Log("toto");
        var input = new Double[5];
        var res = ml_toolbox.linear_classify(model, input, 5);
        Debug.Log(res);
        ml_toolbox.linear_remove_model(model);
        Debug.Log("end");
    }
}
