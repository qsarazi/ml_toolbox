using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MLClassification : MonoBehaviour
{
    public Material Blue;

    public Material Red;

    public Material Green;

    public Dropdown Ddown;

    public GameObject BackField;

    public int NbSphere = 3;

    public int NbColor = 2;

    public int NbTraining = 100;

    public float Pas = 0.001f;

    public bool PerceptronMulti = false;

    public bool PreColor = false;

    private bool ModelCreated = false;

    private float MaxX = 24;

    private IntPtr myModel;

    private float ZSphere = 10f;

    void Start()
    {
        InitBAckField();
    }

    void Update()
    {
        // Quitter l'appli
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void InitBAckField()
    {
        for (int x = (int) -MaxX+1; x < MaxX; x++)
        {
            for (int y = -15; y < 15; y++)
            {
                InitImg(x, y);
            }
        }
    }

    public void InitImg(int x, int y)
    {
        GameObject Go = new GameObject();
        Go.transform.SetParent(BackField.transform);
        Go.AddComponent<Image>().color = Color.gray;
        Go.GetComponent<RectTransform>().sizeDelta = new Vector2(2f, 2f);
        Go.transform.position = BackField.transform.position;
        Go.GetComponent<RectTransform>().localPosition += new Vector3(20*x, 20*y,0);
    }

    public void Init()
    {
        switch (Ddown.value)
        {
            case 0:
                InstantiationSimple3();
                break;
            case 1:
                InstantiationSimple50();
                break;
            case 2:
                InstantiationSoft50();
                break;
            case 3:
                InstantiationXOR4();
                break;
            case 4:
                InstantiationCross50();
                break;
            case 5:
                break;
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
            input[1] = (double)child.position.x;
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
                input[0] = (double)child.position.y;
                input[1] = (double)child.position.x;
                // Calcul résultat
                double res = ml_toolbox.linear_classify(myModel, input, 2);
                // Vérifie et Fit
                ml_toolbox.linear_fit_classification(myModel, input, 2, Pas, expectedvalue, res);
            }
        }
        // Lance un test de l'IA
        LaunchIAResult();
    }

    public void LaunchIAResult2()
    {
        // On regarde toutes les sphères
        foreach (Transform child in BackField.transform)
        {
            // Créer Input
            double[] input = new Double[2];
            // Remplir Input
            input[1] = (double)child.position.y/2;
            input[0] = (double)child.position.x/2;
            
            // Calcul résultat
            double res = ml_toolbox.linear_classify(myModel, input, 2);
            // Affichage Debug
            Debug.Log(child.name + " : " + input[0] + "/" + input[1] + "/" + res);
            switch (res)
            {
                case -1:
                    child.gameObject.GetComponent<Image>().color = Color.blue;
                    break;
                case 1:
                    child.gameObject.GetComponent<Image>().color = Color.red;
                    break;
            }
        }
    }

    public void TrainIA2()
    {
        foreach (Transform child in this.transform)
        {
            Debug.Log(child.name + " : " + child.position.x + "/" + child.position.y);
        }
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
                input[1] = (double)child.position.y;
                input[0] = (double)child.position.x;
                //Debug.Log(child.name + " : " + input[0] + "/" + input[1]);
                // Calcul résultat
                double res = ml_toolbox.linear_classify(myModel, input, 2);
                // Vérifie et Fit
                ml_toolbox.linear_fit_classification(myModel, input, 2, Pas, expectedvalue, res);
            }
        }
        // Lance un test de l'IA
        LaunchIAResult2();
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

    public void OldInstantiation()
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
            float X = -(MaxX) + ((float)cpt * 2 * (MaxX + 1) / NbSphere);
            GO.transform.position = new Vector3(X, Y, ZSphere);
        }
    }

    public void InstantiationSoft()
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
            float X = -(MaxX) + ((float)cpt * 2 * (MaxX + 1) / NbSphere);
            GO.transform.position = new Vector3(X, Y, ZSphere);
        }
    }

    public void InstantiationXOR()
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
            float X = 0f;
            if (cptb == 0)
            {
                if (PreColor) GO.GetComponent<Renderer>().material = Blue;
                if (cpt % 2 == 0)
                {
                    Y = UnityEngine.Random.Range(0.5f, MaxX);
                    X = UnityEngine.Random.Range(-MaxX, -0.5f);
                }
                else
                {
                    Y = UnityEngine.Random.Range(-MaxX, -0.5f);
                    X = UnityEngine.Random.Range(0.5f, MaxX);
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
                    X = UnityEngine.Random.Range(-MaxX, -0.5f);
                }
                else
                {
                    Y = UnityEngine.Random.Range(0.5f, 15f);
                    X = UnityEngine.Random.Range(0.5f, MaxX);
                }
                GO.name = "Red" + cpt + 1;
                GO.tag = "Red";
            }
            GO.transform.position = new Vector3(X, Y, ZSphere);
        }
    }

    public void InstantiationCross()
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
            float X = 0f;
            if (cptb == 0)
            {
                if (PreColor) GO.GetComponent<Renderer>().material = Blue;
                if(UnityEngine.Random.Range(0, 2) == 1)Y = UnityEngine.Random.Range(-15f, -14f);
                else Y = UnityEngine.Random.Range(14f, 15f);
                if (UnityEngine.Random.Range(0, 2) == 1) X = UnityEngine.Random.Range(-MaxX, -MaxX+1f);
                else X = UnityEngine.Random.Range(MaxX-1f, MaxX);
                GO.name = "Blue" + cpt + 1;
                GO.tag = "Blue";
            }
            else if (cptb == 1)
            {
                if (PreColor) GO.GetComponent<Renderer>().material = Red;
                Y = UnityEngine.Random.Range(-15f, 15f);
                X = UnityEngine.Random.Range(-MaxX, MaxX);
                while (X < -MaxX+1 && (Y < -14f || Y > 14f))
                {
                    Y = UnityEngine.Random.Range(-15f, 15f);
                    X = UnityEngine.Random.Range(-MaxX, MaxX);
                }
                while (X > MaxX-1 && (Y < -14f || Y > 14f))
                {
                    Y = UnityEngine.Random.Range(-15f, 15f);
                    X = UnityEngine.Random.Range(-MaxX, MaxX);
                }
                GO.name = "Red" + cpt + 1;
                GO.tag = "Red";
            }
            GO.transform.position = new Vector3(X, Y, ZSphere);
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
            float X = 0f;
            if (cpt < NbSphere / 2)
            {
                cptb = 1;
            }
            if (cptb == 0)
            {
                if (PreColor) GO.GetComponent<Renderer>().material = Blue;
                Y = UnityEngine.Random.Range(0.5f, 15f);
                X = UnityEngine.Random.Range(-MaxX, -0.5f);
                GO.name = "Blue" + cpt + 1;
                GO.tag = "Blue";
            }
            else if (cptb == 1)
            {
                if (PreColor) GO.GetComponent<Renderer>().material = Red;
                Y = UnityEngine.Random.Range(-15f, -0.5f);
                X = UnityEngine.Random.Range(0.5f, MaxX);
                GO.name = "Red" + cpt + 1;
                GO.tag = "Red";
            }
            // Decalage en Z
            GO.transform.position = new Vector3(X, Y, ZSphere);
        }
    }

    public void InstantiationSimple3()
    {
        // Création des sphere
        for (int cpt = 0; cpt < 3; cpt++)
        {
            // Instantiate
            GameObject GO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GO.transform.SetParent(this.transform);
            // Making Color
            int cptb = 0;
            float Y = 0f;
            float Z = 0f;
            if (cpt == 0)
            {
                cptb = 1;
            }
            else if (cpt == 1)
            {
                cptb = 0;
            }
            else
            {
                cptb = UnityEngine.Random.Range(0, NbColor);
            }
            if (cptb == 0)
            {
                if (PreColor) GO.GetComponent<Renderer>().material = Blue;
                Y = UnityEngine.Random.Range(0.5f, 15f);
                Z = UnityEngine.Random.Range(-MaxX, -0.5f);
                GO.name = "Blue" + cpt + 1;
                GO.tag = "Blue";
            }
            else if (cptb == 1)
            {
                if (PreColor) GO.GetComponent<Renderer>().material = Red;
                Y = UnityEngine.Random.Range(-15f, -0.5f);
                Z = UnityEngine.Random.Range(0.5f, MaxX);
                GO.name = "Red" + cpt + 1;
                GO.tag = "Red";
            }
            // Decalage en Z
            GO.transform.position = new Vector3(Z, Y, ZSphere);
        }
    }

    public void InstantiationSimple50()
    {
        // Création des sphere
        for (int cpt = 0; cpt < 50; cpt++)
        {
            // Instantiate
            GameObject GO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GO.transform.SetParent(this.transform);
            // Making Color
            int cptb = 0;
            float Y = 0f;
            float Z = 0f;
            if (cpt < 25)
            {
                cptb = 1;
            }
            if (cptb == 0)
            {
                if (PreColor) GO.GetComponent<Renderer>().material = Blue;
                Y = UnityEngine.Random.Range(0.5f, 15f);
                Z = UnityEngine.Random.Range(-MaxX, -0.5f);
                GO.name = "Blue" + cpt + 1;
                GO.tag = "Blue";
            }
            else if (cptb == 1)
            {
                if (PreColor) GO.GetComponent<Renderer>().material = Red;
                Y = UnityEngine.Random.Range(-15f, -0.5f);
                Z = UnityEngine.Random.Range(0.5f, MaxX);
                GO.name = "Red" + cpt + 1;
                GO.tag = "Red";
            }
            // Decalage en Z
            GO.transform.position = new Vector3(Z, Y, ZSphere);
        }
    }

    public void InstantiationSoft50()
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
        for (int cpt = 0; cpt < 50; cpt++)
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
            float Z = -(MaxX) + ((float)cpt * 2 * (MaxX + 1) / 50);
            GO.transform.position = new Vector3(Z, Y, ZSphere);
        }
    }

    public void InstantiationXOR4()
    {
        // Création des sphere
        for (int cpt = 0; cpt < 4; cpt++)
        {
            // Instantiate
            GameObject GO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GO.transform.SetParent(this.transform);
            // Making Color
            int cptb = 0;
            if (cpt < 2)
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
                    Y = 15f;
                    Z = -MaxX;
                }
                else
                {
                    Y = -15f;
                    Z = MaxX;
                }
                GO.name = "Blue" + cpt + 1;
                GO.tag = "Blue";
            }
            else if (cptb == 1)
            {
                if (PreColor) GO.GetComponent<Renderer>().material = Red;
                if (cpt % 2 == 0)
                {
                    Y = -15f;
                    Z = -MaxX;
                }
                else
                {
                    Y = 15f;
                    Z = MaxX;
                }
                GO.name = "Red" + cpt + 1;
                GO.tag = "Red";
            }
            GO.transform.position = new Vector3(Z, Y, ZSphere);
        }
    }

    public void InstantiationCross50()
    {
        // Création des sphere
        for (int cpt = 0; cpt < 50; cpt++)
        {
            // Instantiate
            GameObject GO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GO.transform.SetParent(this.transform);
            // Making Color
            int cptb = UnityEngine.Random.Range(0, NbColor);
            float Y = 0f;
            float Z = 0f;
            if (cptb == 0)
            {
                if (PreColor) GO.GetComponent<Renderer>().material = Blue;
                if (UnityEngine.Random.Range(0, 2) == 1) Y = UnityEngine.Random.Range(-15f, -14f);
                else Y = UnityEngine.Random.Range(14f, 15f);
                if (UnityEngine.Random.Range(0, 2) == 1) Z = UnityEngine.Random.Range(-MaxX, -MaxX + 1f);
                else Z = UnityEngine.Random.Range(MaxX - 1f, MaxX);
                GO.name = "Blue" + cpt + 1;
                GO.tag = "Blue";
            }
            else if (cptb == 1)
            {
                if (PreColor) GO.GetComponent<Renderer>().material = Red;
                Y = UnityEngine.Random.Range(-15f, 15f);
                Z = UnityEngine.Random.Range(-MaxX, MaxX);
                while (Z < -MaxX + 1 && (Y < -14f || Y > 14f))
                {
                    Y = UnityEngine.Random.Range(-15f, 15f);
                    Z = UnityEngine.Random.Range(-MaxX, MaxX);
                }
                while (Z > MaxX - 1 && (Y < -14f || Y > 14f))
                {
                    Y = UnityEngine.Random.Range(-15f, 15f);
                    Z = UnityEngine.Random.Range(-MaxX, MaxX);
                }
                GO.name = "Red" + cpt + 1;
                GO.tag = "Red";
            }
            GO.transform.position = new Vector3(Z, Y, ZSphere);
        }
    }
}
