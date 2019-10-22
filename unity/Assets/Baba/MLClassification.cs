using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MLClassification : MonoBehaviour
{
    public Material Blue;

    public Material Red;

    public Material Green;

    public int NbSphere = 3;

    public int NbColor = 2;

    public int NbTraining = 100;

    public float Pas = 0.001f;

    public bool PerceptronMulti = false;

    public bool PreColor = false;

    private bool ModelCreated = false;

    private float MaxZ = 24;

    private IntPtr myModel;

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

    public void LaunchIAResult()
    {
        // On regarde toutes les sphères
        foreach (Transform child in this.transform)
        {
            // Créer Input
            double[] input = new Double[2];
            // Remplir Input
            input[0] = (double)child.position.y;
            input[1] = (double)child.position.z;
            // Calcul résultat
            double res = ml_toolbox.linear_classify(myModel, input, 2);
            // Affichage Debug
            //Debug.Log(child.gameObject.name + " : " + res);
            // Set Color des sphères
            switch (res)
            {
                case -1:
                    child.gameObject.GetComponent<Renderer>().material = Blue;
                    break;
                case 1:
                    child.gameObject.GetComponent<Renderer>().material = Red;
                    break;
            }
            double expectedvalue = 0;
            switch (child.gameObject.tag)
            {
                case "Blue":
                    expectedvalue = -1;
                    break;
                case "Red":
                    expectedvalue = 1;
                    break;
                case "Green":
                    expectedvalue = 0;
                    break;
            }

            if (res != expectedvalue)
            {
                Debug.Log(child.gameObject.name + " : Erreur");
            }
        }
    }

    public void TrainIA()
    {
        // On recréait le model si pas de model crée
        if (!ModelCreated) myModel = ml_toolbox.linear_create_model(2);
        ModelCreated = true;
        // Boucle d'entrainement entrainements
        for (int cpt = 0; cpt < NbTraining; cpt++)
        {
            // On regarde toutes les sphères
            foreach (Transform child in this.transform)
            {
                // Set expected value suvant couleur
                double expectedvalue = 0;
                switch (child.gameObject.tag)
                {
                    case "Blue":
                        expectedvalue = -1;
                        break;
                    case "Red":
                        expectedvalue = 1;
                        break;
                    case "Green":
                        expectedvalue = 0;
                        break;
                }
                // Créer Input
                double[] input = new Double[2];
                // Remplir Input
                input[0] = (double) child.position.y;
                input[1] = (double) child.position.z;
                // Calcul résultat
                double res = ml_toolbox.linear_classify(myModel, input, 2);
                // Vérifie et Fit
                ml_toolbox.linear_fit_classification(myModel, input, 2, Pas, expectedvalue, res);
            }
        }
        // Lance un test de l'IA
        LaunchIAResult();
    }

    public void RemoveModel()
    {
        ml_toolbox.linear_remove_model(myModel);
        ModelCreated = false;
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
        float mins = 0, medsup = 0, medsinf = 0, maxs = 0;
        switch (NbColor)
        {
            case 2:
                mins = -15f;
                medsup = 0f;
                medsinf = 0f;
                maxs = 15f;
                break;
            case 3:
                mins = -15f;
                medsup = 5f;
                medsinf = -5f;
                maxs = 15f;
                break;

        }
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
                if (PreColor) GO.GetComponent<Renderer>().material = Blue;
                Y = UnityEngine.Random.Range(medsup, maxs);
                GO.name = "Blue" + cpt+1;
                GO.tag = "Blue";
            }
            else if (cptb == 1)
            {
                if (PreColor) GO.GetComponent<Renderer>().material = Red;
                Y = UnityEngine.Random.Range(mins, medsinf);
                GO.name = "Red" + cpt + 1;
                GO.tag = "Red";
            }
            else
            {
                if (PreColor) GO.GetComponent<Renderer>().material = Green;
                Y = UnityEngine.Random.Range(medsinf, medsup);
                GO.name = "Green" + cpt + 1;
                GO.tag = "Green";
            }
            // Decalage en Z
            float Z = -(MaxZ) + ((float)cpt * 2 * (MaxZ + 1) / NbSphere);
            GO.transform.position = new Vector3(0f, Y, Z);
        }
    }

    public void InstantiationComplexe()
    {
        float mins = 0, medsup = 0, medsinf = 0, maxs = 0;
        switch (NbColor)
        {
            case 2:
                mins = -15f;
                medsup = -1f;
                medsinf = 1f;
                maxs = 15f;
                break;
            case 3:
                mins = -15f;
                medsup = 5f;
                medsinf = -5f;
                maxs = 15f;
                break;

        }
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
                if (PreColor) GO.GetComponent<Renderer>().material = Blue;
                Y = UnityEngine.Random.Range(medsup, maxs);
                GO.name = "Blue" + cpt + 1;
                GO.tag = "Blue";
            }
            else if (cptb == 1)
            {
                if (PreColor) GO.GetComponent<Renderer>().material = Red;
                Y = UnityEngine.Random.Range(mins, medsinf);
                GO.name = "Red" + cpt + 1;
                GO.tag = "Red";
            }
            else
            {
                if (PreColor) GO.GetComponent<Renderer>().material = Green;
                Y = UnityEngine.Random.Range(medsinf, medsup);
                GO.name = "Green" + cpt + 1;
                GO.tag = "Green";
            }
            // Decalage en Z
            float Z = -(MaxZ) + ((float)cpt * 2 * (MaxZ + 1) / NbSphere);
            GO.transform.position = new Vector3(0f, Y, Z);
        }
    }

    public void InstantiationHard()
    {
        // Création des sphere
        for (int cpt = 0; cpt < NbSphere; cpt++)
        {
            // Instantiate
            GameObject GO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GO.transform.SetParent(this.transform);
            // Making Color
            int cptb = 0;
            if (cpt < (NbSphere / 2))
            {
                cptb = 1;
            }
            float Y = 0f;
            float Z = 0f;
            if (cptb == 0)
            {
                if (PreColor) GO.GetComponent<Renderer>().material = Blue;
                if (cpt % 2 == 0)
                {
                    Y = UnityEngine.Random.Range(0.5f, MaxZ);
                    Z = UnityEngine.Random.Range(-MaxZ, -0.5f);
                }
                else
                {
                    Y = UnityEngine.Random.Range(-MaxZ, -0.5f);
                    Z = UnityEngine.Random.Range(0.5f, MaxZ);
                }
                GO.name = "Blue" + cpt + 1;
                GO.tag = "Blue";
            }
            else if (cptb == 1)
            {
                if (PreColor) GO.GetComponent<Renderer>().material = Red;
                if (cpt % 2 == 0)
                {
                    Y = UnityEngine.Random.Range(-15f, -0.5f);
                    Z = UnityEngine.Random.Range(-MaxZ, -0.5f);
                }
                else
                {
                    Y = UnityEngine.Random.Range(0.5f, 15f);
                    Z = UnityEngine.Random.Range(0.5f, MaxZ);
                }
                GO.name = "Red" + cpt + 1;
                GO.tag = "Red";
            }
            GO.transform.position = new Vector3(0f, Y, Z);
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
            float Z = 0f;
            if (cpt < NbSphere / 2)
            {
                cptb = 1;
            }
            if (cptb == 0)
            {
                if(PreColor) GO.GetComponent<Renderer>().material = Blue;
                Y = UnityEngine.Random.Range(0.5f, 15f);
                Z = UnityEngine.Random.Range(-MaxZ, -0.5f);
                GO.name = "Blue" + cpt + 1;
                GO.tag = "Blue";
            }
            else if (cptb == 1)
            {
                if (PreColor) GO.GetComponent<Renderer>().material = Red;
                Y = UnityEngine.Random.Range(-15f, -0.5f);
                Z = UnityEngine.Random.Range(0.5f, MaxZ);
                GO.name = "Red" + cpt + 1;
                GO.tag = "Red";
            }
            // Decalage en Z
            GO.transform.position = new Vector3(0f, Y, Z);
        }
    }
}
