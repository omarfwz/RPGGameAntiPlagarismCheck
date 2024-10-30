using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Button firstSelected;

    protected virtual void OnEnable()
    {
        try
        {
            SetFirstSelected(firstSelected);
        } 
        catch 
        {
            Debug.Log(gameObject);
        }
        
    }
    public void SetFirstSelected(Button firstSelectedButton)
    {
        firstSelectedButton.Select();
    }
}
