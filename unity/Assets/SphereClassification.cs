using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Runtime.InteropServices;

public class SphereClassification : MonoBehaviour
{
    [DllImport("ml_toolbox")]
    public static extern double *linear_create_model(int inputSize);

    [DllImport("ml_toolbox")]
    public static extern void linear_remove_model(double *model);

    [DllImport("ml_toolbox")]
    public static extern double linear_classify(double* model, double[] inputs, int inputSize);

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
        var model = linear_create_model(5);
        Debug.Log("toto");
        var res = linear_classify(model, model, 5);
        //Debug.Log("titi");
        //ml_toolbox.linear_remove_model(model);
        Debug.Log("end");
    }
}
