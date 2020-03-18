using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject inventoryDisplayUI;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnPauseMenu()
    {
        inventoryDisplayUI.SetActive(false);
    }
}
