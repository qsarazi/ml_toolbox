#pragma once

#ifdef ML_TOOLBOX_EXPORTS
#define ML_TOOLBOX_API __declspec(dllexport)
#else
#define ML_TOOLBOX_API __declspec(dllimport)
#endif