using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using TMPro;

public class PlayerGameplaySlot : MonoBehaviour
{
    public Slider health;
    public Slider bulletsProgress;
    public TextMeshProUGUI bulletsValue;
    // Use this for initialization
   


    public void UpdateBullets(float value, float max)
    {
        Debug.Log("UpdateBullets " + value + " " + max);
        if (bulletsProgress)
            bulletsProgress.value = value / max;

        if (bulletsValue)
            bulletsValue.text = value.ToString();
    }
    public void UpdateHealth(float value, float max)
    {
        Debug.Log("UpdateHealth " + value + " " + max);
        if (health)
            health.value = value / max;
    }
}
