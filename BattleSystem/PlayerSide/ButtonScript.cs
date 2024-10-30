using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ButtonScript : Selectable, ISelectHandler
{
    public string title;
    public string description;
    public Sprite icon;
    public bool first;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Image imageD;

    protected override void Start()
    {
        
    }
    private void Update()
    {
        if (first)
        {
            Select();
            first = false;
        }
        if (IsHighlighted())
        {
            titleText.text = title;
            descriptionText.text = description;
            if (icon != null)
            {
                imageD.sprite = icon;
            }
        }
    }
    public override void OnSelect(BaseEventData eventData)
    {
        titleText.text = title;
        descriptionText.text = description;
        if (icon != null)
        {
            imageD.sprite = icon;
        }
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        titleText.text = "";
        descriptionText.text = "";
        imageD.sprite = null;
    }
    
   
}
