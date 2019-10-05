using System;
using System.Runtime.InteropServices;

public class ml_toolbox
{
    [DllImport("ml_toolbox")]
    public static extern IntPtr linear_create_model(int inputSize);

    [DllImport("ml_toolbox")]
    public static extern void linear_remove_model(IntPtr model);
    
    [DllImport("ml_toolbox")]
    public static extern int linear_fit_regression(IntPtr model, int inputSize, double step, double expectedValue, double value);

    [DllImport("ml_toolbox")]
    public static extern int linear_fit_classification(IntPtr model, int inputSize, double step, double expectedValue, double value);

    [DllImport("ml_toolbox")]
    public static extern double linear_classify(IntPtr model, [In, MarshalAs(UnmanagedType.LPArray)] double[] inputs, int inputSize);

    [DllImport("ml_toolbox")]
    public static extern double linear_predict(IntPtr model, double[] inputs, int inputSize);
}
