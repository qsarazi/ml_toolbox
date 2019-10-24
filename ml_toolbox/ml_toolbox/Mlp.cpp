
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

//TODO recheck layer indices!!!! especcially check weight starting at 1
__declspec(dllexport) int mlp_fit_classification(Mlp *model, double *expectedOutputs, int inputSize, double step) {
	for (int j = 0; j < model->npl[model->layerCount - 1]; ++j) {
		model->deltas[model->layerCount - 1][j] = (1 - model->outputs[model->layerCount - 1][j] * model->outputs[model->layerCount - 1][j]) * (model->outputs[model->layerCount - 1][j] - expectedOutputs[j]);
	}

	for (int l = model->layerCount - 1; l >= 1; --l) {
		for (int i = 0; i < model->npl[l - 1]; ++i) {
			double sum = 0.0;
			for (int j = 0; j < model->npl[l]; ++j) {
				sum += model->weights[l][i][j] * model->deltas[l][j];
			}
			model->deltas[l - 1][i] = (1 - model->outputs[l - 1][i] * model->outputs[l - 1][i]) * sum;
		}
	}

	for (int l = 0; l < model->layerCount; ++l) {
		for (int i = 0; i < model->npl[l]; ++i) {
			for (int j = 0; j < model->npl[i + 1]; ++j) {
				model->weights[l][i][j] -= step * model->outputs[l][i];
			}
		}
	}
}

__declspec(dllexport) double *mlp_predict(Mlp *model, double *inputs, int inputSize) {
	for (int j = 0; j < model->npl[0]; ++j) {
		model->sums[0][j] = model->weights[0][model->npl[0]][j];
		for (int i = 0; i < inputSize; ++i) {
			model->sums[0][j] += inputs[i] * model->weights[0][i][j];
		}
		model->outputs[0][j] = tanh(model->sums[0][j]);
	}

	for (int l = 1; l < model->layerCount; ++l) {
		for (int j = 0; j < model->npl[0]; ++j) {
			model->sums[l][j] = model->weights[l][model->npl[l]][j];
			for (int i = 0; i < model->npl[l - 1]; ++i) {
				model->sums[l][j] += model->outputs[l - 1][i] * model->weights[l][i][j];
			}
			model->outputs[l][j] = tanh(model->sums[l][j]);
		}
	}

	for (int j = 0; j < model->npl[0]; ++j) {
		model->sums[model->layerCount - 1][j] = model->weights[model->layerCount - 1][model->npl[model->layerCount - 1]][j];
		for (int i = 0; i < model->npl[model->layerCount - 2]; ++i) {
			model->sums[model->layerCount - 1][j] += model->outputs[model->layerCount - 2][i] * model->weights[model->layerCount - 1][i][j];
		}
		model->outputs[model->layerCount - 1][j] = tanh(model->sums[model->layerCount - 1][j]);
	}

	return model->outputs[model->layerCount - 1];
}

__declspec(dllexport) double *mlp_classify(Mlp *model, double *inputs, int inputSize) {
	for (int j = 0; j < model->npl[0]; ++j) {
		model->sums[0][j] = model->weights[0][model->npl[0]][j];
		for (int i = 0; i < inputSize; ++i) {
			model->sums[0][j] += inputs[i] * model->weights[0][i][j];
		}
		model->outputs[0][j] = tanh(model->sums[0][j]);
	}
	
	for (int l = 1; l < model->layerCount; ++l) {
		for (int j = 0; j < model->npl[0]; ++j) {
			model->sums[l][j] = model->weights[l][model->npl[l]][j];
			for (int i = 0; i < model->npl[l - 1]; ++i) {
				model->sums[l][j] += model->outputs[l - 1][i] * model->weights[l][i][j];
			}
			model->outputs[l][j] = tanh(model->sums[l][j]);
		}
	}
	return model->outputs[model->layerCount - 1];
}