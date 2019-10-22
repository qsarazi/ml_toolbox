using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text TxtSphere;
    public Text TxtColor;

    private MLClassification _mlClassification;

    private void Start()
    {
        _mlClassification = GetComponent<MLClassification>();
    }

    public void SetNbSphere(Slider val)
    {
       _mlClassification.NbSphere = (int)val.value;
        TxtSphere.text = "Sphère : " +  _mlClassification.NbSphere;
    }
    public void SetNbColor(Slider val)
    {
        _mlClassification.NbColor = (int)val.value;
        TxtColor.text = "Couleurs : " + _mlClassification.NbColor;
    }
}
