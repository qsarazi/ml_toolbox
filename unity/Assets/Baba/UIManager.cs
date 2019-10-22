using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text TxtSphere;
    public Text TxtColor;

    public void SetNbSphere(Slider val)
    {
        this.GetComponent<MLClassification>().NbSphere = (int)val.value;
        TxtSphere.text = "Sphère : " + val;
    }
    public void SetNbColor(Slider val)
    {
        this.GetComponent<MLClassification>().NbColor = (int)val.value;
        TxtColor.text = "Couleurs : " + val;
    }
}
