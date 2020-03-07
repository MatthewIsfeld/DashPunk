using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public GameObject healthbarUI;

    public void setMaxHealth (int health)
    {
        healthbarUI.SetActive(true);
        slider.maxValue = health;
        slider.value = health;
        Debug.Log("max Health set on healthbar script.");
    }
    public void setHealth (int health)
    {
        slider.value = health;
        Debug.Log("Health set on healthbar script.");
    }

    public void setActive()
    {
        healthbarUI.SetActive(true);
    }

    public void setUnactive()
    {
        healthbarUI.SetActive(false);
    }

}
