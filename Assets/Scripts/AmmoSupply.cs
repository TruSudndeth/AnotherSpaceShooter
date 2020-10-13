using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoSupply : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI AmmoMinMax;
    private int min = 0;
    private int max = 255;
    private int UpdateAmmo;
    void Start()
    {
        PlayerMoves.AmmoUpdate += UpdateCanvas;
    }

    private void UpdateCanvas(int AmmoUpdate)
    {
        if(AmmoUpdate > 255 || AmmoUpdate < 0)
        {
            AmmoMinMax.text = "Cheater Logged";
        }
        AmmoMinMax.text = AmmoUpdate + "/" + max; 
    }
}
