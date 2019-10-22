using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLClassification : MonoBehaviour
{
    public Material Blue;

    public Material Red;

    public Material Green;

    public int NbSphere = 20;

    public int NbColor = 2;

    //private ml_toolbox MlTool;

    // Update is called once per frame
    void Update()
    {
        // Quitter l'appli
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void LaunchIA()
    {
        var model = ml_toolbox.linear_create_model(5);
        var input = new Double[5];
        var res = ml_toolbox.linear_classify(model, input, 5);
        Debug.Log(res);
        ml_toolbox.linear_remove_model(model);
    }

    public void ReInit()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void Instantiation()
    {
        // Création des sphere
        for (int cpt = 0; cpt < NbSphere; cpt++)
        {
            // Instantiate
            GameObject GO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GO.transform.SetParent(this.transform);
            // Making Color
            int cptb = UnityEngine.Random.Range(0, NbColor);
            float Y = 0f;
            if (cptb == 0)
            {
                GO.GetComponent<Renderer>().material = Blue;
                Y = UnityEngine.Random.Range(7.5f, 15f);
            }
            else if (cptb == 1)
            {
                GO.GetComponent<Renderer>().material = Red;
                Y = UnityEngine.Random.Range(-15f, -7.5f);
            }
            else
            {
                GO.GetComponent<Renderer>().material = Green;
                Y = UnityEngine.Random.Range(-7.5f, 7.5f);
            }
            // Decalage en Z
            GO.transform.position = new Vector3(0f, Y, -(((float)NbSphere - 1f)) + (float)cpt * 2);
        }
    }

    public void InstantiationSimple()
    {
        // Création des sphere
        for (int cpt = 0; cpt < NbSphere; cpt++)
        {
            // Instantiate
            GameObject GO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GO.transform.SetParent(this.transform);
            // Making Color
            int cptb = 0;
            float Y = 0f;
            if (cpt < NbSphere / 2)
            {
                cptb = 1;
            }
            if (cptb == 0)
            {
                GO.GetComponent<Renderer>().material = Blue;
                Y = UnityEngine.Random.Range(0, 15f);
            }
            else if (cptb == 1)
            {
                GO.GetComponent<Renderer>().material = Red;
                Y = UnityEngine.Random.Range(-15f, 0f);
            }
            // Decalage en Z
            GO.transform.position = new Vector3(0f, Y, -(((float)NbSphere - 1f)) + (float)cpt*2);
        }
    }
}
