using System.Runtime.InteropServices;

public class ml_toolbox
{
   

   

    [DllImport("ml_toolbox")]
    public static extern int linear_fit_regression(double[] model, int inputSize, double step, double expectedValue, double value);

    [DllImport("ml_toolbox")]
    public static extern int linear_fit_classification(double[] model, int inputSize, double step, double expectedValue, double value);

   

    [DllImport("ml_toolbox")]
    public static extern double linear_predict(double[] model, double[] inputs, int inputSize);
}
