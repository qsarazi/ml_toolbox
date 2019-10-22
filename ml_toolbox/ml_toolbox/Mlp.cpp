
#include "Mlp.h"

#include <random>
#include <iostream>

__declspec(dllexport) Mlp *mlp_create_model(int layerCount, int *npl) {
	auto model = new Mlp;

	model->layerCount = layerCount - 1;

	// because of C#'s garbage collector
	model->npl = new int[layerCount - 1];
	memcpy(model->npl, npl + 1, (layerCount - 1) * sizeof(int));

	// Weights
	model->weights = new double**[model->layerCount];
	for (int i = 0; i < model->layerCount; ++i) {
		model->weights[i] = new double*[npl[i] + 1];
		for (int j = 0; j <= npl[i]; ++j) {
			model->weights[i][j] = new double[npl[i + 1]];
			for (int k = 0; k < npl[i + 1]; ++k)
				model->weights[i][j][k] = rand() / double(RAND_MAX);
		}
	}

	// Ouputs
	model->outputs = new double*[model->layerCount];
	for (int i = 0; i < model->layerCount; ++i) {
		model->outputs[i] = new double[npl[i + 1]];
	}

	// Suums
	model->sums = new double*[model->layerCount];
	for (int i = 0; i < model->layerCount; ++i) {
		model->sums[i] = new double[npl[i + 1]];
	}

	// Sums
	model->sums = new double*[model->layerCount];
	for (int i = 0; i < model->layerCount; ++i) {
		model->sums[i] = new double[npl[i + 1]];
	}

	// Deltas
	model->deltas = new double*[model->layerCount];
	for (int i = 0; i < model->layerCount; ++i) {
		model->deltas[i] = new double[npl[i + 1]];
	}

	return model;
}

__declspec(dllexport) void mlp_remove_model(Mlp *model) {
	for (int i = 0; i < model->layerCount - 1; ++i) {
		for (int j = 0; j < model->npl[i]; ++j) {
			delete[] model->weights[i][j];
		}
		delete[] model->weights[i];
		delete[] model->outputs[i];
		delete[] model->sums[i];
		delete[] model->deltas[i];
	}
	delete[] model->weights;
	delete[] model->outputs;
	delete[] model->sums;
	delete[] model->deltas;
	delete[] model->npl;
	delete model;
}

__declspec(dllexport) int mlp_fit_regression(Mlp *model, double *inputs, int inputSize /**/);
__declspec(dllexport) int mlp_fit_classification(Mlp *model, double *inputs, int inputSize /**/);
__declspec(dllexport) double *mlp_predict(Mlp *model, double *inputs, int inputSize);
__declspec(dllexport) double *mlp_classify(Mlp *model, double *inputs, int inputSize);