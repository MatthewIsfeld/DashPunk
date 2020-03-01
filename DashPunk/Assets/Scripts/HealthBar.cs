using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void setMaxHealth (int health)
    {
        slider.maxValue = health;
        Debug.Log(" max Health set on healthbar script.");
    }
    public void setHealth (int health)
    {
        slider.value = health;
        Debug.Log("Health set on healthbar script.");
    }
}
