#include "Perceptron.h"

#include <random>

__declspec(dllexport) double *linear_create_model(int inputSize) {
	auto model = new double[inputSize + 1];
	for (int i = 0; i < inputSize + 1; ++i)
		model[i] = rand() / double(RAND_MAX) - 0.5;
	return model;
}

__declspec(dllexport) void linear_remove_model(double *model) {
	delete[] model;
}

__declspec(dllexport) int linear_fit_regression(double *model, double *inputs, int inputSize/**/) {
	double xsum = 0, x2sum = 0, ysum = 0, xysum = 0;                //variables for sums/sigma of xi,yi,xi^2,xiyi etc

		xsum = xsum + inputs[0];                        //calculate sigma(xi)
		ysum = ysum + inputs[1];                        //calculate sigma(yi)
		x2sum = x2sum + pow(inputs[0], 2);                //calculate sigma(x^2i)
		xysum = xysum + inputs[0] * inputs[1];                    //calculate sigma(xi*yi)
	
	double a = (inputSize*xysum - xsum * ysum) / (inputSize*x2sum - xsum * xsum);            //calculate slope
	double b = (x2sum*ysum - xsum * xysum) / (x2sum*inputSize - xsum * xsum);            //calculate intercept
	for (int i = 0; i < inputSize+1; i++)
		model[i] = a * inputs[i] + b;
	return 0;
}

__declspec(dllexport) void linear_fit_classification(double *model, double *inputs, int inputSize, double step, double expectedValue, double value) {
	for (int i = 0; i < inputSize; ++i)
		model[i] += step * (expectedValue - value) * inputs[i];
	model[inputSize] += step * (expectedValue - value);
}

__declspec(dllexport) double linear_classify(double *model, double *inputs, int inputSize) {
	double sum = model[inputSize];

	for (int i = 0; i < inputSize; ++i)
		sum += model[i] * inputs[i];	
	return (sum > 0 ? 1.0 : -1.0);
}

__declspec(dllexport) double linear_predict(double *model, double *inputs, int inputSize) {
	double sum = model[inputSize];

	for (int i = 0; i < inputSize; ++i)
		sum += model[i];

	return sum;
}
