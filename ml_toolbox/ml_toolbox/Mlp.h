#pragma once

extern "C" {

	struct Mlp {
		double ***weights; //1 : layer, 2 : layer size, 3 : next layer size
		double **outputs;
		double **sums;
		double **deltas;
		int layerCount;
		int *npl; // neurons per layer
	};

	__declspec(dllexport) Mlp *mlp_create_model(int layerCount, int *npl);
	__declspec(dllexport) void mlp_remove_model(Mlp *model);
	__declspec(dllexport) int mlp_fit_regression(Mlp *model, double *inputs, int inputSize /**/);
	__declspec(dllexport) int mlp_fit_classification(Mlp *model, double *expectedOutputs, int inputSize, double step);
	__declspec(dllexport) double *mlp_predict(Mlp *model, double *inputs, int inputSize);
	__declspec(dllexport) double *mlp_classify(Mlp *model, double *inputs, int inputSize);
}