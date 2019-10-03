#include "Perceptron.h"

__declspec(dllexport) double *linear_create_model(int inputSize) {
	return nullptr;
}

__declspec(dllexport) void linear_remove_model(double *model) {
	delete[] model;
}

__declspec(dllexport) int linear_fit_regression(double *model, double *inputs, int inputSize/**/) {
	return 0;
}

__declspec(dllexport) int linear_fit_classification(double * model, double *input, int inputSize/**/) {
	return 0;
}

__declspec(dllexport) double linear_classify(double *model, double *inputs, int inputSize) {
	return 0.0;
}

__declspec(dllexport) double linear_predict(double *model, double *inputs, int inputSize) {
	return 0.0;
}
