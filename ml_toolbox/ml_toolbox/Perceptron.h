#pragma once


extern "C" {
	__declspec(dllexport) double *linear_create_model(int inputSize);
	__declspec(dllexport) void linear_remove_model(double *model);
	__declspec(dllexport) int linear_fit_regression(double *model, double *inputs, int inputSize/**/);
	__declspec(dllexport) int linear_fit_classification(double * model, double *input, int inputSize/**/);
	__declspec(dllexport) double linear_classify(double *model, double *inputs, int inputSize);
	__declspec(dllexport) double linear_predict(double *model, double *inputs, int inputSize);
}