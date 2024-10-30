using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileID = "";
    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI savePointText;
    [SerializeField] private TextMeshProUGUI personalityText;
    [SerializeField] private TextMeshProUGUI mentalStateText;
    public bool hasData;

    private Button saveSlotButton;

    private void Awake()
    {
        saveSlotButton = GetComponent<Button>();
    }
    public void SetData(PlayerData data)
    {
        if(data == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        else
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            savePointText.text = data.savePoint;
            personalityText.text = data.playerName;
        }
    }

    public string getProfileID()
    {
        return profileID;
    }
    
    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
    }


}
