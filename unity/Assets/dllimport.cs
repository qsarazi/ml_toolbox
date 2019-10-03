using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class dllimport : MonoBehaviour
{
    [DllImport("ml_toolbox")]
    static extern double linear_classify(double [] model, double [] inputs, int inputSize);
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(linear_classify(null, null, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
